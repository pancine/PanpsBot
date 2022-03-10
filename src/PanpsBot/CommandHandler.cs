using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PanpsBot.Modules;

namespace PanpsBot;

public class CommandHandler : DiscordClientService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _services;
    private readonly DiscordSocketClient _client;
    private readonly CommandService _command;

    public CommandHandler(IConfiguration configuration, IServiceProvider services, CommandService commandService,
        DiscordSocketClient client, ILogger<DiscordClientService> logger) : base(client, logger)
    {
        _configuration = configuration;
        _services = services;

        _command = commandService;
        _client = client;
    }

    protected async override Task ExecuteAsync(CancellationToken _)
    {
        _client.MessageReceived += HandleMessageAsync;

        await _command.AddModuleAsync<SpotifyModule>(_services);
    }

    private async Task HandleMessageAsync(SocketMessage messageParameter)
    {
        if (messageParameter is not SocketUserMessage message) return;

        var argPos = 0;

        if (!(message.HasStringPrefix(_configuration["Bot:Prefix"], ref argPos) ||
              message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
              message.Author.IsBot) 
            return;

        var context = new SocketCommandContext(_client, message);

        await _command.ExecuteAsync(context, argPos, _services);
    }
}
