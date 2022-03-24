namespace PanpsBot.Services.Clients;

public interface IBitlyClient
{
    Task<string> ShortenUrl(string longUrl);
}
