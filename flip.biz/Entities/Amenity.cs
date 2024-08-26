using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Amenity
    {
        public Amenity()
        {
            Activities = new HashSet<Activity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BuildingId { get; set; }
        public string Photo { get; set; }

        public virtual Building Building { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}