using System;
using System.Collections.Generic;

namespace flip.biz.Entities
{
    public partial class User
    {
        public User()
        {
            BookLogs = new HashSet<BookLog>();
            Books = new HashSet<Book>();
            Comments = new HashSet<Comment>();
            CreditCards = new HashSet<CreditCard>();
            HistoricalMemberships = new HashSet<HistoricalMembership>();
            HistoricalRooms = new HashSet<HistoricalRoom>();
            HistoricalServices = new HashSet<HistoricalService>();
            MessageReplies = new HashSet<MessageReply>();
            MessageUserIdReciverNavigations = new HashSet<Message>();
            MessageUsers = new HashSet<Message>();
            Posts = new HashSet<Post>();
            Tokens = new HashSet<Token>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string MotherName { get; set; }
        public string Avatar { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int BuildingId { get; set; }
        public int SystemTypeId { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string AboutMe { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }

        public virtual Building Building { get; set; }
        public virtual SystemType SystemType { get; set; }
        public virtual ICollection<BookLog> BookLogs { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CreditCard> CreditCards { get; set; }
        public virtual ICollection<HistoricalMembership> HistoricalMemberships { get; set; }
        public virtual ICollection<HistoricalRoom> HistoricalRooms { get; set; }
        public virtual ICollection<HistoricalService> HistoricalServices { get; set; }
        public virtual ICollection<MessageReply> MessageReplies { get; set; }
        public virtual ICollection<Message> MessageUserIdReciverNavigations { get; set; }
        public virtual ICollection<Message> MessageUsers { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}