using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class MembershipIndex
    {
        public int Id { get; set; }
        public int MembershipId { get; set; }
        public int ServiceId { get; set; }

        public virtual Membership Membership { get; set; }
        public virtual Service Service { get; set; }
    }
}