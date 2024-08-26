using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class PerkGuide
    {
        public PerkGuide()
        {
            GalleryPerks = new HashSet<GalleryPerk>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string StateProvincy { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? PackCategoryId { get; set; }
        public int? BuildingId { get; set; }

        public virtual Building Building { get; set; }
        public virtual PerkCategory PackCategory { get; set; }
        public virtual ICollection<GalleryPerk> GalleryPerks { get; set; }
    }
}