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

        // GET Req : To Fetch all employees
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            ApiResponse<List<Employee>> response;

            try
            {
                var allEmployees = _dbContext.Employees.Where(e => e.Status == EmployeeStatus.Active).ToList();

                // Ensure you are getting all employees
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

                response = new ApiResponse<List<Employee>>(500, Constants.Messages.InternalServerError, null);
            }

            return Ok(response);
        }

        // GET Req : To Fetch employee by id
        [HttpGet("{id:int}")]
        public IActionResult GetEmployeeById(int id)
        {
            ApiResponse<object> response;

            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id && e.Status == EmployeeStatus.Active);
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
                response = new ApiResponse<object>(500, Constants.Messages.InternalServerError, null);
            }

            return Ok(response);
        }

        // POST Req: To Add the Employee
        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            ApiResponse<object> response;

            if (string.IsNullOrWhiteSpace(addEmployeeDto.Name) ||
                string.IsNullOrWhiteSpace(addEmployeeDto.Department) ||
                string.IsNullOrWhiteSpace(addEmployeeDto.Position))
            {
                response = new ApiResponse<object>(400, Constants.Messages.InvalidData, null);
                return Ok(response);
            }

            try
            {
                var employeeEntity = new Employee
                {
                    Name = addEmployeeDto.Name,
                    Department = addEmployeeDto.Department,
                    Position = addEmployeeDto.Position,
                    Status = EmployeeStatus.Active,
                    StatusChangeDate = DateTime.UtcNow
                };

                _dbContext.Employees.Add(employeeEntity);
                _dbContext.SaveChanges();

                response = new ApiResponse<object>(201, Constants.Messages.EmployeeCreated, employeeEntity);
            }
            catch (Exception)
            {
                response = new ApiResponse<object>(500, Constants.Messages.InternalServerError, null);
            }

            return Ok(response);
        }

        // PUT Req : To Update an Employee data by id
        [HttpPut("{id:int}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            ApiResponse<object> response;

            if (string.IsNullOrWhiteSpace(updateEmployeeDto.Name) ||
                string.IsNullOrWhiteSpace(updateEmployeeDto.Department) ||
                string.IsNullOrWhiteSpace(updateEmployeeDto.Position))
            {
                response = new ApiResponse<object>(400, Constants.Messages.InvalidData, null);
                return Ok(response);
            }

            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id && e.Status == EmployeeStatus.Active);
                if (employee == null)
                {
                    response = new ApiResponse<object>(404, Constants.Messages.EmployeeNotFound, null);
                }
                else
                {
                    employee.Name = updateEmployeeDto.Name;
                    employee.Department = updateEmployeeDto.Department;
                    employee.Position = updateEmployeeDto.Position;
                    employee.StatusChangeDate = DateTime.UtcNow;

                    _dbContext.SaveChanges();
                    response = new ApiResponse<object>(200, Constants.Messages.EmployeeUpdated, employee);
                }
            }
            catch (Exception)
            {
                response = new ApiResponse<object>(500, Constants.Messages.InternalServerError, null);
            }

            return Ok(response);
        }

        // DELETE Req : to soft delete the employee data
        [HttpDelete("{id:int}")]
        public IActionResult RemoveEmployee(int id)
        {
            ApiResponse<object> response;

            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id && e.Status == EmployeeStatus.Active);
                if (employee == null)
                {
                    response = new ApiResponse<object>(404, Constants.Messages.EmployeeNotFound, null);
                }
                else
                {
                    employee.Status = EmployeeStatus.Inactive;
                    employee.StatusChangeDate = DateTime.UtcNow;

                    _dbContext.SaveChanges();
                    response = new ApiResponse<object>(200, Constants.Messages.EmployeeDeleted, null);
                }
            }
            catch (Exception)
            {
                response = new ApiResponse<object>(500, Constants.Messages.InternalServerError, null);
            }

            return Ok(response);
        }
    }
}
