using EventClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventClient.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient httpClient;
        private const string json = "application/json";

        public HomeController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
        }

        public async Task<IActionResult> IndexAsync()
        {
            //var res = await SimpleGet();
            //var res = await GetWithHttpRequestMessage();
            var res = await CreateLecture();

            return View();
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

            var response =  await httpClient.SendAsync(request);
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
