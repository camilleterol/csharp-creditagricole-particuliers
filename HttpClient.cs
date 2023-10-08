using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CreditAgricoleSdk.Interfaces;
using BaseHttpClient = System.Net.Http.HttpClient;

namespace CreditAgricoleSdk;

public class HttpClient : IHttpClient
{
    private static readonly CookieContainer cookies = new();

    private static readonly HttpClientHandler handler = new()
    {
        AutomaticDecompression = DecompressionMethods.All,
        AllowAutoRedirect = true,
        CookieContainer = cookies,
        UseCookies = true,
    };
    
    private static readonly BaseHttpClient SharedClient = new(handler)
    {
        BaseAddress = new Uri("https://www.credit-agricole.fr"),
        DefaultRequestHeaders =
        {
            UserAgent = {
                new ProductInfoHeaderValue("CreditAgricoleSdk", "1.0"),
            },
        },
    };

    public async Task<T?> GetSingleAsync<T>(string url) => (await GetAsync<IEnumerable<T>>(url) ?? Enumerable.Empty<T>()).FirstOrDefault();

    public async Task<T?> GetAsync<T>(string url) => await SharedClient.GetFromJsonAsync<T>(url);
    
    public async Task<T?> GetAsync<T>(string url, IEnumerable<KeyValuePair<string, string>> data) => await SharedClient.GetFromJsonAsync<T>($"{url}?{await new FormUrlEncodedContent(data).ReadAsStringAsync()}");

    public async Task<HttpResponseMessage> GetAsync(string url) => await SharedClient.GetAsync(url);

    public async Task<HttpResponseMessage> GetAsync(string url, IEnumerable<KeyValuePair<string, string>> data) => await SharedClient.GetAsync($"{url}?{await new FormUrlEncodedContent(data).ReadAsStringAsync()}");

    public async Task<StreamReader> GetAsyncStreamReader(string url) => new(await SharedClient.GetStreamAsync(url));
    
    public async Task<StreamReader> GetAsyncStreamReader(string url, IEnumerable<KeyValuePair<string, string>> data) => new(await SharedClient.GetStreamAsync($"{url}?{await new FormUrlEncodedContent(data).ReadAsStringAsync()}"));

    public async Task<T?> PostSingleAsync<T>(string url) => (await PostAsync<IEnumerable<T>>(url) ?? Enumerable.Empty<T>()).FirstOrDefault();

    public async Task<T?> PostSingleAsync<T>(string url, IEnumerable<KeyValuePair<string, string>> data) => (await PostAsync<IEnumerable<T>>(url, data) ?? Enumerable.Empty<T>()).FirstOrDefault();

    public async Task<HttpResponseMessage> PostAsync(string url, IEnumerable<KeyValuePair<string, string>> data)
    {
        var content = new FormUrlEncodedContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        
        return await SharedClient.PostAsync(url, content);
    }
    
    public async Task<T?> PostAsync<T>(string url)
    {
        using HttpResponseMessage response = await SharedClient.PostAsync(url, null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }
    
    public async Task<T?> PostAsync<T>(string url, IEnumerable<KeyValuePair<string, string>> data)
    {
        var content = new FormUrlEncodedContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        
        using HttpResponseMessage response = await SharedClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }
}