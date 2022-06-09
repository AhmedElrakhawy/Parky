using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOS;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/Trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrail")]
    [ProducesResponseType(400)]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _TrRepository;
        private readonly IMapper _Mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _TrRepository = trailRepository;
            _Mapper = mapper;
        }

        /// <summary>
        /// Get All Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesDefaultResponseType]
        public IActionResult GetAllTrails()
        {
            var Trails = _TrRepository.GetAllTrails();
            var TrailDto = new List<TrailDto>();
            foreach (var trail in Trails)
                TrailDto.Add(_Mapper.Map<TrailDto>(trail));
            return Ok(TrailDto);
        }

        /// <summary>
        /// Get Idividual Trail
        /// </summary>
        /// <param name="Id">Trail Id</param>
        /// <returns></returns>
        [HttpGet("{Id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(Trail))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetTrail(int Id)
        {
            var Trail = _TrRepository.GetTrail(Id);
            if (Trail == null)
                return NotFound();
            var TrailDto = _Mapper.Map<TrailDto>(Trail);
            return Ok(TrailDto);
        }

        /// <summary>
        /// Getting the Trails of a National Park
        /// </summary>
        /// <param name="NationalParkId">The Id of The National Park</param>
        /// <returns></returns>
        [HttpGet("GetTrailsInNationPark/{NationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailsInNationPark(int NationalParkId)
        {
            var Trails = _TrRepository.GetTrailInNationalPark(NationalParkId);
            if (Trails == null)
                return NotFound();
            var TrailDto = new List<TrailDto>();
            foreach (var trail in Trails)
            {
                TrailDto.Add(_Mapper.Map<TrailDto>(trail));
            }
            return Ok(TrailDto);
        }


        /// <summary>
        /// Creating a New Trail
        /// </summary>
        /// <param name="trailDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
                return BadRequest(ModelState);
            if (_TrRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Already Excist");
                return StatusCode(404, ModelState);
            }
            var Trail = _Mapper.Map<Trail>(trailDto);
            if (!_TrRepository.CreateTrail(Trail))
            {
                ModelState.AddModelError("", $"Something went wrong when Creating {Trail.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { Id = Trail.Id }, Trail);
        }

        [HttpPatch("{Id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateTrail(int Id, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailDto.Id != Id)
                return BadRequest(ModelState);
            var Trail = _Mapper.Map<Trail>(trailDto);
            if (!_TrRepository.UpdateTrail(Trail))
            {
                ModelState.AddModelError("", $"Something went wrong when Updating {Trail.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{Id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [ProducesResponseType(409)]
        public IActionResult DeleteTrail(int Id)
        {
            if (!_TrRepository.TrailExists(Id))
                return BadRequest(ModelState);
            var Trail = _TrRepository.GetTrail(Id);
            if (!_TrRepository.DeleteTrail(Trail))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting {Trail.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
