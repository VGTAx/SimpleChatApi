using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SimpleChatApi.Data;
using SimpleChatApi.DTOs;
using SimpleChatApi.Hubs;
using SimpleChatApi.Models;

namespace SimpleChatApi.Controllers
{
  /// <summary>
  /// Controller for managing chats.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class ChatController : ControllerBase
  {
    private readonly ChatContext _context;
    private readonly IHubContext<ChatHub> _hubContext;
    /// <summary>
    /// ChatController constructor
    /// </summary>
    /// <param name="context">DB context</param>
    /// <param name="hubContext">SignalR Hub context</param>
    public ChatController(ChatContext context, IHubContext<ChatHub> hubContext)
    {
      _context = context;
      _hubContext = hubContext;
    }
    /// <summary>
    /// Create chat
    /// </summary>
    /// <param name="createChatDto" example='{"userId":"1", "chatName":"NewChat"}'>DTO for create chat</param>
    /// <response code="200">Chat was created</response> 
    /// <response code="400">Chat has already exist</response>
    /// <response code="404">User was not found.</response>
    /// <response code="500">Internal server error</response>
    /// <returns> Returns an HTTP status code indicating the result of the method.</returns>
    [HttpPost(template: nameof(CreateChat), Name = nameof(CreateChat))]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto)
    {
      try
      {
        if (!IsUserExist(createChatDto.UserId))
        {
          return NotFound("User was not found");
        }

        if (IsChatNameUnique(createChatDto.ChatName!))
        {
          return BadRequest($"Chat with name \"{createChatDto.ChatName!}\" has already exist");
        }

        var chat = new Chat { Name = createChatDto.ChatName!, CreatorChatId = createChatDto.UserId };        
        await _context.Chats.AddAsync(chat);
        await _context.SaveChangesAsync(); 

        return Ok("Chat has created");
      }
      catch (Exception)
      {
        return StatusCode(500, "Internal server error");
      }      
    }

    /// <summary>
    /// Delete chat
    /// </summary>
    /// <param name="deleteChatDto" example='{"userId":1, "chatId":1}'>DTO for delete chat</param>
    /// <response code="204">Chat was deleted</response>
    /// <response code="404">Chat was not found.</response>
    /// <response code="500">Internal server error</response>
    /// <returns>Returns an HTTP status code indicating the result of the method.</returns>
    [HttpPost(template: nameof(DeleteChat), Name = nameof(DeleteChat))]
    public async Task<IActionResult> DeleteChat([FromBody] DeleteChatDto deleteChatDto)
    {
      try
      {
        var chat = _context.Chats
          .FirstOrDefault(c => c.Id == deleteChatDto.ChatId);

        if (chat is null)
        {
          return NotFound($"Chat with ID: {deleteChatDto.ChatId} was not found");
        }
        // checks user permissions to delete chat
        if (chat.CreatorChatId != deleteChatDto.UserId)
        {
          return StatusCode(403, "There are no permissions to do the operation");
        }
        // remove chat from DB
        _context.Chats.Remove(chat);

        // get users to remove from SignalR hub group
        var usersChat =
          _context.UserChats
            .AsNoTracking()
            .Where(uc => uc.ChatId == chat.Id)
            .Select(u => u.User)
            .ToList();
        // remove user from group
        foreach (var user in usersChat)
        {
          if (user!.ConnectionId != null) 
          {
            await _hubContext.Groups.RemoveFromGroupAsync(user.ConnectionId!, chat.Name!);
            await _hubContext.Clients.Group(chat.Name!).SendAsync("ReceiveMessage", user.Name, " left chat");
          }          
        }
        // get related entities (users and chat)
        var userChatsToRemove =
          _context.UserChats
            .Where(uc => uc.Chat!.Id == chat.Id)
            .ToList();
        // remove related entites
        _context.UserChats.RemoveRange(userChatsToRemove);
        await _context.SaveChangesAsync();

        return NoContent();
      }
      catch (Exception)
      {
        return StatusCode(500, "Internal Server Error");
      }

    }

    /// <summary>
    /// Get chat by name
    /// </summary>
    /// <param name="chatName">Chat name</param>
    /// <response code="200">Chat was got</response>
    /// <response code="404">Chat was not found.</response>
    /// <response code="500">Internal server error</response>
    /// <returns>Returns an HTTP status code indicating the result of the get user method.</returns>
    [HttpGet($"{nameof(GetChat)}" + "/{chatName}", Name = nameof(GetChat))]
    public async Task<IActionResult> GetChat(string chatName)
    {
      try
      {
        var chat = await _context.Chats
          .FirstOrDefaultAsync(c => c.Name!.Contains(chatName));

        if (chat == null)
        {
          return NotFound("Chat was not found");
        }
        return Ok(chat);
      }
      catch (Exception)
      {
        return StatusCode(500, "Internal Server Error");
      }      
    }
    /// <summary>
    /// Method checks the existence of a user.
    /// </summary>
    /// <param name="userId">User id for check</param>
    /// <returns> Result checking the existence of a user. True if user exists, else false</returns>
    private bool IsUserExist(int userId)
    {
      return _context.Users.Any(u => u.Id == userId);
    }

    /// <summary>
    /// Method checks the uniqueness of the chat name
    /// </summary>
    /// <param name="chatName">Chat name for check</param>
    /// <returns>Result checking the uniqueness chat name. True if email is unique, else false</returns>
    private bool IsChatNameUnique(string chatName)
    {
      return _context.Chats.Any(u => u.Name == chatName);
    }    
  }
}
