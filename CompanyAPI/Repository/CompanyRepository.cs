using System;
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
    public class CompanyRepository : IBaseInterface<CompanyDto, Company>
    {
        // Entity Framework zum Abfragen, Einfügen und Löschen von Daten
        private readonly IDbContext _dbContext;
        // -------------------------------------------------------------

        // SQL Abfrage -----------------------------------------------------------------------------------------------------------------
        private const string SqlCommSel = "SELECT Id, Name, FoundedDate FROM Company WHERE DeleteTime IS NULL";
        private const string SqlCommSelId = "SELECT Id, Name, FoundedDate from Company WHERE Id = @id and DeleteTime IS NULL";
        private const string SqlCommDel = "UPDATE company SET DeleteTime = GetDate() WHERE id = @id";
        private const string SqlCommAddOrUpdate = "spCreateCompany";
        // -----------------------------------------------------------------------------------------------------------------------------

        public CompanyRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> Create(CompanyDto data)
        {
            var newModel = new Company()
            {
                Name = data.Name,
                FoundedDate = data.FoundedDate
            };
            return CreateOrUpdate(newModel);
        }

        public async Task<List<Company>> Read()
        {
            List<Company> retval;
            try
            {

                // stellt die Verbindung zur Datenbank URL her ------
                using (var sqlConn = _dbContext.GetConnection())
                // --------------------------------------------------

                {
                    retval = (await sqlConn.QueryAsync<Company>(SqlCommSel)).AsList();
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
            CompanyDto retval;
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
                    retval = await sqlConn.QueryFirstOrDefaultAsync<CompanyDto>(SqlCommSelId, param);
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
            var newModel = new Company()
            {
                Id = id,
                Name = model.Name,
                FoundedDate = model.FoundedDate
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

        private async Task<bool> CreateOrUpdate(Company model)
        {
            if (model.FoundedDate != null)
            {
                var date = (DateTime)model.FoundedDate;
                var minDate = new DateTime(1753, 1, 1);
                if (DateTime.Compare(minDate, date) > 0)
                {
                    throw new RepoException(RepoResultType.WRONGPARAMETER);
                }
            }
            const string query = SqlCommAddOrUpdate;
            try
            {
                using (var sqlConn = _dbContext.GetConnection())
                {
                    var param = new DynamicParameters();
                    param.AddDynamicParams(
                        new { model.Id, model.Name, model.FoundedDate }
                        );
                    var retval = await sqlConn.QueryFirstOrDefaultAsync<Company>(query, param, commandType: CommandType.StoredProcedure);
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
