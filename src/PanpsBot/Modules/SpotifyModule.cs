using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using Serilog;
using PanpsBot.Services;
using System.Threading.Tasks;
using System;
using PanpsBot.Models.Exceptions;

namespace PanpsBot.Modules;

[Group("spotify")]
public class SpotifyModule : InteractiveBase<SocketCommandContext>
{
    private readonly ISpotifyService _spotifyService;

    public SpotifyModule(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    [Command("login")]
    [Summary("Login a user account to spotify")]
    public async Task Login()
    {
        Log.Information("[{@Command}] {@AuthorName}", Context.Message.Content, Context.User.Username);
        try
        {
            var userId = Context.User.Id.ToString();
            var url = await _spotifyService.GetAccessTokenUri(userId);
            await Context.User.SendMessageAsync($"1. Go to: {url}\n2. Log in and accept\n");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
    }

    [Command("artists")]
    [Summary("Get a user top 30 artists on spotify")]
    public async Task ArtistsStats(string timeRange = "short")
    {
        Log.Information("[{@Command}] {@AuthorName}", Context.Message.Content, Context.User.Username);
        try
        {
            var artistsResult = await _spotifyService.GetUserTopArtistsAsync(Context.User, timeRange);

            await PagedReplyAsync(artistsResult);
        }
        catch (SpotifyUserNotFoundException)
        {
            await ReplyAsync("Run '=spotify login' first");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
    }

    [Command("tracks")]
    public async Task TracksStats(string timeRange = "short")
    {
        Log.Information("[{@Command}] {@AuthorName}", Context.Message.Content, Context.User.Username);
        try
        {
            var tracksResult = await _spotifyService.GetUserTopTracksAsync(Context.User, timeRange);

            await PagedReplyAsync(tracksResult);
        }
        catch (SpotifyUserNotFoundException)
        {
            await ReplyAsync("Run '=spotify login' first");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
    }
}
