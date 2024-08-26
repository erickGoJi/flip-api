using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class HistoricalService
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServicesId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int? HistoricalRoomId { get; set; }

        public virtual HistoricalRoom HistoricalRoom { get; set; }
        public virtual Service Services { get; set; }
        public virtual User User { get; set; }
    }
}