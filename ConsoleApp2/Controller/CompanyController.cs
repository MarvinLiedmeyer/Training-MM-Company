using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ConsoleApp.Model;
using ConsoleApp.Interface;
using ConsoleApp.Repository;

namespace CompanyApp.Controller {
    class CompanyController {
        string connectionString = "";
        string companyReadCmd = "SELECT Id, Name, FoundedDate from Company WHERE DeleteTime IS NULL";
        private readonly IBaseInterface<Company> companyRepository;
        public CompanyController(string connectionString) {
            this.connectionString = connectionString;
            companyRepository = new CompanyRepository(connectionString);
        }

        public Company Create(Company data) {
            return companyRepository.Create(data);
        }

        public List<Company> Read() {
            return companyRepository.Read();
        }

        public Company Update(Company data) {
            return companyRepository.Update(data);
        }

        public bool Delete(int id) {
            return companyRepository.Delete(id);
        }
    }
}
