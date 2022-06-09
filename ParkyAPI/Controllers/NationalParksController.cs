using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOS;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/NationalParks")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _NPRepository;
        private readonly IMapper _Mapper;
        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _NPRepository = nationalParkRepository;
            _Mapper = mapper;
        }

        /// <summary>
        /// Get All National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalParks()
        {
            var ParksList = _NPRepository.GetNationalParks();
            var ParksDtoList = new List<NationalParkDto>();
            foreach (var Park in ParksList)
                ParksDtoList.Add(_Mapper.Map<NationalParkDto>(Park));

            return Ok(ParksDtoList);
        }

        /// <summary>
        /// Get Indvidual National Park
        /// </summary>
        /// <param name="Id">The Id Of The National Park</param>
        /// <returns></returns>
        [HttpGet("{Id:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize]
        public IActionResult GetNationalPark(int Id)
        {
            var Park = _NPRepository.GetNationalPark(Id);
            if (Park == null)
            {
                return NotFound();
            }
            var ParkDto = _Mapper.Map<NationalParkDto>(Park);

            return Ok(ParkDto);
        }


        /// <summary>
        /// Creating a new National Park
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
                return BadRequest(ModelState);
            if (_NPRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Is Excist");
                return StatusCode(404, ModelState);
            }

            var NationalPark = _Mapper.Map<NationalPark>(nationalParkDto);
            if (!_NPRepository.CreateNAtionalPark(NationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong on saving {NationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { version = HttpContext.GetRequestedApiVersion().ToString(), Id = NationalPark.Id }, NationalPark);
        }


        [HttpPatch("{Id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateNationalPark(int Id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkDto.Id != Id)
            {
                return BadRequest(ModelState);
            }
            var NationalPark = _Mapper.Map<NationalPark>(nationalParkDto);
            if (!_NPRepository.UpdateNationalPark(NationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong on updating {NationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }



        [HttpDelete("{Id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [ProducesResponseType(409)]
        public IActionResult DeleteNationalPark(int Id)
        {
            if (!_NPRepository.NationalParkExists(Id))
                return NotFound();

            var NationalPark = _NPRepository.GetNationalPark(Id);
            if (!_NPRepository.DeleteNationalPark(NationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong on deleting {NationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
