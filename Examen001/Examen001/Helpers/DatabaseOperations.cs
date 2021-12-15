using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Examen001.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Examen001.Controllers
{
    public class DatabaseOperations : Controller
    {
        public readonly string _connectionString = string.Empty;
        public static DatabaseOperations Methods => new DatabaseOperations();
        private static bool ServerOnline { get; set; }

        public DatabaseOperations()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _connectionString = root.GetSection("ConnectionStrings").GetSection("DevConnections").Value;
            var appSetting = root.GetSection("ApplicationSettings");

            ValidateConnection();
        }

        private bool ValidateConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open(); // throws if invalid
                    conn.Close();
                }
                ServerOnline = true;
            }
            catch (Exception errorException)
            {
                ServerOnline = false;
            }
            return ServerOnline;
        }

        public DataSet ExecuteCommand(SqlCommand command)
        {
            if (!ServerOnline)
            {
                throw new Exception("La conexión al servidor se perdio. Reintente de nuevo");
            }

            DataSet ds = new DataSet("Resultado");
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlComm = command;
                sqlComm.Connection = conn;
                sqlComm.CommandTimeout = 15000;
                SqlDataAdapter da = new SqlDataAdapter { SelectCommand = sqlComm };
                da.Fill(ds);
            }
            return ds;
        }

        public string ConnectionString
        {
            get => _connectionString;
        }
    }
}
