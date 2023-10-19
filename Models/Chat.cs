using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimpleChatApi.Models
{
  /// <summary>
  /// Represent a chat
  /// </summary>
  public class Chat
  {
    /// <summary>
    /// Chat ID
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    /// <summary>
    /// Chat name
    /// </summary>
    [Required]
    public string? Name { get; set; }
    /// <summary>
    /// Creator chat id
    /// </summary>
    [Required]
    public int CreatorChatId { get; set; }
    /// <summary>
    /// Navigation property
    /// </summary>
    [JsonIgnore]
    public List<UserChat>? UserChats { get; set; }

  }
}
