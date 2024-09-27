using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Simple_Employee_Management_System.Data;
using Simple_Employee_Management_System.Models;
using Simple_Employee_Management_System.Models.ViewModels;
using System.Text.RegularExpressions;

namespace Simple_Employee_Management_System.Controllers
{
    public class JsonController : Controller
    {

        private readonly AppDbContext _context;
        public JsonController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sampleJson = GetSampleJson();
            ViewBag.SampleJson = sampleJson;
            return View();
        }

        [HttpPost]
        public IActionResult ProcessJson([FromBody] string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
            {
                return BadRequest("JSON data is required.");
            }

            try
            {
                var employees = JsonConvert.DeserializeObject<List<EmployeeJson>>(jsonData);

                if (employees == null || !employees.Any())
                {
                    return BadRequest("Invalid JSON format or empty employee list.");
                }

                foreach (var emp in employees)
                {
                    if (!ValidateEmployee(emp))
                    {
                        return BadRequest($"Invalid data for employee: {emp.FirstName} {emp.LastName}");
                    }

                    SaveEmployee(emp);
                }

                return Ok("Employees processed and saved successfully.");
            }
            catch (JsonException ex)
            {
                return BadRequest($"JSON parsing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadJsonFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            if (!file.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only JSON files are allowed.");
            }

            try
            {
                using var streamReader = new StreamReader(file.OpenReadStream());
                var jsonContent = await streamReader.ReadToEndAsync();
                var employees = JsonConvert.DeserializeObject<List<EmployeeJson>>(jsonContent);

                if (employees == null || !employees.Any())
                {
                    return BadRequest("Invalid JSON format or empty employee list.");
                }

                var validationErrors = new List<string>();
                foreach (var emp in employees)
                {
                    if (!ValidateEmployee(emp))
                    {
                        validationErrors.Add($"Invalid data for employee: {emp.FirstName} {emp.LastName}");
                    }
                }

                if (validationErrors.Any())
                {
                    return BadRequest(string.Join("\n", validationErrors));
                }

                foreach (var emp in employees)
                {
                    SaveEmployee(emp);
                }

                return Ok($"{employees.Count} employees processed and saved successfully.");
            }
            catch (JsonException ex)
            {
                return BadRequest($"JSON parsing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private bool ValidateEmployee(EmployeeJson emp)
        {
            if (string.IsNullOrEmpty(emp.FirstName) || string.IsNullOrEmpty(emp.LastName) ||
                string.IsNullOrEmpty(emp.Email) || emp.DateOfJoining == default ||
                emp.Department == null || emp.Address == null)
            {
                return false;
            }

            if (!Regex.IsMatch(emp.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return false;
            }


            return true;
        }

        private void SaveEmployee(EmployeeJson empJson)
        {
            var employee = new Employee
            {
                FirstName = empJson.FirstName,
                LastName = empJson.LastName,
                Email = empJson.Email,
                DateOfJoining = empJson.DateOfJoining,
                Salary = empJson.Salary,
                Department = new Department { DepartmentName = empJson.Department.DepartmentName },
                Address = new Address
                {
                    Street = empJson.Address.Street,
                    City = empJson.Address.City,
                    State = empJson.Address.State,
                    PostalCode = empJson.Address.PostalCode
                },
                Dependents = empJson.Dependents?.Select(d => new Dependent
                {
                    Name = d.Name,
                    Relationship = d.Relationship,
                    DateOfBirth = d.DateOfBirth
                }).ToList(),
                Projects = empJson.Projects?.Select(p => new Project
                {
                    ProjectName = p.ProjectName,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Role = p.Role
                }).ToList()
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        private string GetSampleJson()
        {
            // Return the sample JSON string here
            return @"[
  {
    ""EmployeeId"": 1,
    ""FirstName"": ""John"",
    ""LastName"": ""Doe"",
    ""Email"": ""john.doe@example.com"",
    ""DateOfJoining"": ""2023-01-15T00:00:00"",
    ""Department"": {
      ""DepartmentId"": 1,
      ""DepartmentName"": ""HR""
    },
    ""Salary"": 50000,
    ""Address"": {
      ""Street"": ""123 Main St"",
      ""City"": ""Springfield"",
      ""State"": ""IL"",
      ""PostalCode"": ""62701""
    },
    ""Dependents"": [
      {
        ""DependentId"": 1,
        ""Name"": ""Jane Doe"",
        ""Relationship"": ""Spouse"",
        ""DateOfBirth"": ""1990-05-12T00:00:00""
      }
    ],
    ""Projects"": [
      {
        ""ProjectId"": 101,
        ""ProjectName"": ""HR System Revamp"",
        ""StartDate"": ""2022-01-10T00:00:00"",
        ""EndDate"": ""2022-12-31T00:00:00"",
        ""Role"": ""Project Manager""
      }
    ]
  }
]";
        }
    }
}
