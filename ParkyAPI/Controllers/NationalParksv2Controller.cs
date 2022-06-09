using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models.DTOS;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/NationalParks")]
    [ApiVersion("2.0")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]
    public class NationalParksv2Controller : ControllerBase
    {
        private readonly INationalParkRepository _NPRepository;
        private readonly IMapper _Mapper;
        public NationalParksv2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
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
    }
}
