using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class HistoricalMembership
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MembershipId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public virtual Membership Membership { get; set; }
        public virtual User User { get; set; }
    }
}