using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPI.Model
{
    public class DepartmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }

        public DepartmentDto GetDepartment() => new DepartmentDto
        {
            Name = this.Name,
            Description = this.Description,
            CompanyId = this.CompanyId
        };
    }
}
