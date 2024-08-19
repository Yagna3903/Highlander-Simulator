using Highlander_Components.lander;
using System;
using System.Data.SqlClient;


namespace Highlander_Components.Database
{
    public class DatabaseContext
    {
        private string connectionString;

        public DatabaseContext()
            //got this reference directly from my database
        {
            connectionString = "Data Source =./; Initial Catalog = Highlanders; User ID = UserHighlanders; Password = 12345; Trust Server Certificate = True";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}