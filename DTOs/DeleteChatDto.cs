using System.ComponentModel.DataAnnotations;

namespace SimpleChatApi.DTOs
{
  /// <summary>
  /// DTO for delete chat
  /// </summary>
  public class DeleteChatDto
  {
    /// <summary>
    /// User ID
    /// </summary>
    /// <example>1</example>
    [Required]
    public int UserId { get; set; }
    /// <summary>
    /// Chat ID
    /// </summary>
    /// <example>1</example>
    [Required]
    public int ChatId { get; set; }
  }
}
