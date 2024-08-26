using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Room
    {
        public Room()
        {
            BuildingIndexes = new HashSet<BuildingIndex>();
            HistoricalRooms = new HashSet<HistoricalRoom>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? BuildingId { get; set; }

        public virtual Building Building { get; set; }
        public virtual ICollection<BuildingIndex> BuildingIndexes { get; set; }
        public virtual ICollection<HistoricalRoom> HistoricalRooms { get; set; }
    }
}