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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CompanyAPI.Controller
{
    [Route("addresses")]
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

            //_logger.LogInformation($"hello from {Request.Headers["User-Agent"]}");
            var retval = await _addressRepository.Read();
            _logger.LogInformation("successful");
            return Ok(retval);

        }

        // GET api/values/5
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

        // POST api/values
        [HttpPost]
        [ChaynsAuth(uac: Uac.Manager, uacSiteId: "77893-11893")]
        public async Task<IActionResult> Post([FromBody] AddressDto address)
        {
            if (validateCreate(address))
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



        // PUT api/values/5
        [HttpPut("{id}")]
        [ChaynsAuth(uac: Uac.Manager, uacSiteId: "77893-11893")]
        public async Task<IActionResult> Put(int id, [FromBody] AddressDto address)
        {
            if (_addressRepository.ReadId(id) != null)
            {
                if (validateUpdate(address))
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ChaynsAuth(uac: Uac.Manager, uacSiteId: "77893-11893")]
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

        private bool validateCreate(AddressDto address)
        {
            if (address.Street != null && address.City != null && address.Zip != null && address.Country != null)
            {
                return true;
            }
            return false;
        }
        private bool validateUpdate(AddressDto address)
        {
            if (address.Street != null && address.City != null && address.Zip != null && address.Country != null)
            {
                return true;
            }
            return false;
        }
    }
}
