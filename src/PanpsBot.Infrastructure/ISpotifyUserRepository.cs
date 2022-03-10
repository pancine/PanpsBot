using PanpsBot.Models.Entities;

namespace PanpsBot.Infrastructure;

public interface ISpotifyUserRepository
{
    Task AddSpotifyUserAsync(SpotifyUser spotifyUser);
    Task<SpotifyUser> GetSpotifyUserAsync(string discordId);
    Task UpdateSpotifyUserAsync(SpotifyUser spotifyUser);
}
