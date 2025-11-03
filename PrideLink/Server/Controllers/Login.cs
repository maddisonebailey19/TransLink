using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.Internal_Models;
using PrideLink.Shared.LoginDetails;
using RTools_NTS.Util;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        private readonly ILoginInterface _loginInterface;
        private readonly IConfiguration _configuration;
        private readonly JWTHelper _jWTHelper;

        public Login(ILoginInterface loginInterface, IConfiguration configuration, JWTHelper jWTHelper)
        {
            _loginInterface = loginInterface;
            _configuration = configuration;
            this._jWTHelper = jWTHelper;
        }

        [HttpGet]
        [Route("Login-Checker")]
        public IActionResult CheckLoginCreds(string userName, string password)
        {
            string userID = _loginInterface.CheckLoginCred(userName, password);
            if(userID != null)
            {
                List<UserRoles> userRoles = _loginInterface.GetRoles(userID);
                List<string> roles = new List<string>();
                foreach (var role in userRoles)
                {
                    roles.Add(role.roleName);
                }
                DateTime expireDate = DateTime.Now;
                expireDate = expireDate.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"]));


                var token = _jWTHelper.GenerateJwtToken(userID, roles);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,          // cannot be accessed via JS
                    Secure = true,            // only sent via HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("AuthToken", token, cookieOptions);

                return Ok();
            }
            else
            {
                return Unauthorized();
            }
            
        }

        [HttpGet]
        [Route("IsUserLoggedIn")]
        public IActionResult IsUserLoggedIn()
        {
            try
            {
                // If the request reached here, the JWT was valid
                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                             ?? User.FindFirstValue("userID");

                return Ok();
            }
            catch
            {
                // If anything went wrong, token is invalid or expired
                return Unauthorized(new { isLoggedIn = false });
            }
        }

        [HttpGet]
        [Route("Login-Creator")]
        public IActionResult LoginCreator(string userName, string password)
        {
            UserCreated login = new UserCreated();
            login.userNo = _loginInterface.CreateLogin(userName, password);

            string locationUri = "api/Login/Login-Checker";

            if(login.userNo == -1)
            {
                return UnprocessableEntity();
            }
            else
            {
                return Created(locationUri, login);
            }
            
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,General")]
        [Route("Update-Password")]
        public IActionResult UpdatePassword(string password)
        {
            UserCreated login = new UserCreated();

            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            bool result = _loginInterface.UpdatePassword(password, userNo);
            if(result)
            {
                return Ok();
            }
            else
            {
                return UnprocessableEntity();
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("IsUserInRole")]
        public IActionResult IsUserInRole(string roleName)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            List<string> roles = _jWTHelper.GetRoles(jwtToken);

            if (roles.Contains(roleName))
            {
                return Ok();
            }
            else
            {
                return Forbid();
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetUserID")]
        public IActionResult GetUserID()
        {
            var jwtToken = Request.Cookies["AuthToken"];

            string userNo = _jWTHelper.GetUserNo(jwtToken);

            return Ok(userNo);

        }
        //TODO patch update password
    }
}
