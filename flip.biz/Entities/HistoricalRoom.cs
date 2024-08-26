using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class HistoricalRoom
    {
        public HistoricalRoom()
        {
            HistoricalServices = new HashSet<HistoricalService>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomsId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool? LongTerm { get; set; }

        public virtual Room Rooms { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<HistoricalService> HistoricalServices { get; set; }
    }
}