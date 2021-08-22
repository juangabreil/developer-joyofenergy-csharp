using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class PricePlanService : IPricePlanService
    {

        private readonly IList<PricePlan> _pricePlans;
        private readonly IMeterReadingService _meterReadingService;

        public PricePlanService(IList<PricePlan> pricePlan, IMeterReadingService meterReadingService)
        {
            _pricePlans = pricePlan;
            _meterReadingService = meterReadingService;
        }

        public IDictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            var electricityReadings = _meterReadingService.GetReadings(smartMeterId);

            if (electricityReadings.Count() < 2)
            {
                return new Dictionary<string, decimal>();
            }
            
            return _pricePlans.ToDictionary(
                plan => plan.EnergySupplier.ToString(), 
                plan => CostCalculator.CalculateCost(electricityReadings, plan));
        }
    }
}
