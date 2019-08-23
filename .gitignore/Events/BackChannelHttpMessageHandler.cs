using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebMvc.Events
{
    public class BackChannelHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}