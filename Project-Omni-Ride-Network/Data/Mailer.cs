using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {
    public class Mailer {
        // class for mailer method
        #region constructor

        private readonly IConfiguration _configuration;

        public Mailer(IConfiguration configuration) {
            this._configuration = configuration;
        }

        #endregion

        #region mailerMethod

        /// <summary>
        /// Method that sends mail from a smtp server
        /// </summary>
        /// <param name="ourMail"></param>
        /// <param name="senderMail"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> MailerAsync(string ourMail, string senderMail, string subject, string message) {
            try {
                // create mail
                using (var mail = new MailMessage()) {

                    mail.From = new MailAddress(ourMail);
                    mail.Subject = subject;
                    mail.To.Add(new MailAddress(senderMail));
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    // send mail via smtp server
                    using (var smtpClient = new SmtpClient(_configuration.GetValue<string>("MailCredentials:Hostname"),
                            _configuration.GetValue<int>("MailCredentials:Port"))) {
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(_configuration.GetValue<string>("MailCredentials:Email"), 
                            _configuration.GetValue<string>("MailCredentials:Passwort"));
                        await smtpClient.SendMailAsync(mail);
                    }

                }

                return true;

            } catch (Exception) {
                return false;
            }

        }

        #endregion

    }
}
