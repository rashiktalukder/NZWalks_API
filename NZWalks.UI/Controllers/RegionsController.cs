using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

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
    }
}
