using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IDictionary<string, IList<ElectricityReading>> _meterAssociatedReadings;

        public MeterReadingService(IDictionary<string, IList<ElectricityReading>> meterAssociatedReadings)
        {
            _meterAssociatedReadings = meterAssociatedReadings;
        }

        public IList<ElectricityReading> GetReadings(string smartMeterId)
        {
            if (_meterAssociatedReadings.ContainsKey(smartMeterId))
            {
                return _meterAssociatedReadings[smartMeterId];
            }

            return new List<ElectricityReading>();
        }

        public void StoreReadings(string smartMeterId, IEnumerable<ElectricityReading> electricityReadings)
        {
            if (!_meterAssociatedReadings.ContainsKey(smartMeterId))
            {
                _meterAssociatedReadings.Add(smartMeterId, new List<ElectricityReading>());
            }

            foreach (var reading in electricityReadings)
            {
                _meterAssociatedReadings[smartMeterId].Add(reading);
            }
        }
    }
}