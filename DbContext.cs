using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestSQLI
{
    public class DbContext : IDisposable
    {
        public OracleConnection Connection { get; set; }
        public DbContext()
        {
            Connection = new OracleConnection
            {
                ConnectionString = "User Id=JR;Password=JR;Data Source=localhost:1521/dba.sqli_test"
            };
            Connection.Open();
        }
        
        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
