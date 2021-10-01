using EventClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventClient.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient httpClient;

        public HomeController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001");
        }

        public async Task<IActionResult> IndexAsync()
        {
            var res = await SimpleGet();

            return View();
        }

        private async Task<IEnumerable<CodeEventDto>> SimpleGet()
        {
            
            var response = await httpClient.GetAsync("api/events?includelectures=true");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var codeEvents = JsonSerializer.Deserialize<IEnumerable<CodeEventDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


            return codeEvents;
            
        }
    }
}
