using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Activity
    {
        public Activity()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int? QuoteMax { get; set; }
        public int? QuoteCurrent { get; set; }
        public int? AmenityId { get; set; }
        public int? Category { get; set; }

        public virtual Amenity Amenity { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}