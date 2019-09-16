using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using ConsoleApp.Model;
using Dapper;
using CompanyAPI.Model;
using CompanyAPI.Interface;
using System.Data;

namespace ConsoleApp.Repository
{
    public class CompanyRepository : CompanyAPI.Interface.IBaseInterface<CompanyDto, Company>
    {

        private readonly IDbContext _dbContext;

        string sqlCommSel = "select Id, Name, FoundedDate from Company where DeleteTime is null";
        string sqlCommSelId = "select Id, Name, FoundedDate from Company where Id = @id and DeleteTime is null";
        string sqlCommDel = "update company set DeleteTime = GetDate() where id = @id";
        string companyReadIdCmd = $"SELECT id, name, foundeddate FROM company WHERE id = @id";
        string sqlCommAddOrUpdate = "spCreateCompany";

        public CompanyRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(CompanyDto data)
        {
            Company newModel = new Company()
            {
                Name = data.Name,
                FoundedDate = data.FoundedDate
            };
            return CreateOrUpdate(newModel);
        }

        public List<Company> Read()
        {
            List<Company> retval = new List<Company>();
            using (var sqlConn = _dbContext.GetConnection())
            {
                retval = sqlConn.Query<Company>(sqlCommSel).AsList();
            }
            return retval;
        }
        public CompanyDto ReadId(int id)
        {
            CompanyDto retval = new CompanyDto();
            using (var sqlConn = _dbContext.GetConnection())
            {
                var param = new DynamicParameters();
                param.Add("@id", id);
                retval = sqlConn.QueryFirstOrDefault<CompanyDto>(sqlCommSelId, param);
            }
            return retval;
        }

        public bool Update(CompanyDto model, int id)
        {
            Company newModel = new Company()
            {
                Id = id,
                Name = model.Name,
                FoundedDate = model.FoundedDate
            };
            return CreateOrUpdate(newModel);
        }

        public bool Delete(int id)
        {
            bool retval = false;
            var query = sqlCommDel;
            var param = new DynamicParameters();
            param.Add("@id", id);
            using (var sqlConn = _dbContext.GetConnection())
            {
                var result = sqlConn.Execute(query, param);
                retval = (result == 1);
            }
            return retval;
        }
        private bool CreateOrUpdate(Company model)
        {
            var query = sqlCommAddOrUpdate;
            Company retval;
            using (var sqlConn = _dbContext.GetConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.AddDynamicParams(
                    new { model.Id, model.Name, model.FoundedDate }
                    );

                retval = sqlConn.QueryFirstOrDefault<Company>(query, param, commandType: CommandType.StoredProcedure);
            }
            return retval != null;
        }
    }
}
