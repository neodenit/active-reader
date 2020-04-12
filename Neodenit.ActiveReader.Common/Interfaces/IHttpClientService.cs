using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(Uri requestUri);
    }
}