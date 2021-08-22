using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public interface IMeterReadingService
    {
        IList<ElectricityReading> GetReadings(string smartMeterId);
        void StoreReadings(string smartMeterId, IEnumerable<ElectricityReading> electricityReadings);
    }
}