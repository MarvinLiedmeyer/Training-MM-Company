using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using ConsoleApp.Model;
using ConsoleApp.Interface;


namespace ConsoleApp.Repository
{
    public class CompanyRepository : IBaseInterface<Company>
    {
        string connectionString = "";
        string companyReadCmd = "select Id, Name, FoundedDate from Company WHERE DeleteTime IS NULL";

        public CompanyRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Company Create(Company data)
        {
            Company retval = new Company();

            var storedProcedure = "spCreateCompany";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Name", data.Name);
                    sqlCommand.Parameters.AddWithValue("@FoundedDate", data.FoundedDate);
                    sqlConnection.Open();

                    var result = sqlCommand.ExecuteNonQuery();
                }
            }

            return retval;
        }

        public List<Company> Read()
        {
            string companyReadCmd = "SELECT Id, Name, FoundedDate FROM Company WHERE DeleteTime IS NULL";
            List<Model.Company> retVal = new List<Company>();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(companyReadCmd, sqlCon))
                {
                    sqlCon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Model.Company company = new Model.Company();

                            company.Id = (int)reader["Id"];
                            company.Name = reader["Name"].ToString();
                            company.FoundedDate = reader["FoundedDate"] != null && reader["FoundedDate"] != DBNull.Value ? Convert.ToDateTime(reader["FoundedDate"]) : DateTime.MinValue;
                            retVal.Add(company);

                        }
                    }
                }
            }
            return retVal;
        }

        public Company Update(Company data)
        {
            Company retval = new Company();
            var storedProcedure = "spCreateCompany";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Id", data.Id);
                    sqlCommand.Parameters.AddWithValue("@Name", data.Name);
                    sqlCommand.Parameters.AddWithValue("@FoundedDate", data.FoundedDate);
                    sqlConnection.Open();

                    var result = sqlCommand.ExecuteNonQuery();
                }
            }

            return retval;
        }

        public bool Delete(int id)
        {
            bool retval = false;
            var query = $"update company set DeleteTime = GetDate() where id = @id AND DeleTime = NULL";

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var result = cmd.ExecuteNonQuery();
                    retval = (result == 1);
                }
            }

            return retval;
        }
    }
}
