using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public interface IMeterReadingValidator
    {
        bool IsMeterReadingsValid(MeterReadings readings);
    }
}