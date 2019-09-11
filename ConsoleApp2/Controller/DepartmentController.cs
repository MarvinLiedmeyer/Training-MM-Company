using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;

namespace ConsoleApp.Controller
{
    class DepartmentController
    {
        string dbSConStr = "";
        string comReadCmd = "SELECT Id, Name, Description, CompanyId FROM Department WHERE DeleteTime IS NULL";
        public DepartmentController(string ConnectionString)
        {
            dbSConStr = ConnectionString;
        }

        public bool Create(string Name, string Description, int CompanyId)
        {
            bool retval = false;

            var storedProcedure = "spCreateDepartment";
            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, sqlcon))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                    sqlcon.Open();

                    var result = cmd.ExecuteNonQuery();
                    retval = (result == 1);

                }
            }

            return retval;
        }

        public List<Model.Department> ReadDapper()
        {
            List<Model.Department> retval = new List<Model.Department>();

            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                retval = sqlcon.Query<Model.Department>(comReadCmd).AsList();
            }

            return retval;
        }

        public bool Update(int id, string name, string description, int CompanyId)
        {
            bool retval = false;
            var storedProcedure = "spCreateDepartment";
            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, sqlcon))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcon.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                    var result = cmd.ExecuteNonQuery();
                    retval = (result == 1);

                }
            }

            return retval;

        }

        public bool Delete(int id = 0)
        {
            bool retval = false;
            var query = "update department set DeleteTime = GetDate() where id = @Id";

            using (SqlConnection sqlCon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                {
                    sqlCon.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    var result = cmd.ExecuteNonQuery();
                    retval = (result == 1);
                }
            }

            return retval;
        }
    }
}
