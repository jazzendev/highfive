using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.Provider
{
    public class ApiClientProvider : IApiClientProvider
    {
        private static readonly string QM = "?";
        private static readonly string EM = "=";
        private static readonly string AM = "&";

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        private readonly JsonSerializerSettings _jsonSetting;

        public ApiClientProvider(ILogger<ApiClientProvider> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();


            DefaultContractResolver resolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            _jsonSetting = new JsonSerializerSettings
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public string GetEndpoint(string endpoint, params string[] parameters)
        {
            var newEndpoint = string.IsNullOrEmpty(endpoint) ? "/" : (endpoint.Last() == '/' ? endpoint : endpoint + "/");
            if (parameters != null && parameters.Length > 0)
            {
                var tail = string.Join("/", parameters);
                newEndpoint += tail;
            }
            return newEndpoint;
        }

        private string GetUrl(string endpoint, string host)
        {
            endpoint = endpoint ?? "";
            var url = host + endpoint;
            return url;
        }

        public async Task<T> GetAsync<T>(string endpoint, string host) where T : class
        {
            var url = GetUrl(endpoint, host);
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(string))
                {
                    var result = await response.Content.ReadAsStringAsync() as T;
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<T>();
                    return result;
                }
            }
            return null;
        }

        public async Task<bool> PostAsync<T>(T request, string endpoint, string host)
        {
            var url = GetUrl(endpoint, host);
            var response = await _httpClient.PostAsJsonAsync(url, request);
            return response.IsSuccessStatusCode;
        }

        public async Task<R> PostAsync<T, R>(T request, string endpoint, string host) where R : class
        {
            var url = GetUrl(endpoint, host);
            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                if (typeof(R) == typeof(string))
                {
                    var result = await response.Content.ReadAsStringAsync() as R;
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<R>();
                    return result;
                }
            }
            return null;
        }

        public async Task<R> SendJsonAsync<T, R>(T request, Dictionary<string, string> headers, HttpMethod method, string endpoint, string host) where R : class
        {
            var url = GetUrl(endpoint, host);

            var msg = new HttpRequestMessage(method, url);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    msg.Headers.Add(header.Key, header.Value);
                }
            }
            string json = string.Empty;
            if (request != null)
            {
                json = JsonConvert.SerializeObject(request, _jsonSetting);
            }
            msg.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<R>();
                return result;
            }
            return null;
        }

        public async Task<R> SendFormAsync<R>(IEnumerable<KeyValuePair<string, string>> request, Dictionary<string, string> headers, HttpMethod method, string endpoint, string host) where R : class
        {
            var url = GetUrl(endpoint, host);

            var msg = new HttpRequestMessage(method, url);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    msg.Headers.Add(header.Key, header.Value);
                }
            }

            msg.Content = new FormUrlEncodedContent(request);

            var response = await _httpClient.SendAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<R>();
                return result;
            }
            return null;
        }
    }
}
