using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using flip.api.Models.Email;

namespace flip.api.Services
{
    public interface IEmailRepository
    {
        void SendEmail(Email email);
        Task<string> SendMailAsync(Email email);
        Task SendAttachment(string email, string subject, bool isBodyHtml, string message, List<Attachment> attachments);
    }
}
