using PanpsBot.Models.Entities;
using SpotifyAPI.Web;

namespace PanpsBot.Services.Clients;

public interface ISpotifyClient
{
    string GetOAuth2AuthorizeUrl(string stateKey);
    Task<SpotifyUser> GetUserSpotifyAsync(string code);
    Task<Paging<FullArtist>> GetTopArtistsAsync(string accessToken, string timeRange);
    Task<Paging<FullTrack>> GetTopTracksAsync(string accessToken, string timeRange);
    Task<SpotifyUser> RefreshUserAccessTokenAsync(SpotifyUser spotifyUser);
}
