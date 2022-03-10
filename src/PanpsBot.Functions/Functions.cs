using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using PanpsBot.Infrastructure;
using PanpsBot.Services.Clients;
using Microsoft.AspNetCore.Mvc;

namespace PanpsBot.Functions;

public class Functions
{
    private readonly IAuthenticationStateRepository _authenticationStateRepository;
    private readonly ISpotifyClient _spotifyClient;
    private readonly ISpotifyUserRepository _spotifyUserRepository;

    public Functions(IAuthenticationStateRepository authenticationStateRepository,
        ISpotifyClient spotifyClient,
        ISpotifyUserRepository spotifyUserRepository)
    {
        _authenticationStateRepository = authenticationStateRepository;
        _spotifyClient = spotifyClient;
        _spotifyUserRepository = spotifyUserRepository;
    }

    [FunctionName(nameof(SpotifyAccessToken))]
    public async Task<IActionResult> SpotifyAccessToken([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "token")] HttpRequest req)
    {
        if (!req.Query.ContainsKey("state")) return new BadRequestResult();

        var stateKey = req.Query["state"];
        var code = req.Query["code"];

        var id = await _authenticationStateRepository.GetSpotifyUserIdByStateKeyAsync(stateKey);
        var userSpotify = await _spotifyClient.GetUserSpotifyAsync(code);
        userSpotify.DiscordId = id;

        await _spotifyUserRepository.AddSpotifyUserAsync(userSpotify);
        return new OkResult();
    }

    [FunctionName(nameof(ClearCacheDaily))]
    public async Task ClearCacheDaily([TimerTrigger("0 0 0 * * *")] TimerInfo timer)
    {
        await _authenticationStateRepository.ClearCacheAsync();
    }
}
