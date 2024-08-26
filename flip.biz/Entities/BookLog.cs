using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class BookLog
    {
        public int Id { get; set; }
        public int? BookId { get; set; }
        public int? UserId { get; set; }

        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}