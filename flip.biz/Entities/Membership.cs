using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Membership
    {
        public Membership()
        {
            HistoricalMemberships = new HashSet<HistoricalMembership>();
            MembershipIndexes = new HashSet<MembershipIndex>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<HistoricalMembership> HistoricalMemberships { get; set; }
        public virtual ICollection<MembershipIndex> MembershipIndexes { get; set; }
    }
}