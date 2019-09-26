using System.Threading.Tasks;
using Chayns.Auth.ApiExtensions;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CompanyAPI.Controller
{
    [Route("api/{locationID}/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly IBaseInterface<CompanyDto, Company> _companyRepository;

        public CompaniesController(IBaseInterface<CompanyDto, Company> companyRepository, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CompaniesController>();
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var retval = await _companyRepository.Read();
            _logger.LogInformation("successful");
            return Ok(retval);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var retVal = await _companyRepository.ReadId(id);
            if (_companyRepository.ReadId(id) == null)
            {
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            _logger.LogInformation("Successful");
            return StatusCode(StatusCodes.Status200OK, retVal);
        }

        [HttpPost]
        [ChaynsAuth]
        public async Task<IActionResult> Post([FromBody] CompanyDto company)
        {
            if (ValidateCreate(company))
            {
                var retVal = await _companyRepository.Create(company.GetCompany());
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
        [ChaynsAuth]
        public async Task<IActionResult> Put(int id, [FromBody] CompanyDto company)
        {
            if (_companyRepository.ReadId(id) != null)
            {
                if (ValidateUpdate(company))
                {
                    var retVal = await _companyRepository.Update(company, id);
                    if (retVal == false)
                    {
                        _logger.LogWarning("Bad Request");
                        return StatusCode(StatusCodes.Status400BadRequest);
                    }
                    _logger.LogInformation("Successful");
                    return Ok();
                }
                else
                {
                    _logger.LogWarning("Bad Request");
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            _logger.LogInformation("Not Found");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpDelete("{id}")]
        [ChaynsAuth]
        public async Task<IActionResult> Delete(int id)
        {
            var retVal = await _companyRepository.Delete(id);
            if (retVal)
            {
                _logger.LogInformation("successful");
                return StatusCode(StatusCodes.Status200OK, $"Deleted {id}");
            }
            _logger.LogWarning("Bad Request");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        private bool ValidateCreate(CompanyDto company)
        {
            if (company.Name != null && company.FoundedDate != null)
            {
                _logger.LogInformation("Validate accepted");
                return true;
            }
            _logger.LogInformation("Validate unaccepted");
            return false;
        }

        private bool ValidateUpdate(CompanyDto company)
        {
            if (company.Name != null && company.FoundedDate != null)
            {
                _logger.LogInformation("Validate accepted");
                return true;
            }
            _logger.LogInformation("Validate unaccepted");
            return false;
        }
    }
}
