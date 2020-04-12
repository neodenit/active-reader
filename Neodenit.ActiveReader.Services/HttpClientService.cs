using System;
using System.Net.Http;
using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient HttpClient = new HttpClient();

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri) => await HttpClient.GetAsync(requestUri);
    }
}
