using EmployeeMgtApi.Data;
using EmployeeMgtApi.Models;
using EmployeeMgtApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
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


        //GET Req : To Fetching all employees
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = _dbContext.Employees.ToList();
            return Ok(allEmployees);
        }

        //GET Req : To Fetching epmloyee by id
        [HttpGet("{id:int}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        //POST Req: To Add the Employee
        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Department = addEmployeeDto.Department,
                Position = addEmployeeDto.Position,
            };
            _dbContext.Employees.Add(employeeEntity);
            _dbContext.SaveChanges();

            return Ok(employeeEntity);
        }

        //PUT Req : To Update an Employee data by id
        [HttpPut("{id:int}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Name = updateEmployeeDto.Name;
            employee.Department = updateEmployeeDto.Department;
            employee.Position = updateEmployeeDto.Position;

            _dbContext.SaveChanges();
            return Ok(employee);
        }

        //DELETE Req : to Delete the employee data
        [HttpDelete("{id:int}")]
        public IActionResult RemoveEmployee(int id)
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
