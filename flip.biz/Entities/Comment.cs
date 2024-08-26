using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Comment1 { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}