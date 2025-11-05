using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using PrideLink.Server.Interfaces;
namespace PrideLink.Server.Helpers
{
    public class GmailHelper : IGmailInterface
    {
        public void SendEmail()
        {
            string toEmail = "angel.c.quinzel@gmail.com";
            string username = "Angel";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TransLink", "maddisonebailey19@gmail.com"));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "🌈 Welcome to TransLink!";

            // Build a pretty HTML body
            var htmlBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f9f9fb; padding: 20px;'>
                  <table style='max-width:600px; margin:auto; background:#fff; border-radius:10px; box-shadow:0 2px 6px rgba(0,0,0,0.1);'>
                    <tr>
                      <td style='background:#6f42c1; color:#fff; text-align:center; padding:20px; font-size:24px; border-top-left-radius:10px; border-top-right-radius:10px;'>
                        TransLink
                      </td>
                    </tr>
                    <tr>
                      <td style='padding:30px; color:#333;'>
                        <h2 style='color:#6f42c1;'>Hey {username}!</h2>
                        <p>Welcome to <b>TransLink</b> — we’re so happy to have you here. 💜</p>
                        <p>Click below to verify your email and start connecting with other trans folks:</p>
                        <p style='text-align:center;'>
                          <a href='https://linkpride.duckdns.org/' 
                             style='background:#6f42c1; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none;'>
                            Verify My Email
                          </a>
                        </p>
                        <p>With love,<br>The TransLink Team 🌈</p>
                      </td>
                    </tr>
                    <tr>
                      <td style='background:#f2f2f2; color:#666; font-size:12px; text-align:center; padding:10px; border-bottom-left-radius:10px; border-bottom-right-radius:10px;'>
                        © 2025 TransLink. You’re receiving this email because you signed up for an account.
                      </td>
                    </tr>
                  </table>
                </body>
                </html>";


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

        public string GetEmailTemplate()
        {

        }
    }
}
