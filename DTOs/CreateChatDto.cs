using System.ComponentModel.DataAnnotations;

namespace SimpleChatApi.DTOs
{
  /// <summary>
  /// DTO for create chat
  /// </summary>
  public class CreateChatDto
  {
    /// <summary>
    /// User ID
    /// </summary>
    /// <example>1</example>
    [Required]
    public int UserId { get; set; }
    /// <summary>
    /// Chat name
    /// </summary>
    /// <example>NewChat</example>
    [Required]
    public string? ChatName { get; set; }    
  }
}
