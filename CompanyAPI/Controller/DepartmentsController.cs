using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.Controller
{
        [Route("departments")]
        [ApiController]
        public class DepartmentsController : ControllerBase
        {
            private readonly IBaseInterface<DepartmentDto, Department> _departmentRepository;

            public DepartmentsController(IBaseInterface<DepartmentDto, Department> departmentRepository)
            {
                _departmentRepository = departmentRepository;
            }

            [HttpGet]
            public IActionResult Get()
            {
                var retval = _departmentRepository.Read();
                return Ok(retval);
            }

            // GET api/values/5
            [HttpGet("{id}")]
            public IActionResult Get(int id)
            {
                var retVal = _departmentRepository.ReadId(id);
                if (_departmentRepository.ReadId(id) == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                return StatusCode(StatusCodes.Status200OK, retVal);
            }

            // POST api/values
            [HttpPost]
            public IActionResult Post([FromBody] DepartmentDto department)
            {
                if (validateCreate(department))
                {
                 var retVal =_departmentRepository.Create(department.GetDepartment());

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



            // PUT api/values/5
            [HttpPut("{id}")]
            public IActionResult Put(int id, [FromBody] DepartmentDto department)
            {
                if (validateUpdate(department))
                {
                var retVal =_departmentRepository.Update(department, id);

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
            public IActionResult Delete(int id)
            {
                {
                    var retVal = _departmentRepository.Delete(id);
                    if (retVal)
                    {
                        return StatusCode(StatusCodes.Status204NoContent, $"Delted {id}");
                    }
                    return StatusCode(StatusCodes.Status400BadRequest);
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
