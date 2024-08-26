using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class PerkCategory
    {
        public PerkCategory()
        {
            PerkGuides = new HashSet<PerkGuide>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PerkGuide> PerkGuides { get; set; }
    }
}