using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Common;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportMPOWiseDoctorWiseCallDAO : ReturnData
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        DateFormat dateFormat = new DateFormat();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

      
        public List<ReportMPOWiseDoctorWiseCallBEL> GetProduct(string MPGroup)
        {
            string Qry = "SELECT distinct PRODUCT_CODE,PRODUCT_NAME From INV_PRODUCT Where 1=1 ";
            if (MPGroup != "")
            {
                string MP = MPGroup.Substring(MPGroup.Length - 3);
                string Group = MP.Substring(0, 2);
                string MPValue = MPGroup.Substring(MPGroup.Length - 1);
               
                if (Group == "R1")
                {
                    Qry = Qry + " and '" + Group + "'||R1='" + MP + "'";
                }
            }           
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportMPOWiseDoctorWiseCallBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportMPOWiseDoctorWiseCallBEL
                    {
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),

                    }).ToList();
            return item;
        }
        public List<ReportMPOWiseDoctorWiseCallBEL> GetDoctor(DefaultParameterBEO model)
        {
            if (model.FromDate != null && model.ToDate != null)
            {
                model.FromDate = "01-" + model.FromDate.Substring(3, 7);
                model.ToDate = "01-" + model.ToDate.Substring(3, 7);
            }

            string CntDate=DateTime.Now.ToString("MMyyyy");
       
            string qry = "Select distinct DOCTOR_ID,DOCTOR_NAME||' |'||DOCTOR_ID DOCTOR_NAME From DOC_DETAIL Where MONTH_NUMBER||YEAR='" + CntDate + "' ";
         
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                qry += " AND MARKET_CODE='" + model.MPGroup.Substring(0,3) + "'";
            }

            qry += " and ROWNUM<=200";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
          
            List<ReportMPOWiseDoctorWiseCallBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportMPOWiseDoctorWiseCallBEL
                    {                      
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                     

                    }).ToList();
            return item;
        }
        public List<ReportDoctorWiseNoOfCallBEO> GetGridData(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string qry = "Select MP_GROUP,MPO_CODE,MPO_NAME,DESIGNATION,MARKET_NAME,DOCTOR_ID,DOCTOR_NAME,DEGREES,SPECIALIZATION,Sum(VISITED_DOC) VISITED_DOC " +
                " From VW_DOC_WISE_NO_OF_CALL Where SET_DATE BETWEEN '" + model.FromDate + "' and '" + model.ToDate + "'";

            if (model.RegionCode != "" && model.RegionCode != null)
            {
                qry += " AND REGION_CODE = '" + model.RegionCode + "'";
            }
            
            if (model.LocCode != "" && model.LocCode != null)
            {
                qry += " AND MP_GROUP='" + model.LocCode + "'";
            }
            if (model.Designation != "" && model.Designation != null)
            {

                if(model.Designation=="MPO")
                {
                    qry += " AND DESIGNATION IN ('MPO','SMPO')";
                }
                else
                {
                    qry += " AND DESIGNATION='" + model.Designation + "'";
                }
               
            }
            
            if (model.DoctorID != " " && model.DoctorID != "" && model.DoctorID != null)
            {
                qry += " AND DOCTOR_ID='" + model.DoctorID + "'";
            }
            if (model.DoctorName != " " && model.DoctorName != "" && model.DoctorName != null)
            {
                qry += " AND DOCTOR_NAME Like '%" + model.DoctorName + "%'";
            }
            qry += " Group by MP_GROUP,MPO_CODE,MPO_NAME,MARKET_NAME,DESIGNATION,DOCTOR_ID,DOCTOR_NAME,DEGREES,SPECIALIZATION ";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportDoctorWiseNoOfCallBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDoctorWiseNoOfCallBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREES"].ToString(),
                        Specialization = row["SPECIALIZATION"].ToString(),                    
                        TotalCall = row["VISITED_DOC"].ToString(),

                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ReportDoctorWiseNoOfCallBEO>> Export(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
            string vHeader = "";
            //string qry =@" Select MP_GROUP,MPO_CODE,MPO_NAME,DESIGNATION,MARKET_NAME,DOCTOR_ID,DOCTOR_NAME,DEGREES,SPECIALIZATION,Sum(VISITED_DOC) VISITED_DOC 
            //              From VW_DOC_WISE_NO_OF_CALL Where SET_DATE BETWEEN '" + model.FromDate + "' and '" + model.ToDate + "'";

            //    vHeader = vHeader + "Date Between: " + model.FromDate + " To " + model.ToDate;

            //if (model.LocCode != null && model.LocCode != "")
            //{
            //    vHeader = vHeader + ", MPO : " + model.LocName;
            //    qry += " AND MP_GROUP='" + model.LocCode + "'";
            //}

            //if (model.Designation != "" && model.Designation != null)
            //{
            //    if (model.Designation == "MPO")
            //    {
            //        qry += " AND DESIGNATION IN ('MPO','SMPO')";
            //    }
            //    else
            //    {
            //        qry += " AND DESIGNATION='" + model.Designation + "'";
            //    }

            //}
            //if (model.RegionCode != "" && model.RegionCode != null)
            //{
            //    if (model.RegionName != "" && model.RegionName != null)
            //    {
            //        string[] Region = model.RegionName.Split(',');
            //        string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
            //        vHeader = vHeader + ", Region : " + lastItem;
            //    }
            //    qry += " AND REGION_CODE='" + model.RegionCode + "'";
            //}

            //if (model.DoctorID != " " && model.DoctorID != "" && model.DoctorID != null)
            //{
            //    qry += " AND DOCTOR_ID='" + model.DoctorID + "'";
            //}
            //if (model.DoctorName != " " && model.DoctorName != "" && model.DoctorName != null)
            //{
            //    qry += " AND DOCTOR_NAME Like '%" + model.DoctorName + "%'";
            //}


            //qry += " Group by MP_GROUP,MPO_CODE,MPO_NAME,DESIGNATION,MARKET_NAME,DOCTOR_ID,DOCTOR_NAME,DEGREES,SPECIALIZATION ";


        string qry = @"Select MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_NAME, DOCTOR_ID, DOCTOR_NAME, DEGREES, SPECIALIZATION, Sum(VISITED_DOC) VISITED_DOC
         From VW_DOC_WISE_NO_OF_CALL Where SET_DATE BETWEEN '16-aug-2023' and '31-aug-2023'
              Group by MP_GROUP,MPO_CODE,MPO_NAME,MARKET_NAME,DESIGNATION,DOCTOR_ID,DOCTOR_NAME,DEGREES,SPECIALIZATION";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);

            List<ReportDoctorWiseNoOfCallBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDoctorWiseNoOfCallBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREES"].ToString(),
                        Specialization = row["SPECIALIZATION"].ToString(),                 
                        TotalCall = row["VISITED_DOC"].ToString(),

                    }).ToList();
            return Tuple.Create(vHeader, dt, item);
        }



        public List<ReportMPOWiseDoctorWiseCallBEL> GetDetailLink(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string qry = " Select  MPO_NAME,To_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,PRODUCT_NAME,PRODUCT_CODE,PACK_SIZE,QUANTITY,STATUS " +
                         " from VW_DOC_WISE_NO_OF_CALL_DTL Where TO_DATE(SET_DATE) BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "' ";
            if (model.DoctorID != "")
            {
                qry += " AND DOCTOR_ID = '" + model.DoctorID + "'";
            }                   
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }        
            qry += " Order by SET_DATE";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportMPOWiseDoctorWiseCallBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportMPOWiseDoctorWiseCallBEL
                    {
                        MPOName = row["MPO_NAME"].ToString(),
                        FromDate = row["SET_DATE"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        PackSize = row["PACK_SIZE"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                        DcrType = row["STATUS"].ToString(),


                    }).ToList();
            return item;
        }
    }
}