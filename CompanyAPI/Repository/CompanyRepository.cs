using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using ConsoleApp.Model;
using Dapper;
using CompanyAPI.Model;
using CompanyAPI.Interface;
using System.Data;
using CompanyAPI.Helper;
using System.Threading.Tasks;

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

        public Task<bool> Create(CompanyDto data)
        {
            Company newModel = new Company()
            {
                Name = data.Name,
                FoundedDate = data.FoundedDate
            };
            return CreateOrUpdate(newModel);
        }

        public async  Task<List<Company>> Read()
        {
            List<Company> retval = new List<Company>();
            try
            {
                using (var sqlConn =  _dbContext.GetConnection())
                {
                    retval = (await sqlConn.QueryAsync<Company>(sqlCommSel)).AsList();
                    if (retval == null)
                    {
                        throw new RepoException(RepoResultType.NOTFOUND);
                    }


                }
            }
            catch (SqlException ex)
            {

                throw new RepoException("Sql Error occured.", ex, RepoResultType.SQLERROR);
            }
            return retval;
        }
        public async Task<CompanyDto> ReadId(int id)
        {
            CompanyDto retval = new CompanyDto();
            if (id < 1)
            {
                throw new RepoException(RepoResultType.WRONGPARAMETER);
            }
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);
                    retval = await sqlConn.QueryFirstOrDefaultAsync<CompanyDto>(sqlCommSelId, param);
                    if (retval == null)
                    {
                        throw new RepoException(RepoResultType.NOTFOUND);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new RepoException("Sql Error occured.", ex, RepoResultType.SQLERROR); 
            }
            return retval;
        }

        public Task<bool> Update(CompanyDto model, int id)
        {
            if (id < 1)
            {
                throw new RepoException(RepoResultType.WRONGPARAMETER);
            }
            Company newModel = new Company()
            {
                Id = id,
                Name = model.Name,
                FoundedDate = model.FoundedDate
            };
            return CreateOrUpdate(newModel);
        }

        public async Task<bool> Delete(int id)
        {
            bool retval = false;
            var query = sqlCommDel;
            var param = new DynamicParameters();
            param.Add("@id", id);
            if ( id < 1)
            {
                throw new RepoException(RepoResultType.WRONGPARAMETER);
            }
            
            try
            {
                using (var sqlConn =  _dbContext.GetConnection())
                {
                    var result = await sqlConn.ExecuteAsync(query, param);
                    retval = (result == 1);
                    if (!retval)
                    {
                        throw new RepoException(RepoResultType.NOTFOUND);
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new RepoException("SQL-ERROR occured", ex, RepoResultType.SQLERROR);
            }
            return retval;
        }
        private async Task<bool> CreateOrUpdate(Company model)
        {
            var query = sqlCommAddOrUpdate;
            Company retval;
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.AddDynamicParams(
                        new { model.Id, model.Name, model.FoundedDate }
                        );

                    retval = await sqlConn.QueryFirstOrDefaultAsync<Company>(query, param, commandType: CommandType.StoredProcedure);
                    if (retval== null)
                    {
                        throw new RepoException(RepoResultType.NOTFOUND);
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new RepoException("SQL-ERROR occured", ex, RepoResultType.SQLERROR);
            }
            return retval != null;
        }
    }
}
