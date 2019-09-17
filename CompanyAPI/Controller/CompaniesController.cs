using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using ConsoleApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.Controller
{
    [Route("companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IBaseInterface<CompanyDto, Company> _companyRepository;

        public CompaniesController(IBaseInterface<CompanyDto, Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }
        [HttpGet] 
        public IActionResult Get()
        {
            var retval = _companyRepository.Read();
            return Ok(retval);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var retVal = _companyRepository.ReadId(id);
            if (_companyRepository.ReadId(id) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return StatusCode(StatusCodes.Status200OK, retVal);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] CompanyDto company)
        {
            if(validateCreate(company))
            {
                var retVal = _companyRepository.Create(company.GetCompany());

            if (retVal == false)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status201Created);

            } else

            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,  [FromBody] CompanyDto company)
        {
            if (validateUpdate(company))
            {
                var retVal =_companyRepository.Update(company, id);

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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete( int id )
        {
            {
                var retVal = _companyRepository.Delete(id);
                if (retVal)
                {
                    return StatusCode(StatusCodes.Status204NoContent, $"Delted {id}");
                }
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
