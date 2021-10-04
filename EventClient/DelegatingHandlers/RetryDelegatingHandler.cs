using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EventClient.DelegatingHandlers
{
    public class RetryDelegatingHandler : DelegatingHandler
    {
        private const int nrOfTimes = 2;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = default;
            for (int i = 0; i < nrOfTimes; i++)
            {
                 response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

            }

            return response;
        }
    }
}
