using Discord.Addons.Interactive;
using Discord.WebSocket;

namespace PanpsBot.Services;

public interface ISpotifyService
{
    Task<string> GetAccessTokenUri(SocketUser user);
    Task<PaginatedMessage> GetUserTopArtistsAsync(SocketUser user, string timeRange);
    Task<PaginatedMessage> GetUserTopTracksAsync(SocketUser user, string timeRange);
}
