using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventClient.HttpClients
{
    public class CodeEventClient
    {
        public HttpClient HttpClients { get; }

        public CodeEventClient(HttpClient httpClients)
        {
            HttpClients = httpClients;
        }

    }
}
