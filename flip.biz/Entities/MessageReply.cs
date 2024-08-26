using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class MessageReply
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
        public bool? Status { get; set; }
        public int MessageId { get; set; }

        public virtual Message MessageNavigation { get; set; }
        public virtual User User { get; set; }
    }
}