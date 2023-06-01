using EFCore.Generic.Repository;
using EFCoreUtility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.API.Using.CommonORMLibrary.Data;
using Web.API.Using.CommonORMLibrary.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.API.Using.CommonORMLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IRepository<EmployeeDbContext> _emprepository;

        public EmployeeController(IRepository repository, IRepository<EmployeeDbContext> emprepository)
        {
            _emprepository = emprepository;
            _repository = repository;
        }

       
        // GET: Employee/Details/5
        [HttpGet("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = await _repository.GetByIdAsync<Employee>(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST api/<EmployeeController>
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
              
                await _repository.AddAsync<Employee>(employee);
               
                var result =  await _repository.SaveChangesAsync();
                return Ok(result);
            }
            return BadRequest("Model not valid");
        }

        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> Update(long id, Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Employee employeeToBeUpdated = await _repository.GetByIdAsync<Employee>(employee.EmployeeId);
                employeeToBeUpdated.FullName = employee.FullName;
                employeeToBeUpdated.Salary = employee.Salary;
                employeeToBeUpdated.Email = employee.Email;

                _repository.Update(employeeToBeUpdated);
                var res = await _repository.SaveChangesAsync();

                return Ok(res);
            }
            return BadRequest("Not Updated");
        }

        [HttpPost("DeleteEmployee")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            Employee employee = await _repository.GetByIdAsync<Employee>(id);
            _repository.Remove(employee);
            var result = await _repository.SaveChangesAsync();
            return Ok(result);
        }
    }
}
