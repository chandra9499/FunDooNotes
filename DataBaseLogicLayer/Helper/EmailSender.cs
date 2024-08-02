
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using MimeKit.Text;

namespace DataBaseLayer.Helper
{
    public class EmailSender
    {
        public static async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            var fromEmail = "chandrashekar.6569@gmail.com";
            var fromPassword = "Chandra@7022";

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Chandrashekar", fromEmail)); // Use your name or your company's name
            mimeMessage.To.Add(new MailboxAddress("", email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart(TextFormat.Html) { Text = message };

            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtpClient.AuthenticateAsync(fromEmail, fromPassword);
                    await smtpClient.SendAsync(mimeMessage);
                    await smtpClient.DisconnectAsync(true);
                    return true;
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    return false;
                }
            }
        }
    }
}