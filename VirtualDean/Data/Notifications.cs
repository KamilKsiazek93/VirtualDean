using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class Notifications : INotifications
    {
        private readonly string _smtpServer;
        private readonly string _email;
        private readonly string _password;
        public Notifications(IConfiguration configuration)
        {
            _smtpServer = configuration["SmtpServer"];
            _email = configuration["Email"];
            _password = configuration["Password"];
        }

        public void SendEmail(string [] reciepients, string mailContent, string subject)
        {
            EmailSendConfigure myConfig = new EmailSendConfigure();
            myConfig.ClientCredentialUserName = _email;
            myConfig.ClientCredentialPassword = _password;
            myConfig.TOs = reciepients;
            myConfig.From = _email;
            myConfig.FromDisplayName = "DziekanInfo";
            myConfig.Priority = MailPriority.Normal;
            myConfig.Subject = subject;

            EmailContent myContent = new EmailContent();
            myContent.Content = mailContent;

            SendMail(myConfig, myContent);
        }

        public void SendMail(EmailSendConfigure emailConfig, EmailContent content)
        {
            MailMessage msg = ConstructEmailMessage(emailConfig, content);
            Send(msg, emailConfig);
        }

        private MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)
        {
            MailMessage msg = new MailMessage();
            foreach (string to in emailConfig.TOs)
            {
                if (!string.IsNullOrEmpty(to))
                {
                    msg.To.Add(to);
                }
            }

            msg.From = new MailAddress(emailConfig.From, emailConfig.FromDisplayName, System.Text.Encoding.UTF8);
            msg.IsBodyHtml = content.IsHtml;
            msg.Body = content.Content;
            msg.Priority = emailConfig.Priority;
            msg.Subject = emailConfig.Subject;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            return msg;
        }

        private void Send(MailMessage message, EmailSendConfigure emailConfig)
        {
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(emailConfig.ClientCredentialUserName, emailConfig.ClientCredentialPassword);
            client.Host = _smtpServer;
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Send email: {0}", e.Message);
                throw;
            }
            message.Dispose();
        }
    }
}
