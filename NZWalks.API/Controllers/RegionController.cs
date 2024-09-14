using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomain = dbContext.Regions.ToList();

            //Map domain models with Dto
            var regionsDto = new List<RegionDTO>();
            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDTO ()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl

                });
            }

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var region = dbContext.Regions.SingleOrDefault(x=>x.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            RegionDTO regionDto = new RegionDTO()
            {
                Id=region.Id,
                Name=region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpPost]
        public IActionResult CreateRegion([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            var regionDomainModel = new Region()
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            //map domain with dto
            RegionDTO regionDto = new RegionDTO()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(s=>s.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            regionDomainModel.Name = updateRegionRequestDTO.Name;
            regionDomainModel.Code = updateRegionRequestDTO.Code;
            regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;


            //dbContext.Regions.Update(regionDomainModel);
            dbContext.SaveChanges();

            //map domain with dto
            RegionDTO regionDto = new RegionDTO()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteRegion([FromRoute] Guid id)
        {
            var region = dbContext.Regions.FirstOrDefault(x=>x.Id==id);
            if(region == null )
            {
                return NotFound();
            }

            dbContext.Regions.Remove(region);
            dbContext.SaveChanges();

            return Ok($"Region {region.Name} Deleted Successfully");
        }
    }
}

