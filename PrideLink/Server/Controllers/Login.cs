using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.Internal_Models;
using PrideLink.Shared.LoginDetails;
using System.Data;
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
            jwtToken response = new jwtToken();
            int userNo = _loginInterface.CheckLoginCred(userName, password);
            if(userNo != -1)
            {
                List<UserRoles> userRoles = _loginInterface.GetRoles(userNo);
                List<string> roles = new List<string>();
                foreach (var role in userRoles)
                {
                    roles.Add(role.roleName);
                }
                DateTime expireDate = DateTime.Now;
                expireDate = expireDate.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"]));
                response.token = _jWTHelper.GenerateJwtToken(userNo.ToString(), roles);
                response.expireDate = expireDate;
                return Ok(response);
            }
            else
            {
                return NotFound();
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

            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

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
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

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
        //TODO patch update password
    }
}
