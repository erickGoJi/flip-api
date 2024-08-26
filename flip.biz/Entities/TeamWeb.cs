using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class TeamWeb
    {
        public int Id { get; set; }
        public string FrontPhoto { get; set; }
        public string BackPhoto { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string LinkedinUrl { get; set; }
        public string TwitterUrl { get; set; }
    }
}