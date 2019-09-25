using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Helper;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyAPI.Controller
{
        [Route("departments")]
        [ApiController]
        public class DepartmentsController : ControllerBase
        {
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IBaseInterface<DepartmentDto, Department> _departmentRepository;

            public DepartmentsController(IBaseInterface<DepartmentDto, Department> departmentRepository)
            {
                _departmentRepository = departmentRepository;
            }

            [HttpGet]
            public async Task<IActionResult> Get()
            {
                var retval = await _departmentRepository.Read();
                return Ok(retval);
            }

            // GET api/values/5
            [HttpGet("{id}")]
            public async Task<IActionResult> Get(int id)
            {
                var retVal = await _departmentRepository.ReadId(id);
                if (retVal == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                return StatusCode(StatusCodes.Status200OK, retVal);
            }

            // POST api/values
            [HttpPost]
            public async Task<IActionResult> Post([FromBody] DepartmentDto department)
            {
            var user = Authorization.GetUser(HttpContext);
            if (user.TobitUserID == 2062210)
            {
                if (validateCreate(department))
                {
                    var retVal = await _departmentRepository.Create(department);

                    if (!retVal)
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
            else
            {
                _logger.LogWarning("Unauthorisiert");
                return Unauthorized();
            }
           
            }



            // PUT api/values/5
            [HttpPut("{id}")]
            public async Task<IActionResult> Put(int id, [FromBody] DepartmentDto department)
            {
            var user = Authorization.GetUser(HttpContext);
            if (user.TobitUserID == 2062210)
            {
                if (_departmentRepository.ReadId(id) != null)
                {
                    if (validateUpdate(department))
                    {
                        var retVal = await _departmentRepository.Update(department, id);

                        if (!retVal)
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
                var retVal = await _departmentRepository.Delete(id);
                if (retVal)
                {
                    return StatusCode(StatusCodes.Status204NoContent, $"Delted {id}");
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                _logger.LogWarning("Unauthorisiert");
                return Unauthorized();
            }
            }

            private bool validateCreate(DepartmentDto department)
            {
                if (department.Name != null && department.Description != null  && department.CompanyId != null)
                {
                    return true;
                }
                return false;
            }
            private bool validateUpdate(DepartmentDto department)
            {
                if (department.Name != null && department.Description != null && department.CompanyId != null)
                {
                    return true;
                }
                return false;
            }
        }
}
