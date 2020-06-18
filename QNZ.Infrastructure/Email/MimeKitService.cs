using MailKit.Net.Smtp;
using MimeKit;

namespace QNZ.Infrastructure.Email
{
    public class MimeKitService : IEmailService
    {
      
        public void SendMail(string sender, string senderEmail, string mailTo, string mailcc, string subject, string body,
         string smtpServer, string fromEmail, string displayName, string userName, string password, int port, bool enableSsl)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(displayName, fromEmail));
            message.To.Add(new MailboxAddress("web1",mailTo));

            if (!string.IsNullOrEmpty(mailcc)) 
                message.Cc.Add(new MailboxAddress("web2",mailcc));

            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };
            message.ReplyTo.Add(new MailboxAddress(sender, senderEmail));


            using (var client = new SmtpClient())
            {

                client.Connect(smtpServer, port, enableSsl);
                //SMTP server authentication if needed
                client.Authenticate(userName, password);

                client.Send(message);

                client.Disconnect(true);
            }

        }
       
    }
}
