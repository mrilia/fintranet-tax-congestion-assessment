using AutoMapper;
using Congestion.Tax.API.Models;
using Congestion.Tax.Business;
using Congestion.Tax.Business.Models.Tax;
using Microsoft.AspNetCore.Mvc;

namespace Congestion.Tax.API.Controllers
{
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ITaxCalculator _taxCalculator;
        private readonly IMapper _mapper;

        public TaxController(ITaxCalculator taxCalculator, IMapper mapper)
        {
            _taxCalculator = taxCalculator;
            _mapper = mapper;
        }

        [HttpGet("/api/tax/")]
        public async Task<ActionResult<double>> GetTax([FromQuery] TaxCalculationRequestDto request)
        {
            var result = _taxCalculator.GetTax(_mapper.Map<TaxCalculationRequest>(request));
            return Ok(result);
        }

    }
}
