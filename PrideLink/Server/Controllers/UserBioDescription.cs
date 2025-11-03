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
    public class UserBioDescription : ControllerBase
    {
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly JWTHelper _jWTHelper;

        public UserBioDescription(IUserInfoInterface userInfoInterface, JWTHelper jWTHelper)
        {
            _userInfoInterface = userInfoInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddUserBioDescription")]
        public IActionResult AddUserBioDescription(UserBioDescriptionData userBioDescription)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            string locationUri = "api/UserBioDescription/GetUserBioDescription";

            bool response = _userInfoInterface.AddUserBioDescription(userBioDescription, userNo);
            if (response)
            {
                return Created(locationUri, response);
            }
            else
            {
                return UnprocessableEntity();
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetUserBioDescription")]
        public IActionResult GetUserBioDescription()
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            UserBioDescriptionData response = _userInfoInterface.GetUserBioDescription(userNo);
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
