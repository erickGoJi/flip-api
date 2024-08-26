using flip.api.Models.Email;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace flip.api.Services
{
    public class EmailRepository : IEmailRepository
    {
        private EmailSettings _emailSettings { get; set; }

        public EmailRepository(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        //Servicio de envio de correos
        public void SendEmail(Email email)
        {
            try
            {
                //new SmtpClient
                //{
                //    Host = "Smtp.Gmail.com",
                //    Port = 587,
                //    EnableSsl = true,
                //    Timeout = 10000,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential("rodrigo.stps@gmail.com", "bostones01")
                //}.Send(new MailMessage
                //{
                //    From = new MailAddress("no-reply@techo.org", "Miele"),
                //    To = { email.To },
                //    Subject = email.Subject,
                //    IsBodyHtml = email.IsBodyHtml,
                //    Body = email.Body
                //});

                new SmtpClient
                {
                    Host = _emailSettings.PrimaryDomain,
                    Port = _emailSettings.PrimaryPort,
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword)
                }.Send(new MailMessage
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "Flip!"),
                    To = { email.To },
                    Subject = email.Subject,
                    IsBodyHtml = email.IsBodyHtml,
                    Body = email.Body
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Servicio asincrono para envio de correos 
        public async Task<string> SendMailAsync(Email email)
        {
            string res = "";
            try
            {
                res = await Send(email.To, email.Subject, email.IsBodyHtml, email.Body);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        //Servicio asincrono para envio de correos con archivos adjuntos
        public async Task SendAttachment(string email, string subject, bool isBodyHtml, string message, List<Attachment> attachments)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_emailSettings.UsernameEmail, _emailSettings.MailName);
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;
                mail.IsBodyHtml = isBodyHtml;
                mail.Body = message;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Timeout = 10000;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = _emailSettings.EnableSsl;
                    foreach (var item in attachments)
                    {
                        mail.Attachments.Add(item);
                    }
                    await smtp.SendMailAsync(mail);

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<string> Send(string email, string subject, bool isBodyHtml, string message)
        {

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_emailSettings.UsernameEmail, _emailSettings.MailName);
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;
                mail.IsBodyHtml = isBodyHtml;
                mail.Body = message;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Timeout = 10000;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = _emailSettings.EnableSsl;
                    await smtp.SendMailAsync(mail);
                }
                return "Enviado";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }




    }
}
