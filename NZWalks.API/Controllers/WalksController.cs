using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
            //Map DTO to Domain model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDTO);

            walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);

            //Map with DTO
            var walkDto = mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDto);
        }
    }
}
