using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;

namespace ConsoleApp.Controller
{
    class EmployeeController
    {
        string dbSConStr = "";
        string comReadCmd = "SELECT Id, FirstName, LastName, BeginDate, DepartmentId, AddressId FROM Employee WHERE DeleteTime IS NULL";
        public EmployeeController(string ConnectionString)
        {
            dbSConStr = ConnectionString;
        }

        public bool Create(string FirstName, string LastName, DateTime BeginDate, int DepartmentId, int AddressId)
        {
            bool retval = false;

            var storedProcedure = "spCreateEmployee";
            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, sqlcon))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@BeginDate", BeginDate);
                    cmd.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                    cmd.Parameters.AddWithValue("@AddressId", AddressId);
                    sqlcon.Open();

                    var result = cmd.ExecuteNonQuery();
                    retval = (result == 1);

                }
            }

            return retval;
        }

        public List<Model.Employee> Read()
        {
            List<Model.Employee> retVal = new List<Model.Employee>();

            using (SqlConnection sqlCon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(comReadCmd, sqlCon))
                {
                    sqlCon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Model.Employee employee = new Model.Employee();
                            employee.Id = (int)reader["Id"];
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.BeginDate = reader["BeginDate"] != null && reader["BeginDate"] != DBNull.Value ? Convert.ToDateTime(reader["BeginDate"]) : DateTime.MinValue;

                            retVal.Add(employee);
                        }

                    }
                }
            }

            return retVal;
        }

        public List<Model.Employee> ReadDapper()
        {
            List<Model.Employee> retval = new List<Model.Employee>();

            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                retval = sqlcon.Query<Model.Employee>(comReadCmd).AsList();
            }

            return retval;
        }

        public bool Update(int id, string FirstName, string LastName, DateTime BeginDate, int DepartmentId, int AddressId)
        {
            bool retval = false;
            var storedProcedure = "spCreateEmployee";
            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, sqlcon))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcon.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@BeginDate", BeginDate);
                    cmd.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                    cmd.Parameters.AddWithValue("@AddressId", AddressId);
                    var result = cmd.ExecuteNonQuery();
                    retval = (result == 1);

                }
            }

            return retval;

        }

        public bool Delete(int id = 0)
        {
            bool retval = false;
            var query = "update employee set DeleteTime = GetDate() where id = @Id";

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

