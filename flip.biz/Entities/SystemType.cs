using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class SystemType
    {
        public SystemType()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}