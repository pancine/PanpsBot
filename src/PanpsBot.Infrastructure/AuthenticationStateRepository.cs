using Newtonsoft.Json;
using PanpsBot.Models.Entities;
using StackExchange.Redis;

namespace PanpsBot.Infrastructure;

public class AuthenticationStateRepository : IAuthenticationStateRepository
{
    private readonly IConnectionMultiplexer _multiplexer;

    public AuthenticationStateRepository(IConnectionMultiplexer redis)
    {
        _multiplexer = redis;
    }

    public async Task ClearCacheAsync()
    {
        var endpoints = _multiplexer.GetEndPoints(true);
        foreach (var endpoint in endpoints)
        {
            var server = _multiplexer.GetServer(endpoint);
            await server.FlushAllDatabasesAsync();
        }
    }

    public async Task<string> GetSpotifyUserIdByStateKeyAsync(string key)
    {
        var db = _multiplexer.GetDatabase();
        var content = await db.StringGetAsync(key);
        
        var user = JsonConvert.DeserializeObject<SpotifyUser>(content.ToString());
        return user.DiscordId;
    }

    public async Task SetSpotifyUserByStateKeyAsync(string key, SpotifyUser user)
    {
        var db = _multiplexer.GetDatabase();
        await db.StringSetAsync(key, JsonConvert.SerializeObject(user));
    }
}
