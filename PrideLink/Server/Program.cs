using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using PrideLink.Server.Controllers;
using PrideLink.Server.Helpers;
using PrideLink.Server.Hubs;
using PrideLink.Server.Interfaces;
using PrideLink.Server.Internal_Models;
using PrideLink.Server.TransLinkDataBase;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddSignalR();
// Add services to the container.

// If your Jwt:Key is base64 (as in appsettings.json) decode it consistently:
var key = Convert.FromBase64String(builder.Configuration["Jwt:Key"] ?? string.Empty);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // For development, consider true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    // Optional: log auth failures to console to diagnose 401/CORS vs auth issues
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("AuthToken"))
            {
                context.Token = context.Request.Cookies["AuthToken"];
            }
            return Task.CompletedTask;
        },
        // Optional: log auth failures
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine("JWT auth failed: " + ctx.Exception);
            return Task.CompletedTask;
        }
    };
});

// Register a CORS policy that allows the client origins used in launchSettings.
// For development you can add all the localhost origins you use.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalDev", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7135", // client https
                "http://localhost:7136",  // client http
                "https://localhost:7125", // server https (if calling directly)
                "http://localhost:7126"   // server http (if calling directly)
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
            // .AllowCredentials(); // only if you use cookies/credentials - do NOT use with AllowAnyOrigin
    });
});

builder.Services.AddDbContext<MasContext>(options =>
    options.UseSqlServer("Server=tcp:maddisonbailey.database.windows.net,1433;Database=MAS;User ID=Maddi;Password=Cobilove19;Trusted_Connection=False;Encrypt=True;", sqlOptions =>
        sqlOptions.UseNetTopologySuite()));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<JWTHelper>();
builder.Services.AddSingleton<InsertIntoGenericReference>();
builder.Services.AddScoped<ILoginInterface, LoginDetailsHelper>();
builder.Services.AddScoped<IUserOptInInterface, OptInHelpercs>();
builder.Services.AddScoped<IUserInfoInterface, UserInfoHelper>();
builder.Services.AddScoped<IGeneralInterface, GeneralHelper>();
builder.Services.AddScoped<ILocationInterface, LocationHelper>();
builder.Services.AddScoped<IFreindFinderUserInfoInterface, FreindFinderUserAccountHelper>();
builder.Services.AddScoped<IGmailInterface, GmailHelper>();
builder.Services.AddScoped<IAccountSettingsInterface, AccountSettingsHelper>();
builder.Services.AddScoped<IFriendInterface, FriendHelper>();
builder.Services.AddScoped<PasswordHelper>();
builder.Services.AddSingleton<EmailVerificationStore>();
builder.Services.AddScoped<Random>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANT: Apply CORS after UseRouting and before auth/endpoints
app.UseCors("AllowLocalDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationhub");
app.MapFallbackToFile("index.html");

app.Run();
