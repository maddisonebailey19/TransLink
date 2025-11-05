using MailKit.Net.Smtp;
using MailKit.Net.Smtp;
using MimeKit;
using PrideLink.Server.Controllers;
using PrideLink.Server.Interfaces;
using PrideLink.Server.Internal_Models;
using PrideLink.Server.TransLinkDataBase;
using PrideLink.Shared.LoginDetails;
using PrideLink.Shared.Notification;
using System.Net.Mail;
using System.Threading.Tasks;
namespace PrideLink.Server.Helpers
{
    public class GmailHelper : IGmailInterface
    {
        private EmailVerificationStore _emailVerificationStore;
        private Random _random;
        public GmailHelper(EmailVerificationStore emailVerificationStore, Random random)
        {
            _emailVerificationStore = emailVerificationStore;
            _random = random;
        }

        public void SendEmail(NotificationContent emailContents)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TransLink", "maddisonebailey19@gmail.com"));
            message.To.Add(MailboxAddress.Parse(emailContents.ToEmail));
            message.Subject = emailContents.Subject;

            var htmlBody = GetEmailTemplate(emailContents.EmailContentNo);
            foreach (var kvp in emailContents.EmailContents)
            {
                htmlBody = htmlBody.Replace(kvp.Key, kvp.Value);
            }
            // Build a pretty HTML body
            //var htmlBody = $@"
            //    <html>
            //    <body style='font-family: Arial, sans-serif; background-color: #f9f9fb; padding: 20px;'>
            //      <table style='max-width:600px; margin:auto; background:#fff; border-radius:10px; box-shadow:0 2px 6px rgba(0,0,0,0.1);'>
            //        <tr>
            //          <td style='background:#6f42c1; color:#fff; text-align:center; padding:20px; font-size:24px; border-top-left-radius:10px; border-top-right-radius:10px;'>
            //            TransLink
            //          </td>
            //        </tr>
            //        <tr>
            //          <td style='padding:30px; color:#333;'>
            //            <h2 style='color:#6f42c1;'>Hey {username}!</h2>
            //            <p>Welcome to <b>TransLink</b> — we’re so happy to have you here. 💜</p>
            //            <p>Click below to verify your email and start connecting with other trans folks:</p>
            //            <p style='text-align:center;'>
            //              <a href='https://linkpride.duckdns.org/' 
            //                 style='background:#6f42c1; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none;'>
            //                Verify My Email
            //              </a>
            //            </p>
            //            <p>With love,<br>The TransLink Team 🌈</p>
            //          </td>
            //        </tr>
            //        <tr>
            //          <td style='background:#f2f2f2; color:#666; font-size:12px; text-align:center; padding:10px; border-bottom-left-radius:10px; border-bottom-right-radius:10px;'>
            //            © 2025 TransLink. You’re receiving this email because you signed up for an account.
            //          </td>
            //        </tr>
            //      </table>
            //    </body>
            //    </html>";


            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            // For testing only, bypass certificate validation
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            try
            {
                // Connect synchronously
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Authenticate
                client.Authenticate("maddisonebailey19@gmail.com", "pmuz sveu mzds bwth");

                // Send the message
                client.Send(message);

                // Disconnect
                client.Disconnect(true);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public void VerificationEmailSender(NotificationContent emailContents)
        {
            string userID;
            string userName = emailContents.EmailContents.FirstOrDefault(e => e.Key == "@userName").Value;
            using (var context = new MasContext())
            {
                userID = context.TblUsers.FirstOrDefault(e => e.Login == userName).UserId;
            }

            int verificationNo = _random.Next(1000, 10000);

            EmailVerification emailVerification = new EmailVerification
            {
                userID = userID,
                verificationCode = verificationNo
            };
            _emailVerificationStore.EmailVerification.Add(emailVerification);
            emailContents.EmailContents.Add("@verificationNo", verificationNo.ToString());

            SendEmail(emailContents);
        }

        private string GetEmailTemplate(int emailContentNo)
        {
            using (var context = new MasContext())
            {
                return context.TblEmailContents.FirstOrDefault(e => e.EmailContentNo == emailContentNo).EmailContent;
            }
        }
    }
}
