using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.FreindFinderDetails;
using PrideLink.Shared.General;
using PrideLink.Shared.Location;
using PrideLink.Shared.Notification;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendInterface _friendInterface;
        private readonly IGmailInterface _gmailInterface;
        private readonly JWTHelper _jWTHelper;
        public FriendController(IFriendInterface generalInterface, JWTHelper jWTHelper, IGmailInterface gmailInterface)
        {
            _friendInterface = generalInterface;
            _jWTHelper = jWTHelper;
            _gmailInterface = gmailInterface;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddFriend")]
        public IActionResult AddFriend(int friendUserNo)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            NotificationContent emailContent = _friendInterface.AddFriend(userNo, friendUserNo);
            if(emailContent != null)
            {
                _gmailInterface.SendEmail(emailContent);
                return Ok();
            }
            else
            {
                return BadRequest();
            }

                
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,General")]
        [Route("RemoveFriend")]
        public IActionResult RemoveFriend(int userNo, int friendUserNo)
        {
            var response = _friendInterface.RemoveFriend(userNo, friendUserNo);
            return Ok(response);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,General")]
        [Route("AcceptFriendRequest")]
        public IActionResult AcceptFriendRequest(int friendUserNo)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            var response = _friendInterface.AcceptFriendRequest(userNo, friendUserNo);
            return Ok(response);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,General")]
        [Route("DeclineFriendRequest")]
        public IActionResult DeclineFriendRequest(int userNo, int friendUserNo)
        {
            var response = _friendInterface.DeclineFriendRequest(userNo, friendUserNo);
            return Ok(response);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,General")]
        [Route("BlockUser")]
        public IActionResult BlockUser(int userNo, int friendUserNo)
        {
            var response = _friendInterface.BlockUser(userNo, friendUserNo);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")] //currently not working need to fix
        [Route("GetFreindFinderAccountInfo")]
        public IActionResult GetFreindFinderAccountInfo()
        {
            //var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            List<UserFreindFinderAccount?> response = _friendInterface.GetAllUserFriends(userNo);
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
