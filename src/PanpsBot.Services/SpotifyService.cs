using Discord.Addons.Interactive;
using Discord.WebSocket;
using PanpsBot.Infrastructure;
using PanpsBot.Models.Entities;
using PanpsBot.Models.Exceptions;
using PanpsBot.Services.Builders;
using PanpsBot.Services.Clients;
using PanpsBot.Services.Extensions;

namespace PanpsBot.Services;

public class SpotifyService : ISpotifyService
{
    private readonly IAuthenticationStateRepository _authenticationStateRepository;
    private readonly IBitlyClient _bitlyClient;
    private readonly ISpotifyClient _spotifyClient;
    private readonly ISpotifyUserRepository _spotifyUserRepository;

    public SpotifyService(IAuthenticationStateRepository authenticationStateRepository,
        IBitlyClient bitlyClient, ISpotifyClient spotifyClient, ISpotifyUserRepository spotifyUserRepository)
    {
        _authenticationStateRepository = authenticationStateRepository;
        _bitlyClient = bitlyClient;
        _spotifyClient = spotifyClient;
        _spotifyUserRepository = spotifyUserRepository;
    }

    public async Task<string> GetAccessTokenUri(SocketUser user)
    {
        var stateKey = Guid.NewGuid().ToString();
        var url = _spotifyClient.GetOAuth2AuthorizeUrl(stateKey);

        var newUser = new SpotifyUser
        {
            DiscordId = user.Id.ToString(),
            DateAdded = DateTime.UtcNow.ConvertToSqlFormat()
        };

        await _authenticationStateRepository.SetSpotifyUserByStateKeyAsync(stateKey, newUser);

        return await _bitlyClient.ShortenUrl(user.Id.ToString(), url);
    }

    public async Task<PaginatedMessage> GetUserTopArtistsAsync(SocketUser user, string timeRange)
    {
        var spotifyUser = await GetSpotifyUserAsync(user.Id.ToString());

        var artists = await _spotifyClient.GetTopArtistsAsync(spotifyUser.AccessToken, timeRange);

        var paginatedMessage = new PaginatedMessageBuilder()
            .BuildWithUser(user.Username)
            .BuildSpotifyArtists(artists)
            .Build();

        return paginatedMessage;
    }

    public async Task<PaginatedMessage> GetUserTopTracksAsync(SocketUser user, string timeRange)
    {
        var spotifyUser = await GetSpotifyUserAsync(user.Id.ToString());

        var tracks = await _spotifyClient.GetTopTracksAsync(spotifyUser.AccessToken, timeRange);

        var paginatedMessage = new PaginatedMessageBuilder()
            .BuildWithUser(user.Username)
            .BuildSpotifyTracks(tracks)
            .Build();

        return paginatedMessage;
    }

    private async Task<SpotifyUser> GetSpotifyUserAsync(string discordId)
    {
        var spotifyUser = await _spotifyUserRepository.GetSpotifyUserAsync(discordId);

        if (spotifyUser == null) throw new SpotifyUserNotFoundException();

        if (spotifyUser.ExpiresIn.ConvertToSqlDateTime() <= DateTime.UtcNow.AddMinutes(-5))
        {
            spotifyUser = await _spotifyClient.RefreshUserAccessTokenAsync(spotifyUser);
            await _spotifyUserRepository.UpdateSpotifyUserAsync(spotifyUser);
        }

        return spotifyUser;
    }
}