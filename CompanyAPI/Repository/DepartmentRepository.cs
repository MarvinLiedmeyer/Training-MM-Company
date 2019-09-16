﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Interface;
using CompanyAPI.Model;
using ConsoleApp.Model;
using Dapper;

namespace CompanyAPI.Repository
{
    public class DepartmentRepository : CompanyAPI.Interface.IBaseInterface<DepartmentDto, Department>
    {
        private readonly IDbContext _dbContext;

        string sqlCommSel = "select Id, Name, Description, CompanyId from Department where DeleteTime is null";
        string sqlCommSelId = "select Id, Name, Description, CompanyId from Department where Id = @id and DeleteTime is null";
        string sqlCommDel = "update Department set DeleteTime = GetDate() where id = @id";
        string companyReadIdCmd = $"SELECT id, name, Description, CompanyId from Department WHERE id = @id";
        string sqlCommAddOrUpdate = "spCreateDepartment";

        public DepartmentRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(DepartmentDto data)
        {
            Department newModel = new Department()
            {
                Name = data.Name,
                Description = data.Description,
                CompanyId = data.CompanyId
            };
            return CreateOrUpdate(newModel);
        }

        public List<Department> Read()
        {
            List<Department> retval = new List<Department>();
            using (var sqlConn = _dbContext.GetConnection())
            {
                retval = sqlConn.Query<Department>(sqlCommSel).AsList();
            }
            return retval;
        }

        public DepartmentDto ReadId(int id)
        {
            DepartmentDto retval = new DepartmentDto();
            using (var sqlConn = _dbContext.GetConnection())
            {
                var param = new DynamicParameters();
                param.Add("@id", id);
                retval = sqlConn.QueryFirstOrDefault<DepartmentDto>(sqlCommSelId, param);
            }
            return retval;
        }

        public bool Update(DepartmentDto model, int id)
        {
            Department newModel = new Department()
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                CompanyId = model.CompanyId
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
        private bool CreateOrUpdate(Department model)
        {
            var query = sqlCommAddOrUpdate;
            Department retval;
            using (var sqlConn = _dbContext.GetConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.AddDynamicParams(
                    new { model.Id, model.Name, model.Description, model.CompanyId }
                    );

                retval = sqlConn.QueryFirstOrDefault<Department>(query, param, commandType: CommandType.StoredProcedure);
            }
            return retval != null;
        }
    }
}
