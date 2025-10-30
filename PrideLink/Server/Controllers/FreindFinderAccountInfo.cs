using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.FreindFinderDetails;
using PrideLink.Shared.General;
using PrideLink.Shared.Location;
using System.Data;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreindFinderAccountInfo : ControllerBase
    {
        private readonly IUserInfoInterface _userInfoInterface;
        private readonly ILocationInterface _locationInterface;
        private readonly IFreindFinderUserInfoInterface _freindFinderUserInfoInterface;
        private readonly JWTHelper _jWTHelper;

        public FreindFinderAccountInfo(IUserInfoInterface userInfoInterface, ILocationInterface locationInterface, IFreindFinderUserInfoInterface freindFinderUserInfoInterface, JWTHelper jWTHelper)
        {
            _userInfoInterface = userInfoInterface;
            _locationInterface = locationInterface;
            _freindFinderUserInfoInterface = freindFinderUserInfoInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")] //currently not working need to fix
        [Route("GetFreindFinderAccountInfo")]
        public IActionResult GetFreindFinderAccountInfo(UserLocationData userLocation)
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            List<UserFreindFinderAccount?> response = _freindFinderUserInfoInterface.GetAllUserFreindFinderAccounts(userLocation);
            response.Remove(response.FirstOrDefault(e => e.userNo == userNo));
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
