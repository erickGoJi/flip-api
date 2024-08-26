using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string PostText { get; set; }
        public string Photo { get; set; }
        public string Title { get; set; }
        public int BuildingId { get; set; }

        public virtual Building Building { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}