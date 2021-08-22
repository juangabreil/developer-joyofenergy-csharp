using System.Collections.Generic;

namespace JOIEnergy.Services
{
    public interface IPricePlanService
    {
        IDictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId);
    }
}