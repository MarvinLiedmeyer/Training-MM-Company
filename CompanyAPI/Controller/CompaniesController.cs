using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly CompanyRepository repo = new CompanyRepository();
        // GET api/values
        [HttpGet] 
        public IActionResult Get()
        {
            var retval = repo.Read();
            return Ok(retval);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var retVal = repo.ReadId(id);
            if (repo.ReadId(id) == null)
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
            repo.Create(company.GetCompany());

            if (repo.Create(company.GetCompany()) == null)
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
        [HttpPut]
        public IActionResult Put([FromBody] Company company)
        {
            if (validateUpdate(company))
            {
                repo.Update(company);

                if (repo.Update(company) == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                return Ok(company.Id);
            }else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete( int id )
        {
            {
                var retVal = repo.Delete(id);
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
        private bool validateUpdate(Company company)
        {
            if (company.Name != null && company.FoundedDate != null)
            {
                return true;
            }
            return false;
        }
    }
}
