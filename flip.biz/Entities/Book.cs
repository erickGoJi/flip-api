using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Book
    {
        public Book()
        {
            BookLogs = new HashSet<BookLog>();
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public int? ActivityId { get; set; }
        public int? UserId { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BookLog> BookLogs { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}