using EmployeeMgtApi.Data;
using EmployeeMgtApi.Models;
using EmployeeMgtApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EmployeeMgtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public EmployeesController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET Request: To Fetch all employees
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            ApiResponse<List<Employee>> response;

            try
            {
                // Fetch all active employees from the database
                var allEmployees = _dbContext.Employees.Where(e => e.Status == "ACTV").ToList();

                // Check if there are any employees
                if (allEmployees == null || allEmployees.Count == 0)
                {
                    response = new ApiResponse<List<Employee>>(404, Constants.Messages.EmployeeNotFound, null);
                }
                else
                {
                    response = new ApiResponse<List<Employee>>(200, Constants.Messages.Success, allEmployees);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);

                // Return a response with an internal server error status
                response = new ApiResponse<List<Employee>>(500, Constants.Messages.InternalServerError, null);
            }

            // Return the response with the status and data
            return Ok(response);
        }

        // GET Request: To Fetch employee by id
        [HttpGet("{id:int}")]
        public IActionResult GetEmployeeById(int id)
        {
            ApiResponse<object> response;

            try
            {
                // Fetch the employee by id from the database if they are active
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id && e.Status == "ACTV");
                if (employee == null)
                {
                    response = new ApiResponse<object>(404, Constants.Messages.EmployeeNotFound, null);
                }
                else
                {
                    response = new ApiResponse<object>(200, Constants.Messages.Success, employee);
                }
            }
            catch (Exception)
            {
                // Return a response with an internal server error status
                response = new ApiResponse<object>(500, Constants.Messages.InternalServerError, null);
            }

            // Return the response with the status and data
            return Ok(response);
        }

        // POST Request: To Add a new Employee
        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            ApiResponse<object> response;

            // Validate the input data
            if (string.IsNullOrWhiteSpace(addEmployeeDto.Name) ||
                string.IsNullOrWhiteSpace(addEmployeeDto.Department) ||
                string.IsNullOrWhiteSpace(addEmployeeDto.Position))
            {
                response = new ApiResponse<object>(400, Constants.Messages.InvalidData, null);
                return Ok(response);
            }

            try
            {
                // Create a new employee entity and set its properties
                var employeeEntity = new Employee
                {
                    Name = addEmployeeDto.Name,
                    Department = addEmployeeDto.Department,
                    Position = addEmployeeDto.Position,
                    Status = "ACTV",
                    StatusChangeDate = DateTime.UtcNow
                };

                // Add the new employee to the database and save changes
                _dbContext.Employees.Add(employeeEntity);
                _dbContext.SaveChanges();

                response = new ApiResponse<object>(201, Constants.Messages.EmployeeCreated, employeeEntity);
            }
            catch (Exception)
            {
                // Return a response with an internal server error status
                response = new ApiResponse<object>(500, Constants.Messages.InternalServerError, null);
            }

            // Return the response with the status and data
            return Ok(response);
        }

        // PUT Request: To Update an Employee data by id
        [HttpPut("{id:int}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            ApiResponse<object> response;

            // Validate the input data
            if (string.IsNullOrWhiteSpace(updateEmployeeDto.Name) ||
                string.IsNullOrWhiteSpace(updateEmployeeDto.Department) ||
                string.IsNullOrWhiteSpace(updateEmployeeDto.Position))
            {
                response = new ApiResponse<object>(400, Constants.Messages.InvalidData, null);
                return Ok(response);
            }

            try
            {
                // Fetch the employee by id from the database if they are active
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id && e.Status == "ACTV");
                if (employee == null)
                {
                    response = new ApiResponse<object>(404, Constants.Messages.EmployeeNotFound, null);
                }
                else
                {
                    // Update the employee's properties
                    employee.Name = updateEmployeeDto.Name;
                    employee.Department = updateEmployeeDto.Department;
                    employee.Position = updateEmployeeDto.Position;
                    employee.StatusChangeDate = DateTime.UtcNow;

                    // Save the changes to the database
                    _dbContext.SaveChanges();
                    response = new ApiResponse<object>(200, Constants.Messages.EmployeeUpdated, employee);
                }
            }
            catch (Exception)
            {
                // Return a response with an internal server error status
                response = new ApiResponse<object>(500, Constants.Messages.InternalServerError, null);
            }

            // Return the response with the status and data
            return Ok(response);
        }

        // DELETE Request: To soft delete the employee data
        [HttpDelete("{id:int}")]
        public IActionResult RemoveEmployee(int id)
        {
            ApiResponse<object> response;

            try
            {
                // Fetch the employee by id from the database if they are active
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id && e.Status == "ACTV");
                if (employee == null)
                {
                    response = new ApiResponse<object>(404, Constants.Messages.EmployeeNotFound, null);
                }
                else
                {
                    // Soft delete the employee by setting the status to inactive
                    employee.Status = "INAC";
                    employee.StatusChangeDate = DateTime.UtcNow;

                    // Save the changes to the database
                    _dbContext.SaveChanges();
                    response = new ApiResponse<object>(200, Constants.Messages.EmployeeInactive, null);
                }
            }
            catch (Exception)
            {
                // Return a response with an internal server error status
                response = new ApiResponse<object>(500, Constants.Messages.InternalServerError, null);
            }

            // Return the response with the status and data
            return Ok(response);
        }
    }
}
