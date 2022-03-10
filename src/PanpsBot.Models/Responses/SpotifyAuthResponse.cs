using Newtonsoft.Json;

namespace PanpsBot.Models.Responses;

public class SpotifyAuthResponse
{
    [JsonProperty("args")] public Args Args { get; set; }

    [JsonProperty("url")] public string Url { get; set; }
}

public class Args
{
    [JsonProperty("code")] public string Code { get; set; }
}
