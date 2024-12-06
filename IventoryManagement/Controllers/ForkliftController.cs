using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IventoryManagement.Models;
using IventoryManagement.Services;
using System.Linq.Expressions;

namespace IventoryManagement.Controllers
{
    [ApiController]
    [Route("api/forklifts")]
    public class ForkliftController : ControllerBase
    {

        private readonly ForkLiftPositionTracking _forkLiftPositionTracking;

        public ForkliftController()
        {
            _forkLiftPositionTracking = new ForkLiftPositionTracking();
        }


        /// <summary>
        /// This GetForklifts method is implemented for Exercise1 To display the Forkliftdata for operators in the warehouse
        /// </summary>
        /// <returns></returns>


        [HttpGet]
        public IActionResult GetForklifts()
        {
            try
            {
                // Load forklift data
                var forklifts = LoadForkliftData();

                // Return data as JSON response
                return Ok(forklifts);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Message = "An error occurred while loading forklift data.", Error = ex.Message });
            }
        }


        public  List<ForkLiftResponse> LoadForkliftData()
        {
            // Path to the data file (ensure this path is correct in your project)
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "forkliftdata.json");
            string csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "testdata.csv");

            // Choose JSON or CSV based on the file format available
            if (System.IO.File.Exists(jsonFilePath))
            {
                return LoadFromJson(jsonFilePath);
            }
            else if (System.IO.File.Exists(csvFilePath))
            {
                return LoadFromCsv(csvFilePath);
            }
            else
            {
                throw new FileNotFoundException("No data file found. Please ensure a .csv or .json file is available.");
            }
        }

        public List<ForkLiftResponse> LoadFromJson(string filePath)
        {
            // Read JSON file content
            var jsonData = System.IO.File.ReadAllText(filePath);

            // Deserialize JSON into Forklift objects
            var forklifts = JsonSerializer.Deserialize<List<ForkLift>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Calculate age and map to response model
            return forklifts?.Select(f => new ForkLiftResponse
            {
                Name = f.Name,
                ModelNumber = f.ModelNumber,
                ManufacturingDate = f.ManufacturingDate,
                Age = CalculateAge(f.ManufacturingDate)
            }).ToList() ?? new List<ForkLiftResponse>();
        }

        public List<ForkLiftResponse> LoadFromCsv(string filePath)
        {
            var forklifts = new List<ForkLift>();

            using (var reader = new StreamReader(filePath))
            {
                // Read CSV header
                var header = reader.ReadLine();

                // Ensure file has expected columns
                if (header == null || !header.Contains("Name,ModelNumber,ManufacturingDate"))
                {
                    throw new FormatException("CSV file is not in the expected format.");
                }

                // Read each line
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var columns = line.Split(',');

                    // Parse and validate the ManufacturingDate
                    DateTime manufacturingDate;
                    var dateFormats = new[] { "yyyy-MM-dd", "MM/dd/yyyy", "M/d/yyyy" }; // Add more formats if needed
                    if (!DateTime.TryParseExact(columns[2].Trim(), dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out manufacturingDate))
                    {
                        throw new FormatException($"Invalid date format in the CSV file: {columns[2]}");
                    }

                    // Add forklift to the list
                    forklifts.Add(new ForkLift
                    {
                        Name = columns[0].Trim(),
                        ModelNumber = columns[1].Trim(),
                        ManufacturingDate = manufacturingDate
                    });
                }
            }

            // Calculate age and map to response model
            return forklifts.Select(f => new ForkLiftResponse
            {
                Name = f.Name,
                ModelNumber = f.ModelNumber,
                ManufacturingDate = f.ManufacturingDate,
                Age = CalculateAge(f.ManufacturingDate)
            }).ToList();
        
    }

        public int CalculateAge(DateTime manufacturingDate)
        {
            var today = DateTime.Today;
            var age = today.Year - manufacturingDate.Year;

            if (manufacturingDate.Date > today.AddYears(-age)) age--;

            return age;
        }
/// <summary>
/// This method is implemented for Exercise3 for Forkliftposition Tracking by parsing commands based on the given input
/// </summary>
/// <param name="request"></param>
/// <returns></returns>

        [HttpPost("PositionTracking")]
        public IActionResult Simulate([FromBody] CommandRequest request)
        {
            try
            {
                var result = _forkLiftPositionTracking.ProcessCommands(request.Commands);
                return Ok(new
                {
                    log = result.log,
                    message = result.message,
                    position = new { x = result.x, y = result.y, direction = result.direction }
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Message = "An error occurred while processing the data.", Error = ex.Message });
            }
        }
     
    }
}

  

   


   
