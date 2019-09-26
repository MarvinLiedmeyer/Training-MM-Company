using System;

namespace CompanyAPI.Model
{
    public class CompanyDto
    {
        public string Name { get; set; }
        public DateTime? FoundedDate { get; set; }

        public CompanyDto GetCompany() => new CompanyDto
        {
            Name = this.Name,
            FoundedDate = this.FoundedDate
        };
    }
}
