using ChatService.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Data
{
    public   class ChatDbContext:DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) :base(options){ }

        //public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    OnModelCreatingPartial(modelBuilder);
        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
