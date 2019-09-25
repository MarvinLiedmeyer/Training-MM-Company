using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CompanyAPI.Helper;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using ConsoleApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CompanyAPI.Controller
{
    [Route("employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IBaseInterface<EmployeeDto, Employee> _employeeRepository;

        public EmployeesController(IBaseInterface<EmployeeDto, Employee> employeeRepository, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EmployeesController>();
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            //_logger.LogInformation($"hello from {Request.Headers["User-Agent"]}");
            var retval = await _employeeRepository.Read();
            _logger.LogInformation("successful");
            return Ok(retval);

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            var retVal = await _employeeRepository.ReadId(id);
            if (_employeeRepository.ReadId(id) == null)
            {
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            _logger.LogInformation("successful");
            return StatusCode(StatusCodes.Status200OK, retVal);

        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeDto employee)
        {
            var user = Authorization.GetUser(HttpContext);

            if (validateCreate(employee))
            {
                var retVal = await _employeeRepository.Create(employee);

                if (retVal == false)
                {
                    _logger.LogWarning("Bad Request");
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                _logger.LogInformation("successful");
                return StatusCode(StatusCodes.Status201Created);

            }
            else

            {
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }




        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeeDto employee)
        {
            var user = Authorization.GetUser(HttpContext);
            if (user.TobitUserID == 2062210)
            {
                if (_employeeRepository.ReadId(id) != null)
                {
                    if (validateUpdate(employee))
                    {
                        var retVal = await _employeeRepository.Update(employee, id);

                        if (retVal == false)
                        {
                            _logger.LogWarning("Bad Request");
                            return StatusCode(StatusCodes.Status400BadRequest);
                        }
                        _logger.LogInformation("OK");
                        return Ok();
                    }
                    else
                    {
                        _logger.LogWarning("Bad Request");
                        return StatusCode(StatusCodes.Status400BadRequest);
                    }
                }
                _logger.LogInformation("successful");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                _logger.LogWarning("Unauthorisiert");
                return Unauthorized();
            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = Authorization.GetUser(HttpContext);
            if (user.TobitUserID == 2062210)
            {
                var retVal = await _employeeRepository.Delete(id);
                if (retVal)
                {
                    _logger.LogInformation("successful");
                    return StatusCode(StatusCodes.Status200OK, $"Deleted {id}");
                }
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                _logger.LogWarning("Unauthorisiert");
                return Unauthorized();
            }


        }

        private bool validateCreate(EmployeeDto employee)
        {
            if (employee.FirstName != null && employee.LastName != null && employee.BeginDate != null && employee.DepartmentId != null && employee.AddressId != null)
            {
                return true;
            }
            return false;
        }
        private bool validateUpdate(EmployeeDto employee)
        {
            if (employee.FirstName != null && employee.LastName != null && employee.BeginDate != null && employee.DepartmentId != null && employee.AddressId != null)
            {
                return true;
            }
            return false;
        }
    }
}
