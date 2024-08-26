using flip.biz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flip.api.Models.Building
{
    public partial class BuildingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string Direction { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Photo { get; set; }

        public List<TypeRoom> TypeRoom { get; set; }
    }
}
