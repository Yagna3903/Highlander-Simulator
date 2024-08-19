
using System.Data.SqlClient;


namespace Highlander_Components.Database
{
    public class DatabaseContext
    {
        private string connectionString;

        public DatabaseContext()
        {
            connectionString = "Server=(local);Database=HighlanderDB;User=UserHighlanders;Password=12345";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}