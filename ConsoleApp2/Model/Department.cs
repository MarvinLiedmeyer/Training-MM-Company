using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Model
{
    class Department
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; internal set; }
    }
}
