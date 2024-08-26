using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Token
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateAcess { get; set; }
        public string Token1 { get; set; }
        public bool Active { get; set; }

        public virtual User User { get; set; }
    }
}