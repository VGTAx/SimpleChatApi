using Microsoft.EntityFrameworkCore;
using SimpleChatApi.Models;

namespace SimpleChatApi.Data
{
  public class ChatContext : DbContext
  {
    public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

    public DbSet<Chat> Chats { get; set; }  
    public DbSet<User> Users { get; set; }
    public DbSet<UserChat> UserChats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder
        .Entity<UserChat>()
        .HasKey(uc => new { uc.UserId, uc.ChatId });

      modelBuilder
        .Entity<Chat>()
        .HasMany(c => c.UserChats)
        .WithOne(uc => uc.Chat)
        .HasForeignKey(uc => uc.ChatId);

      modelBuilder
        .Entity<User>()
        .HasMany(u => u.UserChats)
        .WithOne(uc => uc.User)
        .HasForeignKey(uc => uc.UserId);

      modelBuilder
        .Entity<User>()
        .HasData(
          new User { Id = 1, Name = "John"},
          new User { Id = 2, Name = "Elon"},
          new User { Id = 3, Name = "Kate"},
          new User { Id = 4, Name = "Dan"},
          new User { Id = 5, Name = "Chuck"},
          new User { Id = 6, Name = "Julia"},
          new User { Id = 7, Name = "Juliet" },
          new User { Id = 8, Name = "Bart"},
          new User { Id = 9, Name = "Blair" },
          new User { Id = 10, Name = "Nat"}
        );

      modelBuilder
      .Entity<Chat>()
      .HasData(
        new Chat { Id = 1, Name = "NewChat_1", CreatorChatId = 1 },
        new Chat { Id = 2, Name = "NewChat_2", CreatorChatId = 2 },
        new Chat { Id = 3, Name = "NewChat_3", CreatorChatId = 3 }
      );
    }
  }
}
