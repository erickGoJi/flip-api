using System;
using flip.biz.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace flip.dal.DB_context
{
    public partial class Db_FlipContext : DbContext
    {
        public Db_FlipContext()
        {
        }

        public Db_FlipContext(DbContextOptions<Db_FlipContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Amenity> Amenities { get; set; }
        public virtual DbSet<Ammenitie> Ammenities { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookLog> BookLogs { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<BuildingIndex> BuildingIndexes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CreditCard> CreditCards { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<FileParameter> FileParameters { get; set; }
        public virtual DbSet<GalleryPerk> GalleryPerks { get; set; }
        public virtual DbSet<HistoricalMembership> HistoricalMemberships { get; set; }
        public virtual DbSet<HistoricalRoom> HistoricalRooms { get; set; }
        public virtual DbSet<HistoricalService> HistoricalServices { get; set; }
        public virtual DbSet<IndexWebPhoto> IndexWebPhotos { get; set; }
        public virtual DbSet<JobWeb> JobWebs { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<MembershipIndex> MembershipIndexes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageReply> MessageReplies { get; set; }
        public virtual DbSet<NewsWeb> NewsWebs { get; set; }
        public virtual DbSet<PerkCategory> PerkCategories { get; set; }
        public virtual DbSet<PerkGuide> PerkGuides { get; set; }
        public virtual DbSet<PhotoBuild> PhotoBuilds { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<SystemType> SystemTypes { get; set; }
        public virtual DbSet<TeamWeb> TeamWebs { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activity");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Photo).IsUnicode(false);

                entity.HasOne(d => d.Amenity)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.AmenityId)
                    .HasConstraintName("FK_Activity_Amenity");
            });

            modelBuilder.Entity<Amenity>(entity =>
            {
                entity.ToTable("Amenity");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Photo).IsUnicode(false);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Amenities)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Amenity_Building");
            });

            modelBuilder.Entity<Ammenitie>(entity =>
            {
                entity.ToTable("Ammenitie");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Ammenities)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_Ammenitie_Building");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("FK_Books_Activity");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Book_User");
            });

            modelBuilder.Entity<BookLog>(entity =>
            {
                entity.ToTable("BookLog");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookLogs)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_BookLog_Book");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BookLog_User");
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Building");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Direction).HasMaxLength(100);

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<BuildingIndex>(entity =>
            {
                entity.ToTable("BuildingIndex");

                entity.HasOne(d => d.Ammenities)
                    .WithMany(p => p.BuildingIndexes)
                    .HasForeignKey(d => d.AmmenitiesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuildingIndex_Ammenities");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.BuildingIndexes)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuildingIndex_Building");

                entity.HasOne(d => d.PhotoBuild)
                    .WithMany(p => p.BuildingIndexes)
                    .HasForeignKey(d => d.PhotoBuildId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuildingIndex_PhotoBuild");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.BuildingIndexes)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuildingIndex_Room");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Comment1)
                    .IsRequired()
                    .HasColumnName("Comment")
                    .HasMaxLength(500);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<CreditCard>(entity =>
            {
                entity.ToTable("CreditCard");

                entity.Property(e => e.Ccv)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Month)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Year)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CreditCards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreditCard_User");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("Faq");

                entity.Property(e => e.Answer).IsRequired();

                entity.Property(e => e.Question).IsRequired();
            });

            modelBuilder.Entity<FileParameter>(entity =>
            {
                entity.ToTable("FileParameter");

                entity.Property(e => e.FileFolder)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OneCol).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<GalleryPerk>(entity =>
            {
                entity.ToTable("GalleryPerk");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Photo).IsRequired();

                entity.HasOne(d => d.PerkGuide)
                    .WithMany(p => p.GalleryPerks)
                    .HasForeignKey(d => d.PerkGuideId)
                    .HasConstraintName("FK_GalleryPerk_PerkGuide");
            });

            modelBuilder.Entity<HistoricalMembership>(entity =>
            {
                entity.ToTable("HistoricalMembership");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.HistoricalMemberships)
                    .HasForeignKey(d => d.MembershipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricalMembership_Membership");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HistoricalMemberships)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricalMembership_User");
            });

            modelBuilder.Entity<HistoricalRoom>(entity =>
            {
                entity.ToTable("HistoricalRoom");

                entity.HasOne(d => d.Rooms)
                    .WithMany(p => p.HistoricalRooms)
                    .HasForeignKey(d => d.RoomsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricalRoom_Room");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HistoricalRooms)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricalRoom_UserId");
            });

            modelBuilder.Entity<HistoricalService>(entity =>
            {
                entity.ToTable("HistoricalService");

                entity.HasOne(d => d.HistoricalRoom)
                    .WithMany(p => p.HistoricalServices)
                    .HasForeignKey(d => d.HistoricalRoomId)
                    .HasConstraintName("FK_HistoricalService_HistoricalRoom");

                entity.HasOne(d => d.Services)
                    .WithMany(p => p.HistoricalServices)
                    .HasForeignKey(d => d.ServicesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricalService_Service");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HistoricalServices)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricalService_UserId");
            });

            modelBuilder.Entity<IndexWebPhoto>(entity =>
            {
                entity.Property(e => e.BackPhoto)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FrontPhoto)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<JobWeb>(entity =>
            {
                entity.ToTable("JobWeb");

                entity.Property(e => e.LongDescription)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.ToTable("Membership");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<MembershipIndex>(entity =>
            {
                entity.ToTable("MembershipIndex");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.MembershipIndexes)
                    .HasForeignKey(d => d.MembershipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MembershipIndex_Membership");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.MembershipIndexes)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MembershipIndex_Service");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MessageUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_UserId_Sender");

                entity.HasOne(d => d.UserIdReciverNavigation)
                    .WithMany(p => p.MessageUserIdReciverNavigations)
                    .HasForeignKey(d => d.UserIdReciver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_Reciver");
            });

            modelBuilder.Entity<MessageReply>(entity =>
            {
                entity.ToTable("MessageReply");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.MessageNavigation)
                    .WithMany(p => p.MessageReplies)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MessageReply_Message");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MessageReplies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MessageReply_UserId");
            });

            modelBuilder.Entity<NewsWeb>(entity =>
            {
                entity.ToTable("NewsWeb");

                entity.Property(e => e.LongResumen)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Photo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Resume)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Tittle)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PerkCategory>(entity =>
            {
                entity.ToTable("PerkCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PerkGuide>(entity =>
            {
                entity.ToTable("PerkGuide");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Latitude)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StateProvincy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StreetAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.PerkGuides)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_PerkGuide_Building");

                entity.HasOne(d => d.PackCategory)
                    .WithMany(p => p.PerkGuides)
                    .HasForeignKey(d => d.PackCategoryId)
                    .HasConstraintName("FK_PerkGuide_PerkCategory");
            });

            modelBuilder.Entity<PhotoBuild>(entity =>
            {
                entity.ToTable("PhotoBuild");

                entity.Property(e => e.PhotoUrl)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Photo).HasMaxLength(100);

                entity.Property(e => e.PostText)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Building");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_User");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_Room_BuildingId");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.NoonMidnight).IsUnicode(false);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_Schedule_Book");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Provider).HasMaxLength(50);
            });

            modelBuilder.Entity<SystemType>(entity =>
            {
                entity.ToTable("SystemType");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TeamWeb>(entity =>
            {
                entity.ToTable("TeamWeb");

                entity.Property(e => e.BackPhoto)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FrontPhoto)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LinkedinUrl).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.TwitterUrl).HasMaxLength(100);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Token");

                entity.Property(e => e.Token1)
                    .IsRequired()
                    .HasColumnName("Token")
                    .HasMaxLength(500);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Token_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.AboutMe).HasMaxLength(100);

                entity.Property(e => e.Avatar).HasMaxLength(100);

                entity.Property(e => e.CellPhone).HasMaxLength(20);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FacebookUrl).HasMaxLength(50);

                entity.Property(e => e.InstagramUrl).HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MotherName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.TwitterUrl).HasMaxLength(50);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Building");

                entity.HasOne(d => d.SystemType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.SystemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_SystemType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}