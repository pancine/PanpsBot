namespace PanpsBot.Services.Clients;

public interface IBitlyClient
{
    Task<string> ShortenUrl(string discordId, string longUrl);
}
