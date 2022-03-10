using System.Text;
using Newtonsoft.Json;

namespace PanpsBot.Services.Clients;

public class ClientBase
{
    private readonly HttpClient _httpClient;

    protected ClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected static string BuildUrlWithQueryParameters(string url, Dictionary<string, string> parameters)
    {
        if (url == null) throw new ArgumentNullException(nameof(url));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        var query = new StringBuilder("?");
        query.Append(string.Join("&", parameters.Where(param => param.Key != null && param.Value != null).Select(param => param.Key + "=" + param.Value)));

        return url + query.ToString();
    }

    protected async Task<string> GetAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        return await response.Content.ReadAsStringAsync();
    }

    protected async Task PostAsync(string endpoint, object objectToSend)
    {
        if (objectToSend == null) throw new ArgumentNullException(nameof(objectToSend));

        var httpContent = new StringContent(
            JsonConvert.SerializeObject(objectToSend),
            Encoding.UTF8, "application/json");

        await _httpClient.PostAsync(endpoint, httpContent);
    }

    protected async Task<T> PostAsync<T>(string endpoint, object objectToSend)
    {
        if (objectToSend == null) throw new ArgumentNullException(nameof(objectToSend));

        var httpContent = new StringContent(
            JsonConvert.SerializeObject(objectToSend),
            Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, httpContent);
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(content);
    }
}
