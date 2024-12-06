using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using IventoryManagementTestProject.Controllers;
using MyWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;


namespace IventoryManagementTestProject.Controllers
{
   
    namespace MyWebApi.Tests.Controllers
    {
        public class ForkliftControllerTests
        {
            private readonly Mock<IForkliftService> _mockForkliftService;
            private readonly ForkliftController _controller;

            public ForkliftControllerTests()
            {
                _mockForkliftService = new Mock<IForkliftService>();
                _controller = new ForkliftController(_mockForkliftService.Object);
            }

            [Fact]
            public void GetForklifts_ReturnsOk_WhenDataIsAvailable()
            {
                // Arrange
                _mockForkliftService.Setup(service => service.GetForklifts()).Returns(new List<ForkLift> { new ForkLift() });

                // Act
                var result = _controller.GetForklifts();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var forklifts = Assert.IsType<List<ForkLiftResponse>>(okResult.Value);
                forklifts.Should().HaveCount(1); // Ensure there is one forklift in the list
            }

            [Fact]
            public void GetForklifts_ReturnsNotFound_WhenNoData()
            {
                // Arrange
                _mockForkliftService.Setup(service => service.GetForklifts()).Returns(new List<ForkLift>());

                // Act
                var result = _controller.GetForklifts();

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                notFoundResult.Value.Should().Be("No forklifts found.");
            }
        }
    }

}
