using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;
using JOIEnergy.Enums;

namespace JOIEnergy.Generator
{
    public static class ElectricityReadingGenerator
    {
        public static IDictionary<string, IList<ElectricityReading>> GenerateMeterElectricityReadings(IDictionary<string, Supplier> smartMeterToPricePlanAccounts) {
            var smartMeterIds = smartMeterToPricePlanAccounts.Select(mtpp => mtpp.Key);

            return smartMeterIds.ToDictionary<string, string, IList<ElectricityReading>>(
                smartMeterId => smartMeterId, 
                _ => ElectricityReadingGenerator.Generate(20)
            );
        }
        
        private static List<ElectricityReading> Generate(int number)
        {
            var readings = new List<ElectricityReading>();
            var random = new Random();
            for (var i = 0; i < number; i++)
            {
                var reading = (decimal)random.NextDouble();
                var electricityReading = new ElectricityReading
                {
                    Reading = reading,
                    Time = DateTime.Now.AddSeconds(-i * 10)
                };
                readings.Add(electricityReading);
            }
            readings.Sort((reading1, reading2) => reading1.Time.CompareTo(reading2.Time));
            return readings;
        }
    }
}
