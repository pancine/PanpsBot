namespace PanpsBot.Infrastructure;

public class BitlyInMemoryCollection
{
    private readonly Dictionary<string, string> ShortenedLinks = new();

    public bool TryGetShortenUri(string discordId, out string uri)
    {
        if (ShortenedLinks.TryGetValue(discordId, out var shortenedLink))
        {
            uri = shortenedLink;
            return true;
        }

        uri = null;
        return false;
    }

    public void SetShortenUri(string discordId, string uri)
    {
        ShortenedLinks.Add(discordId, uri);
    }
}
