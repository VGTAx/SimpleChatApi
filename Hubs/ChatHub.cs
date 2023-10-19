using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRSwaggerGen.Attributes;
using SimpleChatApi.Data;
using SimpleChatApi.Models;

namespace SimpleChatApi.Hubs
{
  /// <summary>
  /// SignalR Hub
  /// </summary>
  [SignalRHub]
  public class ChatHub : Hub
  {
    private readonly ChatContext _context;

    /// <summary>
    /// Chathub constructor
    /// </summary>
    /// <param name="context">Context DB</param>
    public ChatHub(ChatContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Send message in chat
    /// </summary>
    /// <param name="username" example="John">Username</param>
    /// <param name="message" example="Hello World!">Message</param>
    /// <param name="nameChat" example="NewChat_1">Name chat</param>
    /// <returns></returns>
    public async Task Send(string username, string message, string nameChat)
    {
      await Clients.Groups(nameChat).SendAsync("ReceiveMessage", username, message);
    }

    /// <summary>
    /// Method overriding
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    [SignalRHidden]
    public override Task OnDisconnectedAsync(Exception? exception)
    {
      var user = _context.Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);

      if (user is not null)
      {
        // Remove connectionId and related entities (user and chat) from the database
        user.ConnectionId = string.Empty;
        _context.Update(user);

        var chatId = _context.UserChats
          .AsNoTracking()
          .FirstOrDefault(uc => uc.UserId == user.Id)!
          .ChatId;

        _context.UserChats.Remove(new UserChat { ChatId = chatId, UserId = user.Id });
        _context.SaveChangesAsync();
      }

      return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Join chat
    /// </summary>
    /// <param name="chatName" example="NewChat_1">Chat name</param>
    /// <param name="userId" example="1">User ID</param>
    /// <returns></returns>
    [SignalRMethod(name: nameof(Join))]
    public async Task Join(string chatName, int userId)
    {
      var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Name == chatName);
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

      if (chat != null && user != null)
      {
        await Groups.AddToGroupAsync(Context.ConnectionId, chat.Name!);
        await Clients.Group(chat.Name!).SendAsync("ReceiveMessage", user.Name, $" join chat");
        user.ConnectionId = Context.ConnectionId;

        _context.Users.Update(user);
        await _context.UserChats.AddAsync(new UserChat { ChatId = chat.Id, UserId = user.Id });
        await _context.SaveChangesAsync();
      }
      else
      {
        await Clients.Client(Context.ConnectionId)
          .SendAsync("ReceiveMessage", "Server", $"Chat {chatName} isn't existed");
      }
    }

    /// <summary>
    /// Left chat
    /// </summary>
    /// <param name="chatName" example="NewChat_1">Chat name</param>
    /// <param name="userId" example="1">User ID</param>
    /// <returns></returns>
    [SignalRMethod(name: nameof(Left))]
    public async Task Left(string chatName, int userId)
    {
      var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Name == chatName);
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

      if (chat != null && user != null)
      {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.Name!);
        await Clients.Group(chat.Name!).SendAsync("ReceiveMessage", user.Name, $" left chat");

        _context.UserChats.Remove(new UserChat { ChatId = chat.Id, UserId = user.Id });
        await _context.SaveChangesAsync();
      }
      else
      {
        await Clients.Client(Context.ConnectionId)
          .SendAsync("ReceiveMessage", "Server", $"Chat {chatName} isn't existed");
      }
    }
  }
}
