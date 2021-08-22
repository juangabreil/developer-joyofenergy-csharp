using System.Collections.Generic;
using JOIEnergy.Controllers;
using JOIEnergy.Domain;
using JOIEnergy.Services;
using Moq;
using Xunit;

namespace JOIEnergy.Tests.Controllers
{
    public class MeterReadingControllerTest
    {
        private readonly MeterReadingController _controller;
        private readonly Mock<IMeterReadingService> _meterReadingServiceMock;
        private readonly Mock<IMeterReadingValidator> _meterReadingValidatorMock;

        public MeterReadingControllerTest()
        {
            _meterReadingServiceMock = new Mock<IMeterReadingService>();
            _meterReadingValidatorMock = new Mock<IMeterReadingValidator>();
            _controller = new MeterReadingController(_meterReadingServiceMock.Object, _meterReadingValidatorMock.Object);
        }

        [Fact]
        public void ShouldReadSmartMeters()
        {
            var meterId = "some-meter";
            var meterList = new List<ElectricityReading>();
            _meterReadingServiceMock.Setup(x => x.GetReadings(meterId)).Returns(meterList);
            var result = _controller.GetReading(meterId);
            
            Assert.Equal(meterList, result.Value);
        }

        [Fact]
        public void ShouldWriteReadings()
        {
            var readings = new MeterReadings();

            _meterReadingValidatorMock.Setup(x => x.IsMeterReadingsValid(readings)).Returns(true);
            
            _controller.Post(readings);
            
            _meterReadingServiceMock.Verify(x => x.StoreReadings(readings.SmartMeterId, readings.ElectricityReadings));
        }
        
        [Fact]
        public void ShouldReturn200WithAnEmptyObject()
        {
            var readings = new MeterReadings();
            _meterReadingValidatorMock.Setup(x => x.IsMeterReadingsValid(readings)).Returns(true);
            var result = _controller.Post(readings);
            
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("{}", result.Value);
        }
        
        [Fact]
        public void ShouldRespondBadRequestWhenReadingsAreInvalid()
        {
            var readings = new MeterReadings();
            
            _meterReadingValidatorMock.Setup(x => x.IsMeterReadingsValid(readings)).Returns(false);
            
            var result = _controller.Post(readings);
            
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Internal Server Error", result.Value);
        }
        
    }
}