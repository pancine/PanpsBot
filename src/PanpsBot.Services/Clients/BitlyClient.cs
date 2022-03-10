using PanpsBot.Infrastructure;
using PanpsBot.Models.Requests;
using PanpsBot.Models.Responses;

namespace PanpsBot.Services.Clients;

public class BitlyClient : ClientBase, IBitlyClient
{
    private readonly BitlyInMemoryCollection _shortenedLinks;

    public BitlyClient(BitlyInMemoryCollection bitlyInMemoryCollection, HttpClient httpClient) : base(httpClient)
    {
        _shortenedLinks = bitlyInMemoryCollection;
    }

    public async Task<string> ShortenUrl(string discordId, string longUrl)
    {
        if (_shortenedLinks.TryGetShortenUri(discordId, out var uri))
        {
            return uri;
        }

        var bitlyShortenRequest = new BitlyShortenRequest()
        {
            LongUrl = longUrl
        };

        var bitlyShortenResponse = await PostAsync<BitlyShortenResponse>("/v4/shorten", bitlyShortenRequest);
        _shortenedLinks.SetShortenUri(discordId, bitlyShortenResponse?.Link);

        return bitlyShortenResponse?.Link;
    }
}
