using PanpsBot.Models.Entities;

namespace PanpsBot.Infrastructure;

public interface IAuthenticationStateRepository
{
    Task ClearCacheAsync();
    Task<string> GetSpotifyUserIdByStateKeyAsync(string key);
    Task SetSpotifyUserByStateKeyAsync(string key, SpotifyUser user);
}
