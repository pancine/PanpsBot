using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PanpsBot;
using PanpsBot.Services.Extensions;
using Serilog;

var hostBuilder = Host.CreateDefaultBuilder()
    .UseSerilog((context, _, config) => config.ReadFrom.Configuration(context.Configuration))
    .ConfigureAppConfiguration((_, config) => { config.Build(); })
    .ConfigureServices((context, services) => {
        services.ConfigureServices(context.Configuration);

        services.AddHostedService<CommandHandler>();
    })
    .ConfigureDiscordHost((context, config) => {
        config.SocketConfig = new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 50
        };
        config.Token = context.Configuration["Bot:Token"];
    })
    .UseCommandService((_, config) => {
        config.CaseSensitiveCommands = false;
        config.DefaultRunMode = RunMode.Async;
        config.IgnoreExtraArgs = true;
        config.LogLevel = LogSeverity.Info;
    });

await hostBuilder.RunConsoleAsync();