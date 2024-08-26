using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class GalleryPerk
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public int? PerkGuideId { get; set; }

        public virtual PerkGuide PerkGuide { get; set; }
    }
}