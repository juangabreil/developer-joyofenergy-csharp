using System;
using System.Collections.Generic;
using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Services;
using Moq;
using Xunit;

namespace JOIEnergy.Tests.Services
{
    public class PricePlanServiceTest
    {
        private const string SmartMeterId = "smart-meter-id";

        private static readonly List<PricePlan> PricePlans = new List<PricePlan>()
        {
            new PricePlan()
                {EnergySupplier = Supplier.DrEvilsDarkEnergy, UnitRate = 10, PeakTimeMultiplier = NoMultipliers()},
            new PricePlan() {EnergySupplier = Supplier.TheGreenEco, UnitRate = 2, PeakTimeMultiplier = NoMultipliers()},
            new PricePlan()
                {EnergySupplier = Supplier.PowerForEveryone, UnitRate = 1, PeakTimeMultiplier = NoMultipliers()}
        };

        private readonly PricePlanService _pricePlanService;
        private readonly Mock<IMeterReadingService> _meterReadingsServiceMock;

        public PricePlanServiceTest()
        {
            _meterReadingsServiceMock = new Mock<IMeterReadingService>();
            _pricePlanService = new PricePlanService(PricePlans, _meterReadingsServiceMock.Object);
        }

        [Fact]
        public void ShouldCalculateCostForMeterReadingsForEveryPricePlan()
        {
            var electricityReading = new ElectricityReading() {Time = DateTime.Now.AddHours(-1), Reading = 15.0m};
            var otherReading = new ElectricityReading() {Time = DateTime.Now, Reading = 5.0m};
            var readings = new List<ElectricityReading>() {electricityReading, otherReading};
            _meterReadingsServiceMock.Setup(x => x.GetReadings(SmartMeterId)).Returns(readings);

            var actualCosts = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SmartMeterId);

            Assert.Equal(3, actualCosts.Count);
            Assert.Equal(100m, actualCosts[$"{Supplier.DrEvilsDarkEnergy}"], 3);
            Assert.Equal(20m, actualCosts[$"{Supplier.TheGreenEco}"], 3);
            Assert.Equal(10m, actualCosts[$"{Supplier.PowerForEveryone}"], 3);
        }

        [Fact]
        public void ShouldReturnAnEmptyDictionaryIfThereAreLessThanTwoReadings()
        {
            var electricityReading = new ElectricityReading() {Time = DateTime.Now.AddHours(-1), Reading = 15.0m};
            var readings = new List<ElectricityReading>() {electricityReading};
            _meterReadingsServiceMock.Setup(x => x.GetReadings(SmartMeterId)).Returns(readings);

            var actualCosts = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SmartMeterId);

            Assert.Equal(0, actualCosts.Count);
        }

        private static List<PeakTimeMultiplier> NoMultipliers()
        {
            return new List<PeakTimeMultiplier>();
        }
    }
}