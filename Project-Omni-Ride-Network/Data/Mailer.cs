using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {
    public class Mailer {

        #region constructor

        private readonly IConfiguration _configuration;

        public Mailer(IConfiguration configuration) {
            this._configuration = configuration;
        }

        #endregion

        #region mailerMethod

        public async Task<bool> MailerAsync(string ourMail, string senderMail, string subject, string message) {
            try {
                using (var mail = new MailMessage()) {

                    mail.From = new MailAddress(ourMail);
                    mail.Subject = subject;
                    mail.To.Add(new MailAddress(senderMail));
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient(_configuration.GetValue<string>("MailCredentials:Hostname"), _configuration.GetValue<int>("MailCredentials:Port"))) {
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(_configuration.GetValue<string>("MailCredentials:Email"), _configuration.GetValue<string>("MailCredentials:Passwort"));
                        await smtpClient.SendMailAsync(mail);
                    }

                }

                return true;

            } catch (Exception ex) {
                return false;
            }

        }

        #endregion

    }
}
