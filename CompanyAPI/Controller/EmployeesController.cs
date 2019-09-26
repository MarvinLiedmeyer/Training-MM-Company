using System.Threading.Tasks;
using Chayns.Auth.ApiExtensions;
using Chayns.Auth.Shared.Constants;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CompanyAPI.Controller
{
    [Route("api/{locationID:int}/employees")]
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
            _tokenInfoProvider.GetUserPayload();
            var retval = await _employeeRepository.Read();
            _logger.LogInformation("successful");
            return Ok(retval);
        }

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

        [HttpPost]
        [ChaynsAuth(uac: Uac.Manager)]
        public async Task<IActionResult> Post([FromBody] EmployeeDto employee)
        {
            if (ValidateCreate(employee))
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

        [HttpPut("{id}")]
        [ChaynsAuth(uac: Uac.Manager)]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeeDto employee)
        {
            if (_employeeRepository.ReadId(id) != null)
            {
                if (ValidateUpdate(employee))
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

        [HttpDelete("{id}")]
        [ChaynsAuth(uac: Uac.Manager)]
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

        private static bool ValidateCreate(EmployeeDto employee)
        {
            return employee.FirstName != null && employee.LastName != null;
        }

        private static bool ValidateUpdate(EmployeeDto employee)
        {
            return employee.FirstName != null && employee.LastName != null;
        }
    }
}
