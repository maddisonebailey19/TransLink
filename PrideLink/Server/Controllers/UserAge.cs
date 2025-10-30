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
    public class UserAge : ControllerBase
    {
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly JWTHelper _jWTHelper;

        public UserAge(IUserInfoInterface userInfoInterface, JWTHelper jWTHelper)
        {
            _userInfoInterface = userInfoInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddUserAge")]
        public IActionResult AddUserAge(Age age)
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            string locationUri = "api/UserAge/GetUserAge";

            bool response = _userInfoInterface.AddUserAge(age, userNo);
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
        [Route("GetUserAge")]
        public IActionResult GetUserAge()
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            Age response = _userInfoInterface.GetUserAge(userNo);

            if(response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
