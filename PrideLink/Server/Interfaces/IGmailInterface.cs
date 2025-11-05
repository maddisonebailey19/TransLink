using PrideLink.Shared.General;
using PrideLink.Shared.Notification;

namespace PrideLink.Server.Interfaces
{
    public interface IGmailInterface
    {
        public void SendEmail(NotificationContent emailContents);
        public void VerificationEmailSender(NotificationContent emailContents);
    }
}
