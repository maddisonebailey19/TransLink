using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.General;
using System.Data;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserHobby : ControllerBase
    {
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly JWTHelper _jWTHelper;

        public UserHobby(IUserInfoInterface userInfoInterface, JWTHelper jWTHelper)
        {
            _userInfoInterface = userInfoInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddUserHobbies")]
        public IActionResult AddUserHobbies(List<Hobbys> hobbys)
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            bool response = _userInfoInterface.AddUserHobbies(hobbys, userNo);
            if (response)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetUserHobbies")]
        public IActionResult GetUserHobbies()
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            List<Hobbys?> hobbys = _userInfoInterface.GetHobbys(userNo);
            if(hobbys == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(hobbys);
            }
            
        }
    }
}
