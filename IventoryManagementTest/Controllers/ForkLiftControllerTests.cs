using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Linq;
using System.Globalization;
using System.Text.Json;
using IventoryManagement.Controllers;
using IventoryManagement.Models;


public class ForkliftControllerTests
{
    private readonly Mock<ForkliftController> _mockController;

    public ForkliftControllerTests()
    {
        _mockController = new Mock<ForkliftController>();
    }
   

    [Fact]
    public void GetForklifts_ReturnsOkResult_WithRealCSVData()
    {
        // Arrange
        var controller = new ForkliftController();

        // Act
        var result = controller.GetForklifts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnData = Assert.IsAssignableFrom<List<ForkLiftResponse>>(okResult.Value);
        Assert.True(returnData.Count > 0); // Adjust based on your real test data
    }


    [Fact]
    public void LoadFromCsv_ThrowsFormatException_WhenCsvIsNotInExpectedFormat()
    {
        // Arrange
        var invalidCsvContent = "Name,ModelNumber\nForklift1,ABC123";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "testdata.csv");

        // Write invalid CSV to a temporary file
        File.WriteAllText(filePath, invalidCsvContent);

        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => _mockController.Object.LoadFromCsv(filePath));
        Assert.Equal("CSV file is not in the expected format.", exception.Message);

        // Clean up
        File.Delete(filePath);
    }

    [Fact]
    public void LoadFromCsv_ThrowsFormatException_WhenDateIsInvalid()
    {
        // Arrange
        var invalidCsvContent = "Name,ModelNumber,ManufacturingDate\nForklift1,ABC123,InvalidDate";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "testdata.csv");

        // Write invalid CSV to a temporary file
        File.WriteAllText(filePath, invalidCsvContent);

        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => _mockController.Object.LoadFromCsv(filePath));
        Assert.Contains("Invalid date format in the CSV file:", exception.Message);

        // Clean up
        File.Delete(filePath);
    }

    [Fact]
    public void LoadForkliftData_ThrowsFileNotFoundException_WhenNoDataFileExists()
    {
        // Arrange
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "forkliftdata.json");
        var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "testdata.csv");

        // Ensure files do not exist
        if (File.Exists(jsonFilePath)) File.Delete(jsonFilePath);
        if (File.Exists(csvFilePath)) File.Delete(csvFilePath);

        // Act & Assert
        var exception = Assert.Throws<FileNotFoundException>(() => _mockController.Object.LoadForkliftData());
        Assert.Equal("No data file found. Please ensure a .csv or .json file is available.", exception.Message);
    }

    [Fact]
    public void CalculateAge_ReturnsCorrectAge()
    {
        // Arrange
        var manufacturingDate = new DateTime(2020, 1, 1);

        // Act
        var age = _mockController.Object.CalculateAge(manufacturingDate);

        // Assert
        Assert.Equal(4, age); 
    }
}
