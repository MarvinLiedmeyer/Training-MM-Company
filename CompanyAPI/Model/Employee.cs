using System;

namespace ConsoleApp.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BeginDate { get; set; }
        public int DepartmentId { get; internal set; }
        public int AddressId { get; internal set; }
    }
}
