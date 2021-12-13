using Microsoft.Extensions.Options;
using net_core_backend.Context;
using net_core_backend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public interface IMailingService
    {
        void SendAccountCreationEmail(string toEmail);
        void SendForgottenPasswordLink(string token, string toEmail);
        void SendLicenseAbuseEmail(string licenseId, string licenseEmail);
    }

    public class MailingService : IMailingService
    {
        private readonly AppSettings appSettings;
        private readonly string[] HardcodedEmailAdmins = new string[1]
        {
            "thomasvandermolen2@gmail.com",
        };

        public MailingService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public void SendLicenseAbuseEmail(string licenseId, string licenseEmail)
        {
            var subject = "OneBlinq License Abuse";
            var content = $"One of your customers with email: {licenseEmail} recently overused one of his license keys with id: {licenseId}." +
                $"\nTo take action please visit: {appSettings.ProductionFrontendUrl}";

            foreach(var adminemail in HardcodedEmailAdmins)
            {
                SendBasicEmail(subject, content, adminemail);
            }
        }

        public void SendForgottenPasswordLink(string token, string toEmail)
        {
            var subject = "OneBlinq password recovery.";
            var content = $"Did you issue a forgotten password request for OneBlinq? Go to this URL and enter your new password:\n" +
                $"{appSettings.ProductionFrontendUrl}recovery?token={token}\n" +
                $"The link will expire in 10 minutes";
            
            SendBasicEmail(subject, content, toEmail);
        }

        public void SendAccountCreationEmail(string toEmail)
        {
            var subject = "OneBlinq license account created.";
            var content = $"You recently bought a license for one of OneBlinqs plugins. " +
                $"We have created an account for you on our website so you can monitor your usage.\n" +
                $"Visit: {appSettings.ProductionFrontendUrl}/login?email={toEmail} and register your password.";
            
            SendBasicEmail(subject, content, toEmail);
        }

        private void SendBasicEmail(string subjectContent, 
            string bodyContent,
            string toEmail,
            bool isBodyHtml = false, 
            string smtpRelay = "smtp.gmail.com", 
            int smtpPort = 587)
        {
            try
            {
                using MailMessage mm = new MailMessage(appSettings.NoReplyEmail, toEmail);
                mm.Subject = subjectContent;
                mm.Body = bodyContent;
                mm.IsBodyHtml = isBodyHtml;

                using SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpRelay;
                smtp.EnableSsl = true;

                NetworkCredential NetworkCred = new NetworkCredential(appSettings.NoReplyEmail, appSettings.NoReplyEmailPassword);
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = NetworkCred;
                smtp.Port = smtpPort;
                smtp.Send(mm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Unable to send email to: {toEmail}.\n--> Error msg: {ex.Message}");
            }
        }
    }
}
