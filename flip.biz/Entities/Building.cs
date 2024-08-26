using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Building
    {
        public Building()
        {
            Amenities = new HashSet<Amenity>();
            Ammenities = new HashSet<Ammenitie>();
            BuildingIndexes = new HashSet<BuildingIndex>();
            PerkGuides = new HashSet<PerkGuide>();
            Posts = new HashSet<Post>();
            Rooms = new HashSet<Room>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string Direction { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public virtual ICollection<Amenity> Amenities { get; set; }
        public virtual ICollection<Ammenitie> Ammenities { get; set; }
        public virtual ICollection<BuildingIndex> BuildingIndexes { get; set; }
        public virtual ICollection<PerkGuide> PerkGuides { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}