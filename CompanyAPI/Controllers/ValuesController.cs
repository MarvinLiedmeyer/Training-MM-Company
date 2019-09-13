using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Model;
using ConsoleApp.Model;
using ConsoleApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.Controllers
{
    [Route("companies")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        const string CONNECTION_STRING = "Data Source=tappqa;Initial Catalog=Training-MM-Company;Integrated Security=True";
        private readonly CompanyRepository repo = new CompanyRepository(CONNECTION_STRING);
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
            var retval = repo.Read();
            var retvalone = retval.Find((e) => e.Id == id);

            if (retvalone == null)
                return NotFound(retvalone);
            return Ok(retvalone);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] CompanyDto company)
        {
            repo.Create(company.GetCompany());
            
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/values/5
        [HttpPut]
        public IActionResult Put([FromBody] Company company)
        {
            repo.Update(company);
            return Ok(company.Id);
        }

        // DELETE api/values/5
        [HttpDelete]
        public IActionResult Delete([FromBody] int id )
        {
            repo.Delete(id);

            return Ok(id);
        }
    }
}
