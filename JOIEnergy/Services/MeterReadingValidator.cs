using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class MeterReadingValidator : IMeterReadingValidator
    {
        public bool IsMeterReadingsValid(MeterReadings readings)
        {
            if (readings == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(readings.SmartMeterId)
                   && (readings.ElectricityReadings?.Any() ?? false);
        }
    }
}