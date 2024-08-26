using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Ammenitie
    {
        public Ammenitie()
        {
            BuildingIndexes = new HashSet<BuildingIndex>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BuildingId { get; set; }

        public virtual Building Building { get; set; }
        public virtual ICollection<BuildingIndex> BuildingIndexes { get; set; }
    }
}