using PanpsBot.Infrastructure;
using PanpsBot.Models.Requests;
using PanpsBot.Models.Responses;

namespace PanpsBot.Services.Clients;

public class BitlyClient : ClientBase, IBitlyClient
{
    public BitlyClient(HttpClient httpClient) : base(httpClient)
    { }

    public async Task<string> ShortenUrl(string longUrl)
    {
        var bitlyShortenRequest = new BitlyShortenRequest()
        {
            LongUrl = longUrl
        };

        var bitlyShortenResponse = await PostAsync<BitlyShortenResponse>("/v4/shorten", bitlyShortenRequest);

        return bitlyShortenResponse?.Link;
    }
}
