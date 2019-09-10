using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CompanyApp.Controller {
    class CompanyController {
        string connectionString = "";
        string companyReadCmd = "select Id, Name, FoundedDate from Company WHERE DeleteTime IS NULL";

        public CompanyController(string connectionString) {
            this.connectionString = connectionString;
        }

        public bool Create(string name, DateTime foundingDate) {
            bool retval = false;

            var storedProcedure = "spCreateCompany";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConnection)) {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Name", name);
                    sqlCommand.Parameters.AddWithValue("@FoundedDate", foundingDate);
                    sqlConnection.Open();

                    var result = sqlCommand.ExecuteNonQuery();
                    retval = (result == 1);
                }
            }

            return retval;
        }

        public List<Model.Company> Read() {
            List<Model.Company> companies = new List<Model.Company>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = new SqlCommand(companyReadCmd, sqlConnection)) {
                    sqlConnection.Open();

                    using (SqlDataReader sqlReader = cmd.ExecuteReader()) {
                        while (sqlReader.Read()) {
                            Model.Company company;

                            company = new Model.Company {
                                Id = (int) sqlReader["Id"],
                                Name = sqlReader["Name"].ToString(),
                                FoundedDate = sqlReader["FoundedDate"] != null ? Convert.ToDateTime(sqlReader["FoundedDate"]) : DateTime.MinValue
                            };
                            companies.Add(company);
                        }
                    }
                }
            }

            return companies;
        }

        public bool Update(int id, string name, DateTime foundingDate) {
            bool retval = false;

            var storedProcedure = "spCreateCompany";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConnection)) {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    sqlCommand.Parameters.AddWithValue("@Name", name);
                    sqlCommand.Parameters.AddWithValue("@FoundedDate", foundingDate);
                    sqlConnection.Open();

                    var result = sqlCommand.ExecuteNonQuery();
                    retval = (result == 1);
                }
            }

            return retval;
        }

        public bool Delete(int id) {
            bool retval = false;
            var query = $"update company set DeleteTime = GetDate() where id = @id";

            using (SqlConnection sqlcon = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon)) {
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
