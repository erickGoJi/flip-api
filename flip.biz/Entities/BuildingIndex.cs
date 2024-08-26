using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class BuildingIndex
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int PhotoBuildId { get; set; }
        public int AmmenitiesId { get; set; }
        public int RoomId { get; set; }

        public virtual Ammenitie Ammenities { get; set; }
        public virtual Building Building { get; set; }
        public virtual PhotoBuild PhotoBuild { get; set; }
        public virtual Room Room { get; set; }
    }
}