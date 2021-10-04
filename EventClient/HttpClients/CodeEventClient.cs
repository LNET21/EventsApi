using EventClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EventClient.HttpClients
{
    public class CodeEventClient
    {
        public HttpClient HttpClients { get; }

        public CodeEventClient(HttpClient httpClients)
        {
            HttpClients = httpClients;
            HttpClients.BaseAddress = new Uri("https://localhost:5001");
            HttpClients.DefaultRequestHeaders.Clear();
            HttpClients.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<CodeEventDto>> GetWithStream()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/events");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            IEnumerable<CodeEventDto> eventsDtos;


            var response = await HttpClients.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                response.EnsureSuccessStatusCode();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        var serializer = new Newtonsoft.Json.JsonSerializer();
                        eventsDtos = serializer.Deserialize<IEnumerable<CodeEventDto>>(jsonReader);
                    }
                }
            }

            return eventsDtos;

        }

    }

    public class EventClient2 : BaseClient, IEventClient2
    {
        //public HttpClient HttpClients { get; }

        public EventClient2(HttpClient httpClients) : base(httpClients, new Uri("https://localhost:5001"))
        {
            // HttpClients = httpClients;
            // HttpClients.BaseAddress = new Uri("https://localhost:5001");
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<CodeEventDto>> GetAll(CancellationToken token)
        {
            return await base.GetAsync<IEnumerable<CodeEventDto>>(token, "api/events");
        }

        public async Task<CodeEventDto> Get(CancellationToken token, string name)
        {
            return await base.GetAsync<CodeEventDto>(token, $"api/events/{name}");
        }

        public async Task<IEnumerable<LectureDto>> GetLectures(CancellationToken token, string name)
        {
            return await base.GetAsync<IEnumerable<LectureDto>>(token, $"api/events/{name}/lectures");
        }

    }
}
