using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PanpsBot.Infrastructure;
using PanpsBot.Services;
using PanpsBot.Services.Clients;
using StackExchange.Redis;

[assembly: FunctionsStartup(typeof(PanpsBot.Functions.Startup))]
namespace PanpsBot.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;

        var multiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("redis"));
        builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        builder.Services.AddSingleton<IAuthenticationStateRepository, AuthenticationStateRepository>();
        builder.Services.AddSingleton<ISpotifyUserRepository, SpotifyUserRepository>();

        builder.Services.AddHttpClient<ISpotifyClient, SpotifyClient>();

        builder.Services.AddScoped<ISpotifyService, SpotifyService>();
    }
}
