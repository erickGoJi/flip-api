using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? TimeStart { get; set; }
        public int? TimeEnd { get; set; }
        public string NoonMidnight { get; set; }
        public int? BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}