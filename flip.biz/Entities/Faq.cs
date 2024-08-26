using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Faq
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}