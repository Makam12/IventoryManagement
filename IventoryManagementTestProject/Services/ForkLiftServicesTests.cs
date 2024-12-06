using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Xunit;
using IventoryManagementTestProject.Services;
using FluentAssertions;
using System.Threading.Tasks;

namespace IventoryManagementTestProject.Services
{
   

    namespace MyWebApi.Tests.Services
    {
        public class ForkliftServiceTests
        {
            private readonly Mock<IForkliftRepository> _mockRepository;
            private readonly ForkliftService _service;

            public ForkliftServiceTests()
            {
                _mockRepository = new Mock<IForkliftRepository>();
                _service = new ForkliftService(_mockRepository.Object);
            }

            [Fact]
            public void GetForklifts_ShouldReturnList_WhenDataIsAvailable()
            {
                // Arrange
                _mockRepository.Setup(repo => repo.GetAllForklifts()).Returns(new List<ForkLift> { new ForkLift() });

                // Act
                var result = _service.GetForklifts();

                // Assert
                result.Should().HaveCount(1); // Ensure there is one forklift in the list
            }
        }
    }

}
