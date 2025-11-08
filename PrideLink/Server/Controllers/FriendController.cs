using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrideLink.Server.Helpers;
using PrideLink.Server.Hubs;
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
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly JWTHelper _jWTHelper;
        public FriendController(IFriendInterface generalInterface, JWTHelper jWTHelper, IGmailInterface gmailInterface, IHubContext<NotificationHub> hubContext)
        {
            _friendInterface = generalInterface;
            _jWTHelper = jWTHelper;
            _gmailInterface = gmailInterface;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddFriend")]
        public async Task<IActionResult> AddFriend(int friendUserNo)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            NotificationContent emailContent = _friendInterface.AddFriend(userNo, friendUserNo);
            if(emailContent != null)
            {
                _gmailInterface.SendEmail(emailContent);
                Dictionary<int, string> message = new Dictionary<int, string>();
                message.Add(userNo, "You have a new friend request!");
                await _hubContext.Clients.User(friendUserNo.ToString()).SendAsync("FriendStatus", message);
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
        public IActionResult RemoveFriend(int friendUserNo)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            var response = _friendInterface.RemoveFriend(userNo, friendUserNo);
            return Ok(response);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,General")]
        [Route("AcceptFriendRequest")]
        public async Task<IActionResult> AcceptFriendRequest(int friendUserNo)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            NotificationContent emailContent = _friendInterface.AcceptFriendRequest(userNo, friendUserNo);
            if (emailContent != null)
            {
                _gmailInterface.SendEmail(emailContent);
                Dictionary<int,string> message = new Dictionary<int,string>();
                message.Add(userNo, emailContent.EmailContents.FirstOrDefault(e => e.Key == "@newFriendUserName").Value + " has accepted your friend request!");
                await _hubContext.Clients.User(friendUserNo.ToString()).SendAsync("FriendStatus",message);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
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
