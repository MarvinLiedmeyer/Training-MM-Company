using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CompanyAPI.Helper;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using Dapper;

namespace CompanyAPI.Repository
{
    public class AddressRepository : IBaseInterface<AddressDto, Address>
    {
        private readonly IDbContext _dbContext;
        private const string SqlCommSel = "SELECT Id, Street, City, ZIP, Country FROM Address WHERE DeleteTime IS NULL";
        private const string SqlCommSelId = "SELECT Id, Street, City, ZIP, Country FROM Address WHERE Id = @id AND DeleteTime IS NULL";
        private const string SqlCommDel = "UPDATE company SET DeleteTime = GetDate() WHERE id = @id";
        private const string SqlCommAddOrUpdate = "spCreateAddress";

        public AddressRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> Create(AddressDto data)
        {
            var newModel = new Address()
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
            List<Address> retval;
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    retval = (await sqlConn.QueryAsync<Address>(SqlCommSel)).AsList();
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
            AddressDto retval;
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
                    retval = await sqlConn.QueryFirstOrDefaultAsync<AddressDto>(SqlCommSelId, param);
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
            var newModel = new Address()
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
        private async Task<bool> CreateOrUpdate(Address model)
        {
            const string query = SqlCommAddOrUpdate;
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    var param = new DynamicParameters();
                    param.AddDynamicParams(
                        new { model.Id, model.Street, model.City, model.Zip, model.Country }
                        );
                    var retval = await sqlConn.QueryFirstOrDefaultAsync<Address>(query, param, commandType: CommandType.StoredProcedure);
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
