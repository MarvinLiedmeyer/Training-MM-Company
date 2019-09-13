﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Model;

namespace CompanyAPI.Model
{
    public class CompanyDto
    {
        public string Name { get; set; }
        public DateTime? FoundedDate { get; set; }

        public Company GetCompany() => new Company
        {
            Name = this.Name,
            FoundedDate = this.FoundedDate
        };
    }
}