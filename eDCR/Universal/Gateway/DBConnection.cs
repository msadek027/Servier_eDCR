using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace eDCR.DAL.Gateway
{
    public class DBConnection
    {
        string connectionString = "";
        public DBConnection()
        {           
            SAConnStrReader();           
        }

        public string SAConnStrReader()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Conn"].ToString();
            return connectionString;
        }
       

    }
}