using AutoMapper;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionController> logger;

        public RegionController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAllRegion Action has been invoked");

            var regionsDomain = await regionRepository.GetAllAsync();

            logger.LogInformation($"Finished GetAllRegion Req with Data: {JsonSerializer.Serialize(regionsDomain)}");
            //Map domain models with Dto
            var regionsDto = mapper.Map<List<RegionDTO>>(regionsDomain); //mapper.Map<Destination>(source)

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDTO>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            /*var regionDomainModel = new Region()
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };*/

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);

            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //map domain with dto
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //map domain with dto
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);
            if(region == null )
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDTO>(region);

            return Ok(regionDto);
        }
    }
}

