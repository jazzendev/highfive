using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.Provider
{
    public interface IApiClientProvider
    {
        Task<T> GetAsync<T>(string endpoint, string host) where T : class;
        Task<bool> PostAsync<T>(T request, string endpoint, string host);
        Task<R> PostAsync<T, R>(T request, string endpoint, string host) where R : class;
        Task<R> SendJsonAsync<T, R>(T request, Dictionary<string, string> headers, HttpMethod method, string endpoint, string host) where R : class;
        Task<R> SendFormAsync<R>(IEnumerable<KeyValuePair<string, string>> request, Dictionary<string, string> headers, HttpMethod method, string endpoint, string host) where R : class;
    }
}
