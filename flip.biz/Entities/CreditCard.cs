using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class CreditCard
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Ccv { get; set; }

        public virtual User User { get; set; }
    }
}