namespace PanpsBot.Models.Entities;

public class SpotifyUser : Entity
{
    public string DiscordId { get; set; }
    public string Code { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string DateAdded { get; set; }
    public string ExpiresIn { get; set; }
}
