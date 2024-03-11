using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using eDCR.Models;
using Systems.Universal;


namespace eDCR.DAL.Gateway
{

    public class DBHelper : SaHelper
    {
       
        DBConnection dbConnection = new DBConnection();
        public Boolean CmdExecute(string ConnString,string Qry)
        {
            bool isTrue = false;
            using (OracleConnection con = new OracleConnection(ConnString))
            {
                OracleCommand cmd = new OracleCommand(Qry, con);
                con.Open();
                int noOfRows = cmd.ExecuteNonQuery();           

                if (noOfRows > 0)
                {
                    isTrue = true;
                  
                }
            }
            return isTrue;
        }

        public List<string> GetListString(string ConnString,string Qry)
        {
            DataTable dt = GetDataTable(ConnString,Qry);
            List<string> obj = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(dt.Rows[i][0].ToString());
            }
            return obj;
        }
        public List<string> GetListString(DataTable dt)
        {
           
            List<string> obj = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(dt.Rows[i][0].ToString());
            }
            return obj;
        }
        public Tuple<Boolean, string> IsExistsWithGetSL(string ConnString,string Qry)
        {
            string GetSL = ""; bool isTrue = false;

            DataTable dt = GetDataTable(ConnString, Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
                GetSL = dt.Rows[0][0].ToString();
            }
            return Tuple.Create(isTrue, GetSL);
        }

      


        public Tuple<Boolean, string,string> IsExistsWithGetSLnID(string ConnString, string Qry)
        {
            string GetSL1 = ""; string GetSL2 = ""; bool isTrue = false;

            DataTable dt = GetDataTable(ConnString, Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
                GetSL1 = dt.Rows[0][0].ToString();
                GetSL2 = dt.Rows[0][1].ToString();
            }
            return Tuple.Create(isTrue, GetSL1, GetSL2);
        }
        public string GetSingleToken(string MPGRoup)
        {
          
            string GetToken= "";
            string Qry = "Select Token From Sa_UserToken Where MP_Group='" + MPGRoup + "'";
            DataTable dt = GetDataTable(dbConnection.SAConnStrReader(), Qry);
            if (dt.Rows.Count > 0)
            {              
                GetToken = dt.Rows[0][0].ToString();
            }
            return GetToken;
        }
        public string GetMultipleToken(string MPGRoup)
        {
            string GetToken = "";
            string Qry = "Select Token From Sa_UserToken Where MP_Group in (" + MPGRoup + ")";
            DataTable dt = GetDataTable(dbConnection.SAConnStrReader(), Qry);
            if (dt.Rows.Count > 0)
            {

                GetToken = dt.Rows[0][0].ToString();
            }
            return "";
        }
        public Int64 CmdExecute(string Qry)
        {
            Int64 noOfRows = 0;
            using (OracleConnection con = new OracleConnection(dbConnection.SAConnStrReader()))
            {
                OracleCommand cmd = new OracleCommand(Qry, con);
                con.Open();
                noOfRows = cmd.ExecuteNonQuery();
            }
            return noOfRows;
        }
       
       
        public DataSet GetDataSet(string Qry)
        {
            OracleDataAdapter odbcDataAdapter = new OracleDataAdapter(Qry, dbConnection.SAConnStrReader());
          DataSet ds = new DataSet();
          odbcDataAdapter.Fill(ds, "Results");
          return ds;           
        }
        public DataTable GetDataTable(string ConnString,string Qry)
        {
            DataTable dt = new DataTable();
            using (OracleConnection objConn = new OracleConnection(ConnString))
            {
                OracleCommand objCmd = new OracleCommand();
                objCmd.CommandText = Qry;
                objCmd.Connection = objConn;
                objConn.Open();
                objCmd.ExecuteNonQuery();
                using (OracleDataReader rdr = objCmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        dt.Load(rdr);
                    }
                }
            }
            return dt;
        }

        public string GetValue(string Qry)
        {
            string Value = "";
            OracleConnection odbcConnection = new OracleConnection(dbConnection.SAConnStrReader());
            odbcConnection.Open();
            OracleCommand odbcCommand = new OracleCommand(Qry, odbcConnection);
            OracleDataReader rdr = odbcCommand.ExecuteReader();
            if (rdr.Read())
            {
                Value = rdr[0].ToString();
            }
            rdr.Close();
            odbcConnection.Close();
            return Value;
        }
        public DataTable dtIncremented(DataTable dt)
        {
            DataTable dtIncremented = new DataTable(dt.TableName);
            DataColumn dc = new DataColumn("Col1");
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;
            dc.AutoIncrementStep = 1;
            dc.DataType = typeof(Int32);
            dtIncremented.Columns.Add(dc);

            dtIncremented.BeginLoadData();

            DataTableReader dtReader = new DataTableReader(dt);
            dtIncremented.Load(dtReader);

            dtIncremented.EndLoadData();

            return dtIncremented;
        }
        public DataTable GetDataTableRefCursorF1(string funName, string FieldName, string FieldValue)
        {
            DataTable dt = new DataTable();
            using (OracleConnection objConn = new OracleConnection(dbConnection.SAConnStrReader()))
            {
                using (OracleCommand objCmd = new OracleCommand())
                {
                    objCmd.Connection = objConn;
                    objCmd.CommandText = funName;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add(FieldName, OracleType.VarChar).Value = FieldValue;
                    objCmd.Parameters.Add("ReturnValue", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            dt.Load(rdr);
                        }
                    }
                }
            }
            return dt;
        }
        public DataTable GetDataTableRefCursorF2(string funName, string FieldName1, string FieldName2, string FieldValue1, string FieldValue2)
        {
            DataTable dt = new DataTable();
            using (OracleConnection objConn = new OracleConnection(dbConnection.SAConnStrReader()))
            {

                using (OracleCommand objCmd = new OracleCommand())
                {
                    objCmd.Connection = objConn;
                    objCmd.CommandText = funName;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add(FieldName1, OracleType.VarChar).Value = FieldValue1;
                    objCmd.Parameters.Add(FieldName2, OracleType.VarChar).Value = FieldValue2;
                    objCmd.Parameters.Add("ReturnValue", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            dt.Load(rdr);
                        }
                    }
                }
            }
            return dt;
        }


        public DataTable GetDataTableRefCursorF3(string funName, string FieldName1, string FieldName2, string FieldName3, string FieldValue1, string FieldValue2, string FieldValue3)
        {
            DataTable dt = new DataTable();
            using (OracleConnection objConn = new OracleConnection(dbConnection.SAConnStrReader()))
            {

                using (OracleCommand objCmd = new OracleCommand())
                {
                    objCmd.Connection = objConn;
                    objCmd.CommandText = funName;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add(FieldName1, OracleType.VarChar).Value = FieldValue1;
                    objCmd.Parameters.Add(FieldName2, OracleType.VarChar).Value = FieldValue2;
                    objCmd.Parameters.Add(FieldName3, OracleType.VarChar).Value = FieldValue3;
                    objCmd.Parameters.Add("ReturnValue", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            dt.Load(rdr);
                        }
                    }
                }
            }
            return dt;
        }


        public Boolean CmdProcedureF1(string Qry, string SPName, string FieldName, string FieldValue)
        {
            bool isTrue = false;
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.SAConnStrReader()))
            {
                using (OracleCommand oracleCommand = new OracleCommand())
                {
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.CommandText = SPName;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add(FieldName, OracleType.VarChar).Value = FieldValue;            
                    oracleConnection.Open();
                    if (oracleCommand.ExecuteNonQuery() > 0)
                    {
                        isTrue = true;                       
                    }
                }
            }
            return isTrue;
        }

        public Boolean CmdProcedureF2(string SPName, string FieldName1, string FieldName2,  string FieldValue1, string FieldValue2)
        {
            bool isTrue = false;
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.SAConnStrReader()))
            {
                using (OracleCommand oracleCommand = new OracleCommand())
                {
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.CommandText = SPName;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add(FieldName1, OracleType.VarChar).Value = FieldValue1;
                    oracleCommand.Parameters.Add(FieldName2, OracleType.VarChar).Value = FieldValue2;                  
                    oracleConnection.Open();
                    if (oracleCommand.ExecuteNonQuery() > 0)
                    {
                        isTrue = true;
                    }
                }
            }
            return isTrue;
        }

        public Boolean CmdProcedureF3(string SPName, string FieldName1, string FieldName2, string FieldName3, string FieldValue1, string FieldValue2, string FieldValue3)
        {
            bool isTrue = false;
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.SAConnStrReader()))
            {
                using (OracleCommand oracleCommand = new OracleCommand())
                {
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.CommandText = SPName;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add(FieldName1, OracleType.VarChar).Value = FieldValue1;
                    oracleCommand.Parameters.Add(FieldName2, OracleType.VarChar).Value = FieldValue2;
                    oracleCommand.Parameters.Add(FieldName3, OracleType.VarChar).Value = FieldValue3;
                    oracleConnection.Open();

                    if (oracleCommand.ExecuteNonQuery() > 0)
                    {
                        isTrue = true;
                    }
                }
            }
            return isTrue;
        }



    

        public DataTable dt { get; set; }
    }
}