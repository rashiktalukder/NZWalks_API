using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                //Get all Regions Info from API

                var client = httpClientFactory.CreateClient();
                var httpResponseMsg = await client.GetAsync("https://localhost:7036/api/Region");
                httpResponseMsg.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMsg.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>()?? new List<RegionDto>());

            }
            catch (Exception ex)
            {
                throw;
            }
            return View(response);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7036/api/Region"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpResponseMsg = await client.SendAsync(httpRequestMessage);
            httpResponseMsg.EnsureSuccessStatusCode();

            var response = await httpResponseMsg.Content.ReadFromJsonAsync<RegionDto>();
            if(response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7036/api/Region/{id.ToString()}");
            if(response is not null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto regionDto)
        {
            var client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7036/api/Region/{regionDto.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(regionDto), Encoding.UTF8, "application/json")
            };

            var httpRespMsg = await client.SendAsync(request);
            httpRespMsg.EnsureSuccessStatusCode();

            var response = await httpRespMsg.Content.ReadFromJsonAsync<RegionDto>();
            if(response is not null )
            {
                // show Message
                TempData["Message"] = "Region has been updated";
                return RedirectToAction("Edit", "Regions");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7036/api/Region/{id.ToString()}");
            if (response is not null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var client = httpClientFactory.CreateClient();
            var httpRespMsg = await client.DeleteAsync($"https://localhost:7036/api/Region/{id.ToString()}");

            httpRespMsg.EnsureSuccessStatusCode();

            if (httpRespMsg.IsSuccessStatusCode)
            {
                TempData["Message"] = "Region has been Deleted";
                return RedirectToAction("Index", "Regions");
            }
            return View();
        }
    }
}
