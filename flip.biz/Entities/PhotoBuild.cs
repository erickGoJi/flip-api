using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class PhotoBuild
    {
        public PhotoBuild()
        {
            BuildingIndexes = new HashSet<BuildingIndex>();
        }

        public int Id { get; set; }
        public string PhotoUrl { get; set; }

        public virtual ICollection<BuildingIndex> BuildingIndexes { get; set; }
    }
}