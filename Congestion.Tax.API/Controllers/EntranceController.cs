using AutoMapper;
using Congestion.Tax.API.Models;
using Congestion.Tax.Business;
using Congestion.Tax.Business.Models.Entrance;
using Congestion.Tax.Business.Models.Tax;
using Microsoft.AspNetCore.Mvc;

namespace Congestion.Tax.API.Controllers
{
    [ApiController]
    public class EntranceController : ControllerBase
    {
        private readonly IEntranceRepository _entrancerepo;
        private readonly IMapper _mapper;

        public EntranceController(IEntranceRepository enteranceRepo, IMapper mapper)
        {
            _entrancerepo = enteranceRepo;
            _mapper = mapper;
        }

        [HttpGet("/api/entrances")]
        public async Task<ActionResult<IEnumerable<EntranceDto>>> GetAll([FromQuery] FilterDto filter)
        {
            var entrances = _entrancerepo.GetAllAsync()?.Result
                ?.Where(i=> 
                            i.Datetime >= filter.From && 
                            i.Datetime <= filter.To && 
                            i.VehicleNumberPlate.Trim().ToLower() == filter.VehicleNumberPlate.Trim().ToLower())
                ?.ToList()
                ?? new List<Entrance>();

            var result = _mapper.Map<List<EntranceDto>>(entrances);
            return Ok(entrances);
        }

        [HttpPost("/api/entrances")]
        public async Task<ActionResult<EntranceDto>> Create(EntranceDto entranceDto)
        {
            await _entrancerepo.AddAsync(_mapper.Map<Entrance>(entranceDto));

            return Ok(entranceDto);
        }
    }
}
