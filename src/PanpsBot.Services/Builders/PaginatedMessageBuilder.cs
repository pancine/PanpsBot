using System.Text;
using Discord;
using Discord.Addons.Interactive;
using SpotifyAPI.Web;

namespace PanpsBot.Services.Builders;

public class PaginatedMessageBuilder
{
    private PaginatedMessage PaginatedMessage { get; }
    private string Username { get; set; }

    public PaginatedMessageBuilder()
    {
        PaginatedMessage = new PaginatedMessage();
    }

    public PaginatedMessage Build()
    {
        return PaginatedMessage;
    }

    public PaginatedMessageBuilder BuildWithUser(string username)
    {
        Username = username;
        return this;
    }

    public PaginatedMessageBuilder BuildSpotifyArtists(IPaginatable<FullArtist> artists)
    {
        PaginatedMessage.Title = (Username is null ? "" : Username + " ") + "Top Artists";
        PaginatedMessage.Color = Color.Green;

        if (artists.Items != null)
        {
            var pages = Paginate(artists.Items.Select(art => art.Name).ToList());
            PaginatedMessage.Pages = pages;
        }
        else
        {
            PaginatedMessage.Pages = new List<string> { "User does not have Top Artists" };
        }

        return this;
    }

    public PaginatedMessageBuilder BuildSpotifyTracks(IPaginatable<FullTrack> tracks)
    {
        PaginatedMessage.Title = (Username is null ? "" : Username + " ") + "Top Artists";
        PaginatedMessage.Color = Color.Green;

        if (tracks.Items != null)
        {
            var pages = Paginate(tracks.Items.Select(track => $"{track.Name} - {track.Artists.FirstOrDefault()?.Name}").ToList());
            PaginatedMessage.Pages = pages;
        }
        else
        {
            PaginatedMessage.Pages = new List<string> { "User does not have Top Tracks" };
        }

        return this;
    }

    private List<string> Paginate(List<string> items, int pageSize = 10)
    {
        var pagesCount = Convert.ToInt32(Math.Ceiling(items.Count / Convert.ToDouble(pageSize)));
        var pages = new List<string>(pagesCount);
        var page = new StringBuilder();

        for (var pageNumber = 0; pageNumber < pagesCount; pageNumber++)
        {
            page.AppendLine($"Page {pageNumber + 1}:\n");

            var itemsInPage = items.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            for (var i = 0; i < itemsInPage.Count; i++)
            {
                page.AppendLine($"{pageNumber * pageSize + i + 1}: {itemsInPage[i]}\n");
            }

            pages.Add(page.ToString());
            page.Clear();
        }

        return pages;
    }
}
