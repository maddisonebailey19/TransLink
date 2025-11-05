using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.LoginDetails;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSettings : ControllerBase
    {
        private readonly IAccountSettingsInterface _accountSettingsInterface;
        private readonly JWTHelper _jWTHelper;
        public AccountSettings(IAccountSettingsInterface accountSettingsInterface, JWTHelper jWTHelper)
        {
            _accountSettingsInterface = accountSettingsInterface;
            _jWTHelper = jWTHelper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("IsPasswordCorrect")]
        public IActionResult IsPasswordCorrect(string password)
        {
            try
            {
                var jwtToken = Request.Cookies["AuthToken"];

                int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

                string user = _accountSettingsInterface.CheckLoginCredWithUserNo(userNo, password);

                if (user == null)
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok();
                }
            }
            catch
            {
                // If anything went wrong, token is invalid or expired
                return Unauthorized();
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

            bool result = _accountSettingsInterface.UpdatePassword(password, userNo);
            if (result)
            {
                return Ok();
            }
            else
            {
                return UnprocessableEntity();
            }

        }

        [HttpPatch]
        [Authorize(Roles = "Admin,General")]
        [Route("Update-Email")]
        public IActionResult UpdateEmail(string email)
        {
            UserCreated login = new UserCreated();

            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            bool result = _accountSettingsInterface.UpdateEmail(email, userNo);
            if (result)
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
        [Route("IsEmailCorrect")]
        public IActionResult IsEmailCorrect(string email)
        {
            UserCreated login = new UserCreated();

            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            bool result = _accountSettingsInterface.IsEmailCorrect(userNo, email);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
