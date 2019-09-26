using System;

namespace CompanyAPI.Model
{
    public class EmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BeginDate { get; set; }
        public int DepartmentId { get; set; }
        public int AddressId { get; set; }
    }
}
