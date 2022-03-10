using Newtonsoft.Json;

namespace PanpsBot.Models.Responses;

public class BitlyShortenResponse
{
    [JsonProperty("link")] public string Link { get; set; }

    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("long_url")] public string LongUrl { get; set; }

    [JsonProperty("archived")] public string Archived { get; set; }

    [JsonProperty("created_at")] public string CreatedAt { get; set; }

    [JsonProperty("custom_bitlinks")] public List<string> CustomBitLinks { get; set; }

    [JsonProperty("tags")] public List<string> Tags { get; set; }

    [JsonProperty("deeplinks")] public List<DeepLink> DeepLinks { get; set; }

    public class DeepLink
    {
        [JsonProperty("guid")] public string Guid { get; set; }

        [JsonProperty("bitlink")] public string BitLink { get; set; }

        [JsonProperty("app_uri_path")] public string AppUriPath { get; set; }

        [JsonProperty("install_url")] public string InstallUrl { get; set; }

        [JsonProperty("app_guid")] public string AppGuid { get; set; }

        [JsonProperty("os")] public string Os { get; set; }

        [JsonProperty("install_type")] public string InstallType { get; set; }

        [JsonProperty("created")] public string Created { get; set; }

        [JsonProperty("modified")] public string Modified { get; set; }

        [JsonProperty("brand_guid")] public string BrandGuid { get; set; }
    }
}
