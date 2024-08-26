using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class IndexWebPhoto
    {
        public int Id { get; set; }
        public string FrontPhoto { get; set; }
        public string BackPhoto { get; set; }
        public int Position { get; set; }
    }
}