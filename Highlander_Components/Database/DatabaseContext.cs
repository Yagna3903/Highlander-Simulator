using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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