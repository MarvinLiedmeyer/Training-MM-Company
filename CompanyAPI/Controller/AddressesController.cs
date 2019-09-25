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
        public async Task<IActionResult> Post([FromBody] AddressDto address)
        {
            var user = Authorization.GetUser(HttpContext);

            if (user.TobitUserID == 2062210)
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
            else
            {
                _logger.LogWarning("Unauthorisiert");
                return Unauthorized();
            }


        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AddressDto address)
        {
            var user = Authorization.GetUser(HttpContext);
            if (user.TobitUserID == 2062210)
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
                var retVal = await _addressRepository.Delete(id);
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
