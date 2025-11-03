using Microsoft.IdentityModel.Tokens;
using PrideLink.Server.Interfaces;
using PrideLink.Server.TransLinkDataBase;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrideLink.Server.Helpers
{
    public class JWTHelper
    {
        private readonly IConfiguration _config;
        public JWTHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateJwtToken(string userId, List<string> roles)
        {
            // Decode the base64 key
            var keyBytes = Convert.FromBase64String(_config["Jwt:Key"]);
            var key = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Core claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),           // standard claim
                new Claim("userID", userId),                              // custom claim
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Role claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string? GetUserNo(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var keyBytes = Convert.FromBase64String(_config["Jwt:Key"]);
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userID = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "userID")?.Value
                ?? claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                using (var context = new MasContext())
                {
                    int userNo = context.TblUsers.FirstOrDefault(e => e.UserId == userID).UserNo;
                    return userNo.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }
        private ClaimsPrincipal? ValidateTokenAndGetPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var keyBytes = Convert.FromBase64String(_config["Jwt:Key"]);
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }

        public List<string> GetRoles(string token)
        {
            var principal = ValidateTokenAndGetPrincipal(token);
            if (principal == null) return new List<string>();

            // roles are issued as ClaimTypes.Role by GenerateJwtToken
            var roles = principal.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .ToList();

            return roles;
        }
    }
}
