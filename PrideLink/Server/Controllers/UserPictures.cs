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
    public class UserPictures : ControllerBase
    {
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly JWTHelper _jWTHelper;

        public UserPictures(IUserInfoInterface userInfoInterface, JWTHelper jWTHelper)
        {
            _userInfoInterface = userInfoInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddUserPicture")]
        public IActionResult AddUserPicture(UserPicture userPicture)
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));
            bool response = _userInfoInterface.AddUserPicture(userPicture, userNo);

            string locationUri = "api/UserPictures/GetUserPictures";

            if (response)
            {
                return Created(locationUri, response);
            }
            else
            {
                return UnprocessableEntity();
            }

        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("GetUserPictures")]
        public IActionResult GetUserPictures(UserPicture userPicture)
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));
            UserPicture response = _userInfoInterface.GetUserPicture(userPicture, userNo);
            if(response == null)
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
