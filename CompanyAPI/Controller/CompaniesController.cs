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
    [Route("companies")]
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

            //_logger.LogInformation($"hello from {Request.Headers["User-Agent"]}");
            var retval = await _companyRepository.Read();
            _logger.LogInformation("successful");
            return Ok(retval);

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            var retVal = await _companyRepository.ReadId(id);
            if (_companyRepository.ReadId(id) == null)
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
        public async Task<IActionResult> Post([FromBody] CompanyDto company)
        {
            if (validateCreate(company))
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





        // PUT api/values/5
        [HttpPut("{id}")]
        [ChaynsAuth(uac: Uac.Manager, uacSiteId: "77893-11893")]
        public async Task<IActionResult> Put(int id, [FromBody] CompanyDto company)
        {
            if (_companyRepository.ReadId(id) != null)
            {
                if (validateUpdate(company))
                {
                    var retVal = await _companyRepository.Update(company, id);

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
            var retVal = await _companyRepository.Delete(id);
            if (retVal)
            {
                _logger.LogInformation("successful");
                return StatusCode(StatusCodes.Status200OK, $"Deleted {id}");
            }
            _logger.LogWarning("Bad Request");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        private bool validateCreate(CompanyDto company)
        {
            if (company.Name != null && company.FoundedDate != null)
            {
                return true;
            }
            return false;
        }
        private bool validateUpdate(CompanyDto company)
        {
            if (company.Name != null && company.FoundedDate != null)
            {
                return true;
            }
            return false;
        }
    }
}
