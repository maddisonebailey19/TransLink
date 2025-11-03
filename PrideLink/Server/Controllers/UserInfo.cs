using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.General;
using PrideLink.Shared.UserInfo;
using PrideLink.Shared.UserOptIn;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfo : ControllerBase
    {
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly JWTHelper _jWTHelper;

        public UserInfo(IUserInfoInterface userInfoInterface, JWTHelper jWTHelper)
        {
            _userInfoInterface = userInfoInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddRemoveEmail")]
        public IActionResult AddRemoveEmail(Email email)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));
            bool response = _userInfoInterface.AddRemoveEmailFromUser(email, userNo);
            if(response)
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