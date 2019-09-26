using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CompanyAPI.Helper;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using Dapper;

namespace CompanyAPI.Repository
{
    public class EmployeeRepository : IBaseInterface<EmployeeDto, Employee>
    {

        private readonly IDbContext _dbContext;

        private const string SqlCommSel = "SELECT Id, FirstName, LastName, BeginDate, DepartmentId, AddressId FROM employee WHERE DeleteTime IS NULL";
        private const string SqlCommSelId = "SELECT Id,  FirstName, LastName, BeginDate, DepartmentId, AddressId FROM employee WHERE Id = @id and DeleteTime IS NULL";
        private const string SqlCommDel = "UPDATE employee SET DeleteTime = GetDate() WHERE id = @id";
        private const string SqlCommAddOrUpdate = "spCreateEmployee";

        public EmployeeRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> Create(EmployeeDto data)
        {
            Employee newModel = new Employee()
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                BeginDate = data.BeginDate,
                DepartmentId = data.DepartmentId,
                AddressId = data.AddressId
            };
            return CreateOrUpdate(newModel);
        }

        public async Task<List<Employee>> Read()
        {
            List<Employee> retval;
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    retval = (await sqlConn.QueryAsync<Employee>(SqlCommSel)).AsList();
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

        public async Task<EmployeeDto> ReadId(int id)
        {
            EmployeeDto retval;
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
                    retval = await sqlConn.QueryFirstOrDefaultAsync<EmployeeDto>(SqlCommSelId, param);
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

        public Task<bool> Update(EmployeeDto data, int id)
        {
            if (id < 1)
            {
                throw new RepoException(RepoResultType.WRONGPARAMETER);
            }
            var newModel = new Employee()
            {
                Id = id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                BeginDate = data.BeginDate,
                DepartmentId = data.DepartmentId,
                AddressId = data.AddressId
            };
            return CreateOrUpdate(newModel);
        }

        public async Task<bool> Delete(int id)
        {
            const string query = SqlCommDel;
            var param = new DynamicParameters();
            param.Add("@id", id);
            if (id < 1)
            {
                throw new RepoException(RepoResultType.WRONGPARAMETER);
            }
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    var result = await sqlConn.ExecuteAsync(query, param);
                    var retval = (result == 1);
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
            return true;
        }
        private async Task<bool> CreateOrUpdate(Employee model)
        {
            var date = model.BeginDate;
            var minDate = new DateTime(1753, 1, 1);
            if (DateTime.Compare(minDate, date) > 0)
            {
                throw new RepoException(RepoResultType.WRONGPARAMETER);
            }

            var query = SqlCommAddOrUpdate;
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    var param = new DynamicParameters();
                    param.AddDynamicParams(
                        new { model.Id, model.FirstName, model.LastName, model.BeginDate, model.DepartmentId, model.AddressId }
                        );
                    var retval = await sqlConn.QueryFirstOrDefaultAsync<Employee>(query, param, commandType: CommandType.StoredProcedure);
                    if (retval == null)
                    {
                        throw new RepoException(RepoResultType.NOTFOUND);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new RepoException("SQL-ERROR occured", ex, RepoResultType.SQLERROR);
            }
            return true;
        }
    }
}
