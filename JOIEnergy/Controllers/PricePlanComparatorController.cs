using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [Route("price-plans")]
    public class PricePlanComparatorController : Controller
    {
        private readonly IPricePlanService _pricePlanService;

        public PricePlanComparatorController(IPricePlanService pricePlanService)
        {
            _pricePlanService = pricePlanService;
        }

        [HttpGet("compare-all/{smartMeterId}")]
        public ObjectResult CalculatedCostForEachPricePlan(string smartMeterId)
        {
            var costPerPricePlan =
                _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
            
            return
                costPerPricePlan.Any()
                    ? new ObjectResult(costPerPricePlan)
                    : new NotFoundObjectResult($"Smart Meter ID ({smartMeterId}) not found");
        }

        [HttpGet("recommend/{smartMeterId}")]
        public ObjectResult RecommendCheapestPricePlans(string smartMeterId, int? limit = null)
        {
            var consumptionForPricePlans =
                _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

            if (!consumptionForPricePlans.Any())
            {
                return new NotFoundObjectResult($"Smart Meter ID ({smartMeterId}) not found");
            }

            IEnumerable<KeyValuePair<string, decimal>> recommendations = consumptionForPricePlans.OrderBy(pricePlanComparison => pricePlanComparison.Value);

            if (limit.HasValue)
            {
                recommendations = recommendations.Take(limit.Value);
            }

            return new ObjectResult(recommendations);
        }
    }
}