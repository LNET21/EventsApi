using EventClient.HttpClients;
using EventClient.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventClient.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient httpClient;
        private const string json = "application/json";
        private readonly IHttpClientFactory httpClientFactory;
        private readonly CodeEventClient codeEventClient;

        public HomeController(IHttpClientFactory httpClientFactory, CodeEventClient codeEventClient)
        {
            //var client = httpClientFactory.CreateClient();

            httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip });

            httpClient.BaseAddress = new Uri("https://localhost:5001");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            this.httpClientFactory = httpClientFactory;
            this.codeEventClient = codeEventClient;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var cancellation = new CancellationTokenSource();
            //var res = await SimpleGet();
            //var res = await GetWithHttpRequestMessage();
            //var res = await CreateLecture();
            //var res = await CreateLecture2();
            //var res = await Patch();
            //var res = await GetWithStream();
            //var res = await GetWithStreamZip();
            //cancellation.CancelAfter(500);
            //var res = await GetWithCancel(cancellation);
            //var res = await GetWithStreamAndFactory();
            //var res = await GetWithStreamAndFactory2();
            var res = await codeEventClient.GetWithStream();


            return View();
        }

        private async Task<IEnumerable<CodeEventDto>> GetWithStreamAndFactory2()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/events");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            IEnumerable<CodeEventDto> eventsDtos;


            var response = await codeEventClient.HttpClients.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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

        private async Task<IEnumerable<CodeEventDto>> GetWithCancel(CancellationTokenSource cancellation)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/events");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            IEnumerable<CodeEventDto> codeEvents = new List<CodeEventDto>();

            try
            {
                var response = await httpClient.SendAsync(request, cancellation.Token);

                var content = await response.Content.ReadAsStringAsync();

                codeEvents = JsonSerializer.Deserialize<IEnumerable<CodeEventDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


            }
            catch (OperationCanceledException ex)
            {
                //Do something

            }

            return codeEvents;
        }

        private async Task<IEnumerable<CodeEventDto>> GetWithStream()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/events");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            IEnumerable<CodeEventDto> eventsDtos;


            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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
        
        private async Task<IEnumerable<CodeEventDto>> GetWithStreamAndFactory()
        {
            var client = httpClientFactory.CreateClient("EventClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "api/events");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            IEnumerable<CodeEventDto> eventsDtos;


            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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

        private async Task<IEnumerable<CodeEventDto>> GetWithStreamZip()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/events");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            IEnumerable<CodeEventDto> eventsDtos;


            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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

        private async Task<CodeEventDto> Patch()
        {
            var patchDocument = new JsonPatchDocument<CodeEventDto>();
            patchDocument.Remove(e => e.LocationStateProvince);
            patchDocument.Replace(e => e.LocationCountry, "Denmark");
            patchDocument.Add(e => e.Length, 500);

            var serializedPatchDoc = JsonConvert.SerializeObject(patchDocument);

            var request = new HttpRequestMessage(HttpMethod.Patch, "api/events/httpclient");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            request.Content = new StringContent(serializedPatchDoc);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json"); //application/json-patch-json

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var codeEvents = JsonSerializer.Deserialize<CodeEventDto>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return codeEvents;

        }

        private async Task<LectureDto> CreateLecture()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/events/httpclient/lectures");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            var lecture = new LectureCreateDto
            {
                Title = "Create from client",
                Level = 50
            };

            var serializedLecture = JsonSerializer.Serialize(lecture);

            request.Content = new StringContent(serializedLecture);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(json);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var codeEvents = JsonSerializer.Deserialize<LectureDto>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return codeEvents;

        }

        private async Task<LectureDto> CreateLecture2()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/events/httpclient/lectures");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            var lecture = new LectureCreateDto
            {
                Title = "Create from client",
                Level = 50
            };

            request.Content = JsonContent.Create(lecture, typeof(LectureCreateDto), new MediaTypeHeaderValue(json));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var codeEvents = JsonSerializer.Deserialize<LectureDto>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return codeEvents;

        }

        private async Task<IEnumerable<CodeEventDto>> GetWithHttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/events");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            var response = await httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var codeEvents = JsonSerializer.Deserialize<IEnumerable<CodeEventDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return codeEvents;

        }

        private async Task<IEnumerable<CodeEventDto>> SimpleGet()
        {

            var response = await httpClient.GetAsync("api/events?includelectures=true");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var codeEvents = JsonSerializer.Deserialize<IEnumerable<CodeEventDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            //var codeEvents2 = JsonConvert.DeserializeObject<IEnumerable<CodeEventDto>>(content);

            return codeEvents;

        }
    }
}
