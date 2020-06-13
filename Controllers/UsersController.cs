using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using TestSQLI.Models;

namespace TestSQLI.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [Route("users/{userId}")]
        public IActionResult GetOracle(string userId)
        {
            using DbContext db = new DbContext();
            var con = db.Connection;
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM jr.users WHERE user_id = " + userId;
            using var dataReader = cmd.ExecuteReader();
            var results = new LinkedList<User>();
            while (dataReader.Read())
            {
                results.AddLast(new User
                {
                    UserId = dataReader.GetInt64(0),
                    Username = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Password = dataReader.GetString(3),
                    Balance = dataReader.GetInt64(4)
                });
            }
            return Ok(results);
        }

        [HttpGet]
        [Route("usersFixed/{userId}")]
        public IActionResult GetOracleFixed(string userId)
        {
            using DbContext db = new DbContext();
            var con = db.Connection;
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM jr.users WHERE user_id = @UserId";
            cmd.Parameters.Add("UserId", userId);
            using var dataReader = cmd.ExecuteReader();
            var results = new LinkedList<User>();
            while (dataReader.Read())
            {
                results.AddLast(new User
                {
                    UserId = dataReader.GetInt64(0),
                    Username = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Password = dataReader.GetString(3),
                    Balance = dataReader.GetInt64(4)
                });
            }
            return Ok(results);
        }



        [HttpGet]
        [Route("usersMssql/{userId}")]
        public IActionResult Get(string userId)
        {
            using var db = new DbContextMSSQL();
            var con = db.Connection;
            var sql = $"SELECT * FROM users WHERE user_id = {userId}";
            var cmd = new SqlCommand(sql, con);
            var dataReader = cmd.ExecuteReader();
            var results = new LinkedList<User>();
            while (dataReader.Read())
            {
                results.AddLast(new User
                {
                    UserId = dataReader.GetInt32(0),
                    Username = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Password = dataReader.GetString(3),
                    Balance = dataReader.GetInt32(4)
                });
            }
            return Ok(results);
        }

        [HttpGet]
        [Route("usersMssqlFixed/{userId}")]
        public IActionResult GetFixed(string userId)
        {
            using var db = new DbContextMSSQL();
            var con = db.Connection;
            var sql = $"SELECT * FROM users WHERE user_id = @UserId";
            var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("UserId", userId);
            var dataReader = cmd.ExecuteReader();
            var results = new LinkedList<User>();
            while (dataReader.Read())
            {
                results.AddLast(new User
                {
                    UserId = dataReader.GetInt32(0),
                    Username = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Password = dataReader.GetString(3),
                    Balance = dataReader.GetInt32(4)
                });
            }
            return Ok(results);
        }

        [HttpGet]
        [Route("usersTable/{tableName}")]
        public IActionResult GetFromTable(string tableName)
        {
            using var db = new DbContextMSSQL();
            var con = db.Connection;
            var sql = $"SELECT * FROM " + tableName;
            var cmd = new SqlCommand(sql, con);
            var dataReader = cmd.ExecuteReader();
            var results = new LinkedList<Dictionary<string, string>>();
            while (dataReader.Read())
            {
                var dict = new Dictionary<string, string>();
                results.AddLast(dict);
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    dict.Add(dataReader.GetName(i), dataReader.GetString(i));
                }
            }
            return Ok(results);
        }

        [HttpGet]
        [Route("usersTableFixed/{tableName}")]
        public IActionResult GetFromTableFixed(string table)
        {
            string sqlTableName;
            switch(table)
            {
                case "u":
                    sqlTableName = "users";
                    break;
                case "s":
                    sqlTableName = "shop_items";
                    break;
                default:
                    throw new HttpRequestException($"Neteisingas lentelės pavadinimas: {table}");
            }
            using var db = new DbContextMSSQL();
            var con = db.Connection;
            var sql = $"SELECT * FROM " + sqlTableName;
            var cmd = new SqlCommand(sql, con);
            var dataReader = cmd.ExecuteReader();
            var results = new LinkedList<Dictionary<string, string>>();
            while (dataReader.Read())
            {
                var dict = new Dictionary<string, string>();
                results.AddLast(dict);
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    dict.Add(dataReader.GetName(i), dataReader.GetString(i));
                }
            }
            return Ok(results);
        }

        [HttpGet]
        [Route("usersTableFixed2/{tableName}")]
        public IActionResult GetFromTableFixed2(string table)
        {
            var sqlTableName = DoMagicWithString(table);
            using var db = new DbContextMSSQL();
            var con = db.Connection;
            var sql = $"SELECT * FROM " + sqlTableName;
            var cmd = new SqlCommand(sql, con);
            var dataReader = cmd.ExecuteReader();
            var results = new LinkedList<Dictionary<string, string>>();
            while (dataReader.Read())
            {
                var dict = new Dictionary<string, string>();
                results.AddLast(dict);
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    dict.Add(dataReader.GetName(i), dataReader.GetString(i));
                }
            }
            return Ok(results);
        }

        private string DoMagicWithString(string table)
        {
            switch (table)
            {
                case "u":
                    return "users";
                case "s":
                    return "shop_items";
                default:
                    throw new HttpRequestException($"Neteisingas lentelės pavadinimas: {table}");
            }
        }
    }
}
