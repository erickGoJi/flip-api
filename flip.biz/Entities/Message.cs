using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Message
    {
        public Message()
        {
            MessageReplies = new HashSet<MessageReply>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserIdReciver { get; set; }
        public DateTime Date { get; set; }
        public bool? Status { get; set; }

        public virtual User User { get; set; }
        public virtual User UserIdReciverNavigation { get; set; }
        public virtual ICollection<MessageReply> MessageReplies { get; set; }
    }
}