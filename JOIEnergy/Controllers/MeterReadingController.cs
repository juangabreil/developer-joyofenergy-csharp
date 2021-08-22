using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [Route("readings")]
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly IMeterReadingValidator _meterReadingValidator;

        public MeterReadingController(IMeterReadingService meterReadingService, IMeterReadingValidator meterReadingValidator)
        {
            _meterReadingService = meterReadingService;
            _meterReadingValidator = meterReadingValidator;
        }

        // POST api/values
        [HttpPost("store")]
        public ObjectResult Post([FromBody] MeterReadings meterReadings)
        {
            if (!_meterReadingValidator.IsMeterReadingsValid(meterReadings))
            {
                return new BadRequestObjectResult("Internal Server Error");
            }

            _meterReadingService.StoreReadings(meterReadings.SmartMeterId, meterReadings.ElectricityReadings);
            return new OkObjectResult("{}");
        }

        [HttpGet("read/{smartMeterId}")]
        public ObjectResult GetReading(string smartMeterId)
        {
            return new OkObjectResult(_meterReadingService.GetReadings(smartMeterId));
        }
    }
}