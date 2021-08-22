using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public static class CostCalculator
    {
        public static decimal CalculateCost(IList<ElectricityReading> electricityReadings, PricePlan pricePlan)
        {
            var average = CalculateAverageReading(electricityReadings);
            var timeElapsed = CalculateTimeElapsed(electricityReadings);
            var averagedCost = average / timeElapsed;
            return averagedCost * pricePlan.UnitRate;
        }

        private static decimal CalculateAverageReading(IList<ElectricityReading> electricityReadings)
        {
            var newSummedReadings = electricityReadings.Select(readings => readings.Reading)
                .Aggregate((reading, accumulator) => reading + accumulator);

            return newSummedReadings / electricityReadings.Count();
        }

        private static decimal CalculateTimeElapsed(IList<ElectricityReading> electricityReadings)
        {
            var first = electricityReadings.Min(reading => reading.Time);
            var last = electricityReadings.Max(reading => reading.Time);

            return (decimal) (last - first).TotalHours;
        }
    }
}