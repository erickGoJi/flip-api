using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class FileParameter
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string FileFolder { get; set; }
        public decimal? OneCol { get; set; }
    }
}