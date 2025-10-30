using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.Internal_Models;
using PrideLink.Shared.LoginDetails;
using PrideLink.Shared.UserOptIn;
using System.Data;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOptIns : ControllerBase
    {
        private readonly IUserOptInInterface _userOptInInterface;
        private readonly JWTHelper _jWTHelper;

        public UserOptIns(IUserOptInInterface userOptInInterface, JWTHelper jWTHelper)
        {
            _userOptInInterface = userOptInInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("UserOptIn")]
        public IActionResult UserOptInOut(UserOptIn userOptIns)
        {
            UserOptInsResponse userOptInsResponse = new UserOptInsResponse();
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ","");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));
            bool response = _userOptInInterface.UserOptIn(userOptIns, userNo);

            return Ok(response);
        }
    }
}
