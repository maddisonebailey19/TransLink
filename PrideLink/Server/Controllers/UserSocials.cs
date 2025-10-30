using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.UserInfo;
using System.Data;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSocials : ControllerBase
    {
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly JWTHelper _jWTHelper;

        public UserSocials(IUserInfoInterface userInfoInterface, JWTHelper jWTHelper)
        {
            _userInfoInterface = userInfoInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddUpdateUserSocial")]
        public IActionResult AddUpdateUserSocial(UserSocial userSocial)
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string locationUri = "api/UserSocials/AddUpdateUserSocial";

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));
            bool response = _userInfoInterface.AddUpdateUserSocial(userSocial, userNo);
            if (response)
            {
                return Created(locationUri, response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetUserSocial")]
        public IActionResult GetUserSocial()
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string locationUri = "api/UserSocials/GetUserSocial";

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));
            List<UserSocial?> response = _userInfoInterface.GetUserSocial(userNo);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }

        }
    }
}
