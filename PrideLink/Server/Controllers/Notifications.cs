using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.Notification;
using System.Text.Json;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Notifications : ControllerBase
    {
        private readonly IGmailInterface _gmailInterface;
        public Notifications(IGmailInterface gmailInterface)
        {
            _gmailInterface = gmailInterface;
        }

        [HttpPost]
        [Route("SendEmail")]
        public IActionResult SendEmail(NotificationContent emailContentsJson)
        {
            //NotificationContent emailContents = JsonSerializer.Deserialize<NotificationContent>(emailContentsJson);
            //List<(string Name, string value)>
            _gmailInterface.SendEmail(emailContentsJson);
            return Ok();
        }

        [HttpPost]
        [Route("VerificationSendEmail")]
        public IActionResult VerificationSendEmail(NotificationContent emailContentsJson)
        {
            //NotificationContent emailContents = JsonSerializer.Deserialize<NotificationContent>(emailContentsJson);
            //List<(string Name, string value)>
            _gmailInterface.VerificationEmailSender(emailContentsJson);
            return Ok();
        }
    }
}
