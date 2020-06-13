using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TestSQLI
{
    public class DbContextMSSQL : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public DbContextMSSQL()
        {
            const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestSQLI;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets = true;";
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
        
    }
}
