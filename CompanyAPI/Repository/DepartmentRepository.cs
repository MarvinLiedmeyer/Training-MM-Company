using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using Dapper;

namespace CompanyAPI.Repository
{
    public class DepartmentRepository : IBaseInterface<DepartmentDto, Department>
    {
        private readonly IDbContext _dbContext;

        private const string SqlCommSel = "SELECT [Department].Id, [Department].Name, Description, CompanyId, [Company].Name AS CompanyName FROM Department JOIN Company ON CompanyId = [Company].Id  WHERE [Department].DeleteTime IS NULL";
        private const string SqlCommSelId = "SELECT [Department].Id, [Department].Name, Description, [Company].Name FROM Department JOIN Company ON CompanyId = [Company].Id FROM Department WHERE Id = @id and [Department].DeleteTime IS NULL";
        private const string SqlCommDel = "UPDATE Department SET DeleteTime = GetDate() WHERE id = @id";
        private const string SqlCommAddOrUpdate = "spCreateDepartment";

        public DepartmentRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> Create(DepartmentDto data)
        {
            var newModel = new Department()
            {
                Name = data.Name,
                Description = data.Description,
                CompanyId = data.CompanyId
            };
            return CreateOrUpdate(newModel);
        }

        public async Task<List<Department>> Read()
        {
            List<Department> retval;
            using (var sqlConn = _dbContext.GetConnection())
            {
                retval = (await sqlConn.QueryAsync<Department>(SqlCommSel)).AsList();
            }
            return retval;
        }

        public async Task<DepartmentDto> ReadId(int id)
        {
            DepartmentDto retval;
            using (var sqlConn = _dbContext.GetConnection())
            {
                var param = new DynamicParameters();
                param.Add("@id", id);
                retval = await sqlConn.QueryFirstOrDefaultAsync<DepartmentDto>(SqlCommSelId, param);
            }
            return retval;
        }

        public Task<bool> Update(DepartmentDto model, int id)
        {
            var newModel = new Department()
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                CompanyId = model.CompanyId
            };
            return CreateOrUpdate(newModel);
        }

        public async Task<bool> Delete(int id)
        {
            bool retval;
            var query = SqlCommDel;
            var param = new DynamicParameters();
            param.Add("@id", id);
            using (var sqlConn = _dbContext.GetConnection())
            {
                var result = await sqlConn.ExecuteAsync(query, param);
                retval = (result == 1);
            }
            return retval;
        }
        private async Task<bool> CreateOrUpdate(Department model)
        {
            var query = SqlCommAddOrUpdate;
            Department retval;
            var param = new DynamicParameters();
            using (var sqlConn = _dbContext.GetConnection())
            {
                param.AddDynamicParams(
                    new { model.Id, model.Name, model.Description, model.CompanyId }
                    );
                retval = await sqlConn.QueryFirstOrDefaultAsync<Department>(query, param, commandType: CommandType.StoredProcedure);
            }
            return retval != null;
        }
    }
}
