using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimpleChatApi.Models
{
  /// <summary>
  /// Represent a user
  /// </summary>
  public class User
  {
    /// <summary>
    /// Used ID
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>
    /// Username
    /// </summary>
    [Required]
    public string? Name { get; set; }
    /// <summary>
    /// ConnectionID in SignalR Hub
    /// </summary>
    public string? ConnectionId { get; set; }
    /// <summary>
    /// Navigation property
    /// </summary>
    [JsonIgnore]
    public List<UserChat>? UserChats { get; set; } = new List<UserChat> { };
  }
}
