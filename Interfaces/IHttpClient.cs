namespace CreditAgricoleSdk.Interfaces;

public interface IHttpClient
{
    public Task<T?> GetSingleAsync<T>(string url);
    public Task<T?> GetAsync<T>(string url);
    public Task<T?> GetAsync<T>(string url, IEnumerable<KeyValuePair<string, string>> data);
    public Task<HttpResponseMessage> GetAsync(string url);
    public Task<HttpResponseMessage> GetAsync(string url, IEnumerable<KeyValuePair<string, string>> data);
    public Task<StreamReader> GetAsyncStreamReader(string url);
    public Task<StreamReader> GetAsyncStreamReader(string url, IEnumerable<KeyValuePair<string, string>> data);

    public Task<T?> PostSingleAsync<T>(string url);
    public Task<T?> PostSingleAsync<T>(string url, IEnumerable<KeyValuePair<string, string>> data);
    public Task<T?> PostAsync<T>(string url);
    public Task<HttpResponseMessage> PostAsync(string url, IEnumerable<KeyValuePair<string, string>> data);
    public Task<T?> PostAsync<T>(string url, IEnumerable<KeyValuePair<string, string>> data);
}