using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Interfaces;

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

        [HttpGet]
        [Route("SendEmail")]
        public IActionResult SendEmail(List<(string Name, string value)> emailContents)
        {
            _gmailInterface.SendEmail(emailContents);
            return Ok();
        }
    }
}
