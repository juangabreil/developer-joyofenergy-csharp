using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Controllers;
using JOIEnergy.Enums;
using JOIEnergy.Services;
using Moq;
using Xunit;

namespace JOIEnergy.Tests.Controllers
{
    public class PricePlanComparatorControllerTest
    {
        private static readonly IDictionary<string, decimal> Costs = new Dictionary<string, decimal>()
        {
            {$"{Supplier.PowerForEveryone}", 38m},
            {$"{Supplier.TheGreenEco}", 76m},
            {$"{Supplier.DrEvilsDarkEnergy}", 380m}
        };
        
        private readonly PricePlanComparatorController _controller;
        private readonly Mock<IPricePlanService> _pricePlanServiceMock;
        private const string SmartMeterId = "smart-meter-id";

        public PricePlanComparatorControllerTest()
        {
            _pricePlanServiceMock = new Mock<IPricePlanService>();
            _controller = new PricePlanComparatorController(_pricePlanServiceMock.Object);
            _pricePlanServiceMock.Setup(x => x.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SmartMeterId))
                .Returns(Costs);
            _pricePlanServiceMock.Setup(x => x.GetConsumptionCostOfElectricityReadingsForEachPricePlan("not-found"))
                .Returns(new Dictionary<string, decimal>());
        }

        [Fact]
        public void ShouldCalculateCostForMeterReadingsForEveryPricePlan()
        {
            var result = _controller.CalculatedCostForEachPricePlan(SmartMeterId);

            Assert.Equal(Costs, result.Value);
        }

        [Fact]
        public void ShouldRecommendCheapestPricePlansNoLimitForMeterUsage()
        {
            var result = _controller.RecommendCheapestPricePlans(SmartMeterId, null).Value;
            var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

            Assert.Equal("" + Supplier.PowerForEveryone, recommendations[0].Key);
            Assert.Equal("" + Supplier.TheGreenEco, recommendations[1].Key);
            Assert.Equal("" + Supplier.DrEvilsDarkEnergy, recommendations[2].Key);
            Assert.Equal(38m, recommendations[0].Value, 3);
            Assert.Equal(76m, recommendations[1].Value, 3);
            Assert.Equal(380m, recommendations[2].Value, 3);
            Assert.Equal(3, recommendations.Count);
        }

        [Fact]
        public void ShouldRecommendLimitedCheapestPricePlansForMeterUsage() 
        {
            var result = _controller.RecommendCheapestPricePlans(SmartMeterId, 2).Value;
            var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

            Assert.Equal("" + Supplier.PowerForEveryone, recommendations[0].Key);
            Assert.Equal("" + Supplier.TheGreenEco, recommendations[1].Key);
            Assert.Equal(38m, recommendations[0].Value, 3);
            Assert.Equal(76m, recommendations[1].Value, 3);
            Assert.Equal(2, recommendations.Count);
        }

        [Fact]
        public void ShouldRecommendCheapestPricePlansMoreThanLimitAvailableForMeterUsage()
        {
            var result = _controller.RecommendCheapestPricePlans(SmartMeterId, 5).Value;
            var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

            Assert.Equal(3, recommendations.Count);
        }
        
        [Fact]
        public void ShouldReturnNotFoundRecommendingNoMatchingMeterId()
        {
            var response = _controller.RecommendCheapestPricePlans("not-found");
            Assert.Equal(404, response.StatusCode);
            Assert.Equal("Smart Meter ID (not-found) not found", response.Value);
        }

        [Fact]
        public void GivenNoMatchingMeterIdShouldReturnNotFound()
        {
            var response = _controller.CalculatedCostForEachPricePlan("not-found");
            Assert.Equal(404, response.StatusCode);
            Assert.Equal("Smart Meter ID (not-found) not found", response.Value);
        }
    }
}
