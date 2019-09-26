using System.Threading.Tasks;
using Chayns.Auth.ApiExtensions;
using Chayns.Auth.Shared.Constants;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CompanyAPI.Controller
{
    [Route("api/{locationID:int}/addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly ILogger<AddressesController> _logger;
        private readonly IBaseInterface<AddressDto, Address> _addressRepository;

        public AddressesController(IBaseInterface<AddressDto, Address> addressRepository, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AddressesController>();
            _addressRepository = addressRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var retval = await _addressRepository.Read();
            _logger.LogInformation("successful");
            return Ok(retval);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var retVal = await _addressRepository.ReadId(id);
            if (_addressRepository.ReadId(id) == null)
            {
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            _logger.LogInformation("successful");
            return StatusCode(StatusCodes.Status200OK, retVal);

        }
        
        [HttpPost]
        [ChaynsAuth(uac: Uac.Manager)]
        public async Task<IActionResult> Post([FromBody] AddressDto address)
        {
            if (ValidateCreate(address))
            {
                var retVal = await _addressRepository.Create(address);
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
        public async Task<IActionResult> Put(int id, [FromBody] AddressDto address)
        {
            if (_addressRepository.ReadId(id) != null)
            {
                if (ValidateUpdate(address))
                {
                    var retVal = await _addressRepository.Update(address, id);
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
            var retVal = await _addressRepository.Delete(id);
            if (retVal)
            {
                _logger.LogInformation("successful");
                return StatusCode(StatusCodes.Status200OK, $"Deleted {id}");
            }
            _logger.LogWarning("Bad Request");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        private bool ValidateCreate(AddressDto address)
        {
            if (address.Street != null && address.City != null && address.Zip != null && address.Country != null)
            {
                _logger.LogInformation("Validate accepted");
                return true;
            }
            _logger.LogInformation("Validate unaccepted");
            return false;
        }

        private bool ValidateUpdate(AddressDto address)
        {
            if (address.Street != null && address.City != null && address.Zip != null && address.Country != null)
            {
                _logger.LogInformation("Validate accepted");
                return true;
            }
            _logger.LogInformation("Validate unaccepted");
            return false;
        }
    }
}
