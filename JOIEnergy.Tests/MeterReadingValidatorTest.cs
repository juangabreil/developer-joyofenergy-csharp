using System.Collections.Generic;
using JOIEnergy.Domain;
using JOIEnergy.Services;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingValidatorTest
    {
        private readonly MeterReadingValidator _validator;

        public MeterReadingValidatorTest()
        {
            _validator = new MeterReadingValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldReturnFalseValidatingReadingsWhenSmartIdIsNotDefined(string meterId)
        {
            var readings = new MeterReadings()
            {
                SmartMeterId = meterId
            };
            
            Assert.False(_validator.IsMeterReadingsValid(readings));
        }
        
        [Fact]
        public void ShouldReturnFalseValidatingReadingsWhenThereAreNoReadings()
        {
            var readings = new MeterReadings()
            {
                SmartMeterId = "some-id"
            };
            
            Assert.False(_validator.IsMeterReadingsValid(readings));
        }
        
        [Fact]
        public void ShouldReturnFalseValidatingReadingsWhenReadingsIsEmpty()
        {
            var readings = new MeterReadings()
            {
                SmartMeterId = "some-id",
                ElectricityReadings = new List<ElectricityReading>()
            };
            
            Assert.False(_validator.IsMeterReadingsValid(readings));
        }
        
        [Fact]
        public void ShouldReturnTrueWhenReadingsAreValid()
        {
            var readings = new MeterReadings()
            {
                SmartMeterId = "some-id",
                ElectricityReadings = new List<ElectricityReading>()
                {
                    new ElectricityReading()
                }
            };
            
            Assert.True(_validator.IsMeterReadingsValid(readings));
        }
    }
}