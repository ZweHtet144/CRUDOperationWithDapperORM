using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperCRUDOperation.Controllers
{
    public class GetConnectionStringController : ControllerBase
    {
        private SqlConnection connection;
        private IConfiguration config;

        public GetConnectionStringController(IConfiguration config)
        {
            this.config = config;
        }
        public SqlConnection OpenConnection(string BusinessName)
        {
            string conStr = config.GetConnectionString(BusinessName);
            this.connection = new SqlConnection(conStr);
            this.connection.Open();
            return this.connection;
        }
        public void CloseConnection()
        {
            if (this.connection.State == ConnectionState.Open)
            {
                this.connection.Close();
            }
        }
    }
}
