using System;
using System.Collections.Generic;
using JOIEnergy.Domain;
using JOIEnergy.Services;
using Xunit;

namespace JOIEnergy.Tests.Services
{
    public class MeterReadingServiceTest
    {
        private const string SmartMeterId = "smart-meter-id";

        private readonly MeterReadingService _meterReadingService;

        public MeterReadingServiceTest()
        {
            _meterReadingService = new MeterReadingService(new Dictionary<string, IList<ElectricityReading>>());

            _meterReadingService.StoreReadings(SmartMeterId, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                new ElectricityReading() { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
            });
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull() {
            Assert.Empty(_meterReadingService.GetReadings("unknown-id"));
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            _meterReadingService.StoreReadings(SmartMeterId, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now, Reading = 25m }
            });

            var electricityReadings = _meterReadingService.GetReadings(SmartMeterId);

            Assert.Equal(3, electricityReadings.Count);
        }

    }
}
