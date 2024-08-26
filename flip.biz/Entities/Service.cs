using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Service
    {
        public Service()
        {
            HistoricalServices = new HashSet<HistoricalService>();
            MembershipIndexes = new HashSet<MembershipIndex>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Provider { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<HistoricalService> HistoricalServices { get; set; }
        public virtual ICollection<MembershipIndex> MembershipIndexes { get; set; }
    }
}