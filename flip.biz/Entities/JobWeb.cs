using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class JobWeb
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
    }
}