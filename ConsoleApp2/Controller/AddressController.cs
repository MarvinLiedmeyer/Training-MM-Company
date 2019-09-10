using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;

namespace ConsoleApp.Controller
{
    class AddressController
    {
        string dbSConStr = "";
        string comReadCmd = "SELECT Id, Street, City, Zip, Country FROM Address WHERE DeleteTime IS NULL";
        public AddressController(string ConnectionString)
        {
            dbSConStr = ConnectionString;
        }

        public bool Create(string Street, string City, string Zip, string Country)
        {
            bool retval = false;

            var storedProcedure = "spCreateAddress";
            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, sqlcon))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@Street", Street);
                    cmd.Parameters.AddWithValue("@City", City);
                    cmd.Parameters.AddWithValue("@Zip", Zip);
                    cmd.Parameters.AddWithValue("@Country", Country);
                    sqlcon.Open();

                    var result = cmd.ExecuteNonQuery();
                    retval = (result == 1);

                }
            }

            return retval;
        }

        public List<Model.Address> Read()
        {
            List<Model.Address> retVal = new List<Model.Address>();

            using (SqlConnection sqlCon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(comReadCmd, sqlCon))
                {
                    sqlCon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Model.Address Address = new Model.Address();
                            Address.Id = (int)reader["Id"];
                            Address.Street = reader["Street"].ToString();
                            Address.City = reader["City"].ToString();
                            Address.Zip = reader["Zip"].ToString();
                            Address.Country = reader["Country"].ToString();

                            retVal.Add(Address);
                        }

                    }
                }
            }

            return retVal;
        }

        public List<Model.Address> ReadDapper()
        {
            List<Model.Address> retval = new List<Model.Address>();

            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                retval = sqlcon.Query<Model.Address>(comReadCmd).AsList();
            }

            return retval;
        }

        public bool Update(int id, string Street, string City, string Zip, string Country)
        {
            bool retval = false;
            var storedProcedure = "spCreateAddress";
            using (SqlConnection sqlcon = new SqlConnection(dbSConStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, sqlcon))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcon.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Street", Street);
                    cmd.Parameters.AddWithValue("@City", City);
                    cmd.Parameters.AddWithValue("@Zip", Zip);
                    cmd.Parameters.AddWithValue("@Country", Country);
                    var result = cmd.ExecuteNonQuery();
                    retval = (result == 1);

                }
            }

            return retval;

        }

        public bool Delete(int id = 0)
        {
            bool retval = false;
            var query = "update address set DeleteTime = GetDate() where id = @Id";

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
