using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
    [Route("companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly IBaseInterface<CompanyDto, Company> _companyRepository;

        public CompaniesController(IBaseInterface<CompanyDto, Company> companyRepository, ILoggerFactory loggerFactory )
        {
            _logger = loggerFactory.CreateLogger<CompaniesController>();
            _companyRepository = companyRepository;
        }
        [HttpGet] 
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation($"hello from {Request.Headers["User-Agent"]}");
                var retval = _companyRepository.Read();
                return Ok(retval);
            }
            catch (RepoException repoEx)
            {

                switch (repoEx.ExType)
                {
                    case RepoResultType.SQLERROR:
                        _logger.LogError(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status503ServiceUnavailable);
                    case RepoResultType.NOTFOUND:
                        _logger.LogError(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status409Conflict);
                }
                _logger.LogInformation("SUCCES");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var retVal = _companyRepository.ReadId(id);
                if (_companyRepository.ReadId(id) == null)
                {
                    _logger.LogWarning("Bad Request");
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                _logger.LogInformation("Succesful");
                return StatusCode(StatusCodes.Status200OK, retVal);
            }catch(RepoException repoEx)
            {
                switch (repoEx.ExType)
                {
                    case RepoResultType.SQLERROR:
                        _logger.LogError(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status503ServiceUnavailable);
                    case RepoResultType.NOTFOUND:
                        _logger.LogWarning(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status409Conflict);
                    case RepoResultType.WRONGPARAMETER:
                        _logger.LogWarning(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status400BadRequest);
                }
                _logger.LogInformation("SUCCES");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] CompanyDto company)
        {
            try
            {
                if (validateCreate(company))
                {
                    var retVal = _companyRepository.Create(company.GetCompany());

                    if (retVal == false)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest);
                    }

                    return StatusCode(StatusCodes.Status201Created);

                }
                else

                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (RepoException repoEx)
            {

                switch (repoEx.ExType)
                {
                    case RepoResultType.SQLERROR:
                        _logger.LogError(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status503ServiceUnavailable);
                    case RepoResultType.NOTFOUND:
                        _logger.LogWarning(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status409Conflict);
                    case RepoResultType.WRONGPARAMETER:
                        _logger.LogWarning(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status400BadRequest);
                }
                _logger.LogInformation("SUCCES");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,  [FromBody] CompanyDto company)
        {
            if (_companyRepository.ReadId(id) != null)
            {
                if (validateUpdate(company))
                {
                    var retVal = _companyRepository.Update(company, id);

                    if (retVal == false)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest);
                    }
                    return NoContent();
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            _logger.LogInformation("SUCCES");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete( int id )
        {
            
            try
            {
                var retVal = _companyRepository.Delete(id);
                if (retVal)
                {
                    _logger.LogInformation("SUCCES");
                    return StatusCode(StatusCodes.Status204NoContent, $"Deleted {id}");
                }
                _logger.LogWarning("Bad Request");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (RepoException repoEx)
            {
                switch (repoEx.ExType)
                {
                    case RepoResultType.SQLERROR:
                        _logger.LogError(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status503ServiceUnavailable);
                    case RepoResultType.NOTFOUND:
                        _logger.LogWarning(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status409Conflict);
                    case RepoResultType.WRONGPARAMETER:
                        _logger.LogWarning(repoEx.InnerException, repoEx.Message);
                        return StatusCode(StatusCodes.Status400BadRequest);
                }
                _logger.LogInformation("SUCCES");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            
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
