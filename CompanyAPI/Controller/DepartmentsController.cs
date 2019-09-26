using System.Threading.Tasks;
using Chayns.Auth.ApiExtensions;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CompanyAPI.Controller
{
    [Route("api/{locationID}/departments")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IBaseInterface<DepartmentDto, Department> _departmentRepository;

        public DepartmentsController(IBaseInterface<DepartmentDto, Department> departmentRepository, ILogger<DepartmentsController> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var retval = await _departmentRepository.Read();
            _logger.LogInformation("Successful");
            return Ok(retval);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var retVal = await _departmentRepository.ReadId(id);
            if (retVal == null)
            {
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            _logger.LogInformation("Successful");
            return StatusCode(StatusCodes.Status200OK, retVal);
        }

        [HttpPost]
        [ChaynsAuth]
        public async Task<IActionResult> Post([FromBody] DepartmentDto department)
        {
            if (ValidateCreate(department))
            {
                var retVal = await _departmentRepository.Create(department);
                if (!retVal)
                {
                    _logger.LogWarning("Bad Request");
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                _logger.LogInformation("Created");
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPut("{id}")]
        [ChaynsAuth]
        public async Task<IActionResult> Put(int id, [FromBody] DepartmentDto department)
        {
            if (_departmentRepository.ReadId(id) != null)
            {
                if (ValidateUpdate(department))
                {
                    var retVal = await _departmentRepository.Update(department, id);
                    if (retVal) return NoContent();
                    _logger.LogWarning("Bad Request");
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            _logger.LogWarning("Not Fount");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpDelete("{id}")]
        [ChaynsAuth]
        public async Task<IActionResult> Delete(int id)
        {
            var retVal = await _departmentRepository.Delete(id);
            if (retVal)
            {
                _logger.LogInformation("Delete");
                return StatusCode(StatusCodes.Status200OK, $"Deleted {id}");
            }
            _logger.LogWarning("Bad Request");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        private bool ValidateCreate(DepartmentDto department)
        {
            if (department.Name != null && department.Description != null)
            {
                _logger.LogInformation("Validate accepted");
                return true;
            }
            _logger.LogInformation("Validate unaccepted");
            return false;
        }

        private bool ValidateUpdate(DepartmentDto department)
        {
            if (department.Name != null && department.Description != null)
            {
                _logger.LogInformation("Validate accepted");
                return true;
            }
            _logger.LogInformation("Validate unaccepted");
            return false;
        }
    }
}
