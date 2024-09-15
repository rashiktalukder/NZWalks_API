using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        //GET: /api/walks?filterOn=Name&filterQuery=track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize=1000)
        {
            var walksList = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            var walkDtoList = mapper.Map<List<WalkDTO>>(walksList);
            return Ok(walkDtoList);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var singleWalk = await walkRepository.GetByIdAsync(id);
            if (singleWalk == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDTO>(singleWalk);
            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            var walkObj = mapper.Map<Walk>(updateWalkRequestDTO);
            var updatedWalk = await walkRepository.UpdateAsync(id, walkObj);
            if(updatedWalk==null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(updatedWalk));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkObj = await walkRepository.DeleteAsync(id);
            if(walkObj == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDTO>(walkObj);
            return Ok(walkDto);
        }
    }
}
