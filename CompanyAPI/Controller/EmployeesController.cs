using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Chayns.Auth.ApiExtensions;
using Chayns.Auth.Shared.Constants;
using CompanyAPI.Helper;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using ConsoleApp.Repository;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITokenInfoProvider _tokenInfoProvider;

        public EmployeesController(IBaseInterface<EmployeeDto, Employee> employeeRepository, ILoggerFactory loggerFactory, ITokenInfoProvider tokenInfoProvider)
        {
            _logger = loggerFactory.CreateLogger<EmployeesController>();
            _employeeRepository = employeeRepository;
            _tokenInfoProvider = tokenInfoProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = _tokenInfoProvider.GetUserPayload();

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
        [ChaynsAuth(uac: Uac.Manager, uacSiteId: "77893-11893")]
        public async Task<IActionResult> Post([FromBody] EmployeeDto employee)
        {
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
        [ChaynsAuth(uac: Uac.Manager, uacSiteId: "77893-11893")]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeeDto employee)
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ChaynsAuth(uac: Uac.Manager, uacSiteId: "77893-11893")]
        public async Task<IActionResult> Delete(int id)
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
