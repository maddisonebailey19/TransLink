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
    public class UserRelationshipStatus : ControllerBase
    {
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly JWTHelper _jWTHelper;

        public UserRelationshipStatus(IUserInfoInterface userInfoInterface, JWTHelper jWTHelper)
        {
            _userInfoInterface = userInfoInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,General")]
        [Route("GetRelationshipStatuses")]
        public IActionResult GetRelationshipStatuses()
        {
            List<UserRelationshipStatusData> response = _userInfoInterface.GetRelationshipStatuses();
            if(response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetUserRelationshipStatus")]
        public IActionResult GetUserRelationshipStatus()
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            UserRelationshipStatusData response = _userInfoInterface.GetUserRelationshipStatus(userNo);
            
            if(response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
            

        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddUserRelationshipStatus")]
        public IActionResult AddUserRelationshipStatus(UserRelationshipStatusData userRelationshipStatus)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            string locationUri = "api/UserRelationshipStatus/GetUserRelationshipStatus";

            bool response = _userInfoInterface.AddRelationshipStatus(userRelationshipStatus, userNo);
            if (response)
            {
                return Created(locationUri, response);
            }
            else
            {
                return UnprocessableEntity();
            }

        }

    }
}
