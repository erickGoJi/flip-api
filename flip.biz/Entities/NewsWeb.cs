using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class NewsWeb
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
        public string Resume { get; set; }
        public string LongResumen { get; set; }
        public string Photo { get; set; }
    }
}