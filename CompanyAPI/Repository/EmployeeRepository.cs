using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ConsoleApp.Model;
using Dapper;
using CompanyAPI.Model;
using CompanyAPI.Interface;
using System.Data;
using CompanyAPI.Helper;
using System.Threading.Tasks;

namespace ConsoleApp.Repository
{
    public class EmployeeRepository : CompanyAPI.Interface.IBaseInterface<EmployeeDto, Employee>
    {

        private readonly IDbContext _dbContext;
        string sqlCommSel = "SELECT Id, FirstName, LastName, BeginDate, DepartmentId, AddressId FROM employee WHERE DeleteTime IS NULL";
        string sqlCommSelId = "SELECT Id,  FirstName, LastName, BeginDate, DepartmentId, AddressId FROM employee WHERE Id = @id and DeleteTime IS NULL";
        string sqlCommDel = "UPDATE employee SET DeleteTime = GetDate() WHERE id = @id";
        string employeeReadIdCmd = $"SELECT id,  FirstName, LastName, BeginDate, DepartmentId, AddressId FROM employee WHERE id = @id";
        string sqlCommAddOrUpdate = "spCreateEmployee";

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
            List<Employee> retval = new List<Employee>();
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    retval = (await sqlConn.QueryAsync<Employee>(sqlCommSel)).AsList();
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
            EmployeeDto retval = new EmployeeDto();
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
                    retval = await sqlConn.QueryFirstOrDefaultAsync<EmployeeDto>(sqlCommSelId, param);
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
            Employee newModel = new Employee()
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
            bool retval = false;
            var query = sqlCommDel;
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
        private async Task<bool> CreateOrUpdate(Employee model)
        {
            if (model.BeginDate != null)
            {
                var date = (DateTime)model.BeginDate;
                var minDate = new DateTime(1753, 1, 1);
                if (DateTime.Compare(minDate, date) > 0)
                {
                    throw new RepoException(RepoResultType.WRONGPARAMETER);
                }
            }
            var query = sqlCommAddOrUpdate;
            Employee retval;
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.AddDynamicParams(
                        new { model.Id, model.FirstName, model.LastName, model.BeginDate, model.DepartmentId, model.AddressId }
                        );
                    retval = await sqlConn.QueryFirstOrDefaultAsync<Employee>(query, param, commandType: CommandType.StoredProcedure);
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
            return retval != null;
        }
    }
}
