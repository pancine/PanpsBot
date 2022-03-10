using Newtonsoft.Json;

namespace PanpsBot.Models.Requests;

public class BitlyShortenRequest
{
    [JsonProperty("long_url")] public string LongUrl { get; set; }
}
