using System;
using System.Data;
using System.Data.SqlClient;

namespace SQL
{
    public class MovieSQLQuerier
    {
        private static string connection_string;

        public MovieSQLQuerier()
        {
            connection_string = "Data Source=movieinventoryapp.database.windows.net;Initial Catalog=MovieDatabase;Persist Security Info=True;User ID=victorchu123;Password=684Dvkx2q0V#1;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False";
        }

        public void QueryOrderData(string query_string)
        {
            using (SqlConnection connection =
                       new SqlConnection(connection_string))
            {
                SqlCommand command =
                    new SqlCommand(query_string, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data.
                while (reader.Read())
                {
                    //ReadSingleRow((IDataRecord)reader);
                }

                // Call Close when done reading.
                reader.Close();
            }
        }


    }
}