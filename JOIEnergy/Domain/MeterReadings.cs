using System.Collections.Generic;

namespace JOIEnergy.Domain
{
    public class MeterReadings
    {
        public string SmartMeterId { get; set; }
        public IList<ElectricityReading> ElectricityReadings { get; set; }
    }
}
