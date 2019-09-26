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
    public class AddressRepository : CompanyAPI.Interface.IBaseInterface<AddressDto, Address>
    {
        private readonly IDbContext _dbContext;
        string sqlCommSel = "SELECT Id, Street, City, ZIP, Country FROM Address WHERE DeleteTime IS NULL";
        string sqlCommSelId = "SELECT Id, Street, City, ZIP, Country FROM Address WHERE Id = @id AND DeleteTime IS NULL";
        string sqlCommDel = "UPDATE company SET DeleteTime = GetDate() WHERE id = @id";
        string companyReadIdCmd = $"SELECT id, Street, City, ZIP, Country FROM Address WHERE id = @id";
        string sqlCommAddOrUpdate = "spCreateAddress";

        public AddressRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> Create(AddressDto data)
        {
            Address newModel = new Address()
            {
                Street = data.Street,
                City = data.City,
                Zip = data.Zip,
                Country = data.Country
            };
            return CreateOrUpdate(newModel);
        }

        public async Task<List<Address>> Read()
        {
            List<Address> retval = new List<Address>();
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    retval = (await sqlConn.QueryAsync<Address>(sqlCommSel)).AsList();
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

        public async Task<AddressDto> ReadId(int id)
        {
            AddressDto retval = new AddressDto();
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
                    retval = await sqlConn.QueryFirstOrDefaultAsync<AddressDto>(sqlCommSelId, param);
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

        public Task<bool> Update(AddressDto model, int id)
        {
            if (id < 1)
            {
                throw new RepoException(RepoResultType.WRONGPARAMETER);
            }
            Address newModel = new Address()
            {
                Street = model.Street,
                City = model.City,
                Zip = model.Zip,
                Country = model.Country
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
        private async Task<bool> CreateOrUpdate(Address model)
        {
            var query = sqlCommAddOrUpdate;
            Address retval;
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.AddDynamicParams(
                        new { model.Id, model.Street, model.City, model.Zip, model.Country }
                        );
                    retval = await sqlConn.QueryFirstOrDefaultAsync<Address>(query, param, commandType: CommandType.StoredProcedure);
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
