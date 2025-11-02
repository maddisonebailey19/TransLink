using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.General;
using PrideLink.Shared.UserInfo;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PrideLink.Server.Hubs;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class General : ControllerBase
    {
        private readonly IGeneralInterface _generalInterface;
        private readonly JWTHelper _jWTHelper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public General(IGeneralInterface generalInterface, JWTHelper jWTHelper, IHubContext<NotificationHub> hubContext)
        {
            _generalInterface = generalInterface;
            _hubContext = hubContext;
            this._jWTHelper = jWTHelper;
        }


        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetHobbies")]
        public IActionResult GetHobbies()
        {
            List<Hobbys> response = _generalInterface.GetHobbies();
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("UserVerificationStatus")]
        public IActionResult GetVerificationStatus()
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            string userStatus = _generalInterface.UserVerificationStatus(userNo);
            return Ok(userStatus);
        }

        [HttpPatch]
        //[Authorize(Roles = "Admin,General")]
        [Route("UpdateVerificationStatus")]
        public async Task<IActionResult> UpdateVerificationStatus(int userNo, string userTypeName)
        {
            _generalInterface.UpdateVerificationStatus(userNo, userTypeName);

            var message = "Your account has been verified!";//🎉
            await _hubContext.Clients.User(userNo.ToString()).SendAsync("ReceiveNotification", message);
            
            return Ok();
        }
    }
}
