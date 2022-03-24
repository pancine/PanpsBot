using System.Net.Http.Headers;
using Discord.Addons.Interactive;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PanpsBot.Infrastructure;
using PanpsBot.Services.Clients;
using StackExchange.Redis;

namespace PanpsBot.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<InteractiveService>();

        var multiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("redis"));
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        services.AddSingleton<IAuthenticationStateRepository, AuthenticationStateRepository>();
        services.AddScoped<ISpotifyUserRepository, SpotifyUserRepository>();

        services.AddHttpClient<ISpotifyClient, SpotifyClient>();
        services.AddHttpClient<IBitlyClient, BitlyClient>(client => {
            client.BaseAddress = new Uri(configuration["Bitly:BaseUrl"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["Bitly:Token"]);
        });

        services.AddScoped<ISpotifyService, SpotifyService>();
    }
}
