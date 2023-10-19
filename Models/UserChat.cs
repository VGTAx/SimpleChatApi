namespace SimpleChatApi.Models
{
  /// <summary>
  /// 
  /// </summary>
  public class UserChat
  {
    /// <summary>
    /// User ID
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// Chat ID
    /// </summary>
    public int ChatId { get; set; }
    /// <summary>
    /// Navigation property
    /// </summary>
    public User? User { get; set; }
    /// <summary>
    /// Navigation property
    /// </summary>
    public Chat? Chat { get; set; }  
  }
}
