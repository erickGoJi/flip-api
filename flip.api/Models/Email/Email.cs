﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flip.api.Models.Email
{
    public class Email
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
    }

    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }
        public string MailName { get; set; }
        public int PrimaryPort { get; set; }
        public string UsernameEmail { get; set; }
        public string UsernamePassword { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public bool EnableSsl { get; set; }
    }
}
