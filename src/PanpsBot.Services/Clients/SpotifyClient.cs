using Microsoft.Extensions.Configuration;
using PanpsBot.Models.Entities;
using PanpsBot.Services.Extensions;
using SpotifyAPI.Web;
using static SpotifyAPI.Web.PersonalizationTopRequest;

namespace PanpsBot.Services.Clients;

public class SpotifyClient : ClientBase, ISpotifyClient
{
    private readonly IConfiguration _config;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly Uri _redirectUri;

    public SpotifyClient(IConfiguration configuration, HttpClient httpClient) : base(httpClient)
    {
        _config = configuration ??
            throw new ArgumentNullException(nameof(configuration));

        _clientId = _config["Spotify:ClientId"];
        _clientSecret = _config["Spotify:ClientSecret"];
        _redirectUri = new Uri(_config["Spotify:RedirectUri"]);
    }

    public string GetOAuth2AuthorizeUrl(string stateKey)
    {
        var baseUrl = "https://accounts.spotify.com/authorize";

        var parameters = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "response_type", "code"},
                { "redirect_uri", _redirectUri.ToString()},
                { "scope", "user-top-read" },
                { "state", stateKey }
            };

        return BuildUrlWithQueryParameters(baseUrl, parameters);
    }

    public async Task<SpotifyUser> GetUserSpotifyAsync(string code)
    {
        var oauth = new OAuthClient();
        var authorizationCodeTokenRequest = new AuthorizationCodeTokenRequest(_clientId, _clientSecret, code, _redirectUri);
        var tokenResponse = await oauth.RequestToken(authorizationCodeTokenRequest);

        return new SpotifyUser
        {
            Code = code,
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            DateAdded = DateTime.UtcNow.ConvertToSqlFormat(),
            ExpiresIn = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ConvertToSqlFormat()
        };
    }

    public async Task<Paging<FullArtist>> GetTopArtistsAsync(string accessToken, string timeRange)
    {
        var spotify = new SpotifyAPI.Web.SpotifyClient(accessToken);
        return await spotify.Personalization.GetTopArtists(GetRequestOptions(timeRange));
    }

    public async Task<Paging<FullTrack>> GetTopTracksAsync(string accessToken, string timeRange)
    {
        var spotify = new SpotifyAPI.Web.SpotifyClient(accessToken);
        return await spotify.Personalization.GetTopTracks(GetRequestOptions(timeRange));
    }

    public async Task<SpotifyUser> RefreshUserAccessTokenAsync(SpotifyUser spotifyUser)
    {
        var oauth = new OAuthClient();
        var tokenSwap = new AuthorizationCodeRefreshRequest(_clientId, _clientSecret, spotifyUser.RefreshToken);
        var tokenResponse = await oauth.RequestToken(tokenSwap);
        spotifyUser.AccessToken = tokenResponse.AccessToken;
        spotifyUser.ExpiresIn = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ConvertToSqlFormat();

        return spotifyUser;
    }

    private static PersonalizationTopRequest GetRequestOptions(string timeRange)
    {
        var timeRangeParam = GetTimeRange(timeRange);
        var requestOptions = new PersonalizationTopRequest
        {
            TimeRangeParam = timeRangeParam,
            Limit = 50,
        };
        return requestOptions;
    }

    private static TimeRange GetTimeRange(string timeRange)
    {
        return timeRange switch
        {
            "short" => TimeRange.ShortTerm,
            "medium" => TimeRange.MediumTerm,
            _ => TimeRange.LongTerm
        };
    }
}
