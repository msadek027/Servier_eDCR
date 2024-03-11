using eDCR.Areas.DCR.Models.BEL;
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
    public class ReportItemStatementDAO
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        DateFormat dateFormat = new DateFormat();
        public Tuple<DataTable, List<ReportItemStatementBEL>> GetMainGridData(DefaultParameterBEO model)
        {

            string Qry =@" Select PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,ITEM_TYPE||ITEM_FOR ITEM_TYPE,MP_GROUP,MARKET_NAME, REST_QTY,CENTRAL_QTY,FRST_WEEK,SECND_WEEK,THRD_WEEK, FRTH_WEEK,
                         D01, D02, D03, D04, D05, D06, D07, D08, D09, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31,
                         FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK TotalQtyAddDefi0130,
                         REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK  Total_QTY,
                         REST_QTY+CENTRAL_QTY+FRST_WEEK FRST_WEEK_STK,
                         D01+D02+D03+D04+D05+D06+D07+D08 D0108,
                         REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08) SECND_WEEK_STK,
                         D09+D10+D11+D12+D13+D14+D15 D0915,
                         REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15) THRD_WEEK_STK,
                         D16+D17+D18+D19+D20+D21+D22+D23 D1623,
                         REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23) FRTH_WEEK_STK,
                         D24+D25+D26+D27+D28+D29+D30+D31 D2431,
                         (D01 + D02 + D03 + D04 + D05 + D06 + D07 + D08 + D09 + D10 + D11 + D12 + D13 + D14 + D15 + D16 + D17 + D18 + D19 + D20 + D21 + D22 + D23 + D24 + D25 + D26 + D27 + D28 + D29 + D30 + D31) D0131,
                         REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23+D24+D25+D26+D27+D28+D29+D30+D31) Closing_STK
                         from VW_INV_SAMPLE_STATEMENT 
                          Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND PRODUCT_CODE!='SmRPPM' ";
          
            if (model.ItemType != "All" && model.ItemType !=null)
            {
                if (model.ItemType == "SlSmRI" )
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Sl','Sm') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "GtRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Gt') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "SlR" || model.ItemType == "SmR" || model.ItemType == "SmI" || model.ItemType == "GtR" || model.ItemType == "GtI")
                {
                    Qry = Qry + " and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }                
            }
            
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TERRITORY_CODE='" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
           
            Qry = Qry + " Order by PRODUCT_NAME ";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportItemStatementBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportItemStatementBEL
                    {
                        SL = row["Col1"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                   
                        
                        ItemType = row["ITEM_TYPE"].ToString(), 
                        MPGroup = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),

                        //TotalQtyAddDefi0130 =(Convert.ToInt64(row["FRST_WEEK"].ToString())+Convert.ToInt64(row["SECND_WEEK"].ToString())+Convert.ToInt64(row["THRD_WEEK"].ToString())+Convert.ToInt64(row["FRTH_WEEK"].ToString())).ToString() ,
                        //TotalQty =(Convert.ToInt64(row["FRST_WEEK"].ToString())+Convert.ToInt64(row["SECND_WEEK"].ToString())+Convert.ToInt64(row["THRD_WEEK"].ToString())+Convert.ToInt64(row["FRTH_WEEK"].ToString())+Convert.ToInt64(row["Total_QTY"].ToString())).ToString(),
                        TotalQtyAddDefi0130 = row["TotalQtyAddDefi0130"].ToString(),
                        TotalQty = row["Total_QTY"].ToString(),
                        
                        GivenQty0108 = row["FRST_WEEK"].ToString(),
                        GivenQty0915 = row["SECND_WEEK"].ToString(),
                        GivenQty1623 = row["THRD_WEEK"].ToString(),
                        GivenQty2431 = row["FRTH_WEEK"].ToString(),

                        TotalStock0108 = row["FRST_WEEK_STK"].ToString(),
                        TotalDCR0108 = row["D0108"].ToString(),
                        TotalStock0915 = row["SECND_WEEK_STK"].ToString(),
                        TotalDCR0915 = row["D0915"].ToString(),
                        TotalStock1623 = row["THRD_WEEK_STK"].ToString(),
                        TotalDCR1623 = row["D1623"].ToString(),
                        TotalStock2431 = row["FRTH_WEEK_STK"].ToString(),
                        TotalDCR2431 = row["D2431"].ToString(),
                        TotalDCR0131 = row["D0131"].ToString(),
                        ClosingStock = row["Closing_STK"].ToString(),

                        d01 = row["D01"].ToString(),
                        d02 = row["D02"].ToString(),
                        d03 = row["D03"].ToString(),
                        d04 = row["D04"].ToString(),
                        d05 = row["D05"].ToString(),
                        d06 = row["D06"].ToString(),
                        d07 = row["D07"].ToString(),
                        d08 = row["D08"].ToString(),
                        d09 = row["D09"].ToString(),
                        d10 = row["D10"].ToString(),
                        d11 = row["D11"].ToString(),
                        d12 = row["D12"].ToString(),
                        d13 = row["D13"].ToString(),
                        d14 = row["D14"].ToString(),
                        d15 = row["D15"].ToString(),
                        d16 = row["D16"].ToString(),
                        d17 = row["D17"].ToString(),
                        d18 = row["D18"].ToString(),
                        d19 = row["D19"].ToString(),
                        d20 = row["D20"].ToString(),
                        d21 = row["D21"].ToString(),
                        d22 = row["D22"].ToString(),
                        d23 = row["D23"].ToString(),
                        d24 = row["D24"].ToString(),
                        d25 = row["D25"].ToString(),
                        d26 = row["D26"].ToString(),
                        d27 = row["D27"].ToString(),
                        d28 = row["D28"].ToString(),
                        d29 = row["D29"].ToString(),
                        d30 = row["D30"].ToString(),
                        d31 = row["D31"].ToString(),

                       
                    }).ToList();
            
            return Tuple.Create(dt, item);
        }



        public Tuple<string, DataTable, List<ReportItemStatementBEL>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";

            string Qry = " Select PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,ITEM_TYPE||ITEM_FOR ITEM_TYPE,MP_GROUP,MARKET_NAME, REST_QTY,CENTRAL_QTY,FRST_WEEK,SECND_WEEK,THRD_WEEK, FRTH_WEEK," +
     " D01, D02, D03, D04, D05, D06, D07, D08, D09, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31," +
     " FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK TotalQtyAddDefi0130," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK  Total_QTY," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK FRST_WEEK_STK," +
     " D01+D02+D03+D04+D05+D06+D07+D08 D0108," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08) SECND_WEEK_STK," +
     " D09+D10+D11+D12+D13+D14+D15 D0915," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15) THRD_WEEK_STK," +
     " D16+D17+D18+D19+D20+D21+D22+D23 D1623," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23) FRTH_WEEK_STK," +
     " D24+D25+D26+D27+D28+D29+D30+D31 D2431," +
     " (D01 + D02 + D03 + D04 + D05 + D06 + D07 + D08 + D09 + D10 + D11 + D12 + D13 + D14 + D15 + D16 + D17 + D18 + D19 + D20 + D21 + D22 + D23 + D24 + D25 + D26 + D27 + D28 + D29 + D30 + D31) D0131," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23+D24+D25+D26+D27+D28+D29+D30+D31) Closing_STK" +
     " from VW_INV_SAMPLE_STATEMENT Where Year=" + model.Year + " AND Month_Number='" + model.MonthNumber + "' AND PRODUCT_CODE!='SmRPPM' ";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;

            string Item = "All";
       
            if (model.ItemType != "All" && model.ItemType != null)
            {
                if (model.ItemType == "SlSmRI")
                {
                    Qry = Qry + " AND ITEM_TYPE IN ('Sl','Sm') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "GtRI")
                {
                    Qry = Qry + " AND ITEM_TYPE IN ('Gt') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "SlR" || model.ItemType == "SmR" || model.ItemType == "SmI" || model.ItemType == "GtR" || model.ItemType == "GtI")
                {
                    Qry = Qry + " AND ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }
                Item= model.ItemTypeName;                
            }
            vHeader = vHeader + ", Item Type : " + Item;

         
             if (model.MPGroup != null && model.MPGroup != "")
             {
                 vHeader = vHeader + ", MPO : " + model.MPOName;
                 Qry += " AND MP_GROUP='" + model.MPGroup + "'";
             }
             if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
             {
                 if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                 {
                     string[] Territory = model.TerritoryManagerName.Split(',');
                     string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                     vHeader = vHeader + ", Territory : " + lastItem;
                 }
                 Qry += " AND TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
             }
             if (model.RegionCode != "" && model.RegionCode != null)
             {
                 if (model.RegionName != "" && model.RegionName != null)
                 {
                     string[] Region = model.RegionName.Split(',');
                     string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                     vHeader = vHeader + ", Region : " + lastItem;
                 }
             }
           
            
            Qry = Qry + " Order by PRODUCT_NAME ";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportItemStatementBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportItemStatementBEL
                    {
                        SL = row["Col1"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),           

                        ItemType = row["ITEM_TYPE"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),

                        //TotalQtyAddDefi0130 = (Convert.ToInt64(row["FRST_WEEK"].ToString()) + Convert.ToInt64(row["SECND_WEEK"].ToString()) + Convert.ToInt64(row["THRD_WEEK"].ToString()) + Convert.ToInt64(row["FRTH_WEEK"].ToString())).ToString(),
                        //TotalQty = (Convert.ToInt64(row["FRST_WEEK"].ToString()) + Convert.ToInt64(row["SECND_WEEK"].ToString()) + Convert.ToInt64(row["THRD_WEEK"].ToString()) + Convert.ToInt64(row["FRTH_WEEK"].ToString()) + Convert.ToInt64(row["Total_QTY"].ToString())).ToString(),


                        TotalQtyAddDefi0130 =row["TotalQtyAddDefi0130"].ToString(),
                        TotalQty = row["Total_QTY"].ToString(),
                        GivenQty0108 = row["FRST_WEEK"].ToString(),
                        GivenQty0915 = row["SECND_WEEK"].ToString(),
                        GivenQty1623 = row["THRD_WEEK"].ToString(),
                        GivenQty2431 = row["FRTH_WEEK"].ToString(),

                        TotalStock0108 = row["FRST_WEEK_STK"].ToString(),
                        TotalDCR0108 = row["D0108"].ToString(),
                        TotalStock0915 = row["SECND_WEEK_STK"].ToString(),
                        TotalDCR0915 = row["D0915"].ToString(),
                        TotalStock1623 = row["THRD_WEEK_STK"].ToString(),
                        TotalDCR1623 = row["D1623"].ToString(),
                        TotalStock2431 = row["FRTH_WEEK_STK"].ToString(),
                        TotalDCR2431 = row["D2431"].ToString(),
                        TotalDCR0131 = row["D0131"].ToString(),
                        ClosingStock = row["Closing_STK"].ToString(),

                        d01 = row["D01"].ToString(),
                        d02 = row["D02"].ToString(),
                        d03 = row["D03"].ToString(),
                        d04 = row["D04"].ToString(),
                        d05 = row["D05"].ToString(),
                        d06 = row["D06"].ToString(),
                        d07 = row["D07"].ToString(),
                        d08 = row["D08"].ToString(),
                        d09 = row["D09"].ToString(),
                        d10 = row["D10"].ToString(),
                        d11 = row["D11"].ToString(),
                        d12 = row["D12"].ToString(),
                        d13 = row["D13"].ToString(),
                        d14 = row["D14"].ToString(),
                        d15 = row["D15"].ToString(),
                        d16 = row["D16"].ToString(),
                        d17 = row["D17"].ToString(),
                        d18 = row["D18"].ToString(),
                        d19 = row["D19"].ToString(),
                        d20 = row["D20"].ToString(),
                        d21 = row["D21"].ToString(),
                        d22 = row["D22"].ToString(),
                        d23 = row["D23"].ToString(),
                        d24 = row["D24"].ToString(),
                        d25 = row["D25"].ToString(),
                        d26 = row["D26"].ToString(),
                        d27 = row["D27"].ToString(),
                        d28 = row["D28"].ToString(),
                        d29 = row["D29"].ToString(),
                        d30 = row["D30"].ToString(),
                        d31 = row["D31"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }

        public List<ReportItemStatementBEL> GetGrandTotalSum(DefaultParameterBEO model,DataTable dtT)
        {




            var newDt = dtT.AsEnumerable()
                // .GroupBy(r => r.Field<string>("MP_GROUP")) //  OR
                //.GroupBy(g => new { Col1 = g["MP_GROUP"] }) //  OR
                .GroupBy(g => new { Col1 ="" })   //Where Group is Empty Group
                    .Select(g =>
                    {
                        var row = dtT.NewRow();

                        row["MP_GROUP"] = g.Key;
                        row["REST_QTY"] = g.Sum(r => (decimal)r["REST_QTY"]);
                        row["CENTRAL_QTY"] = g.Sum(r => (decimal)r["CENTRAL_QTY"]);

                        row["TotalQtyAddDefi0130"] = g.Sum(r => (decimal)r["TotalQtyAddDefi0130"]);
                        row["Total_QTY"] = g.Sum(r => (decimal)r["Total_QTY"]);

                        row["FRST_WEEK"] = g.Sum(r => (decimal)r["FRST_WEEK"]);
                        row["SECND_WEEK"] = g.Sum(r => (decimal)r["SECND_WEEK"]);
                        row["THRD_WEEK"] = g.Sum(r => (decimal)r["THRD_WEEK"]);
                        row["FRTH_WEEK"] = g.Sum(r => (decimal)r["FRTH_WEEK"]);


                        row["FRST_WEEK_STK"] = g.Sum(r => (decimal)r["FRST_WEEK_STK"]);
                        row["D0108"] = g.Sum(r => (decimal)r["D0108"]);
                        row["SECND_WEEK_STK"] = g.Sum(r => (decimal)r["SECND_WEEK_STK"]);
                        row["D0915"] = g.Sum(r => (decimal)r["D0915"]);
                        row["THRD_WEEK_STK"] = g.Sum(r => (decimal)r["THRD_WEEK_STK"]);
                        row["D1623"] = g.Sum(r => (decimal)r["D1623"]);
                        row["FRTH_WEEK_STK"] = g.Sum(r => (decimal)r["FRTH_WEEK_STK"]);
                        row["D2431"] = g.Sum(r => (decimal)r["D2431"]);
                        row["D0131"] = g.Sum(r => (decimal)r["D0131"]);
                        row["Closing_STK"] = g.Sum(r => (decimal)r["Closing_STK"]);

                        row["D01"] = g.Sum(r => (decimal)r["D01"]);
                        row["D02"] = g.Sum(r => (decimal)r["D02"]);
                        row["D03"] = g.Sum(r => (decimal)r["D03"]);
                        row["D04"] = g.Sum(r => (decimal)r["D04"]);
                        row["D05"] = g.Sum(r => (decimal)r["D05"]);
                        row["D06"] = g.Sum(r => (decimal)r["D06"]);
                        row["D07"] = g.Sum(r => (decimal)r["D07"]);
                        row["D08"] = g.Sum(r => (decimal)r["D08"]);
                        row["D09"] = g.Sum(r => (decimal)r["D09"]);
                        row["D10"] = g.Sum(r => (decimal)r["D10"]);
                        row["D11"] = g.Sum(r => (decimal)r["D11"]);
                        row["D12"] = g.Sum(r => (decimal)r["D12"]);
                        row["D13"] = g.Sum(r => (decimal)r["D13"]);
                        row["D14"] = g.Sum(r => (decimal)r["D14"]);
                        row["D15"] = g.Sum(r => (decimal)r["D15"]);
                        row["D16"] = g.Sum(r => (decimal)r["D16"]);
                        row["D17"] = g.Sum(r => (decimal)r["D17"]);
                        row["D18"] = g.Sum(r => (decimal)r["D18"]);
                        row["D19"] = g.Sum(r => (decimal)r["D19"]);
                        row["D20"] = g.Sum(r => (decimal)r["D20"]);
                        row["D21"] = g.Sum(r => (decimal)r["D21"]);
                        row["D22"] = g.Sum(r => (decimal)r["D22"]);
                        row["D23"] = g.Sum(r => (decimal)r["D23"]);
                        row["D24"] = g.Sum(r => (decimal)r["D24"]);
                        row["D25"] = g.Sum(r => (decimal)r["D25"]);
                        row["D26"] = g.Sum(r => (decimal)r["D26"]);
                        row["D27"] = g.Sum(r => (decimal)r["D27"]);
                        row["D28"] = g.Sum(r => (decimal)r["D28"]);
                        row["D29"] = g.Sum(r => (decimal)r["D29"]);
                        row["D30"] = g.Sum(r => (decimal)r["D30"]);
                        row["D31"] = g.Sum(r => (decimal)r["D31"]);


                        return row;
                    }).CopyToDataTable();



            DataTable dt = newDt;
            List <ReportItemStatementBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportItemStatementBEL
                    {
                        ProductName= "Grand Total: ",
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),

                        TotalQtyAddDefi0130 = row["TotalQtyAddDefi0130"].ToString(),
                        TotalQty = row["Total_QTY"].ToString(),

                        GivenQty0108 = row["FRST_WEEK"].ToString(),
                        GivenQty0915 = row["SECND_WEEK"].ToString(),
                        GivenQty1623 = row["THRD_WEEK"].ToString(),
                        GivenQty2431 = row["FRTH_WEEK"].ToString(),

                        TotalStock0108 = row["FRST_WEEK_STK"].ToString(),
                        TotalDCR0108 = row["D0108"].ToString(),
                        TotalStock0915 = row["SECND_WEEK_STK"].ToString(),
                        TotalDCR0915 = row["D0915"].ToString(),
                        TotalStock1623 = row["THRD_WEEK_STK"].ToString(),
                        TotalDCR1623 = row["D1623"].ToString(),
                        TotalStock2431 = row["FRTH_WEEK_STK"].ToString(),
                        TotalDCR2431 = row["D2431"].ToString(),
                        TotalDCR0131 = row["D0131"].ToString(),
                        ClosingStock = row["Closing_STK"].ToString(),

                        d01 = row["D01"].ToString(),
                        d02 = row["D02"].ToString(),
                        d03 = row["D03"].ToString(),
                        d04 = row["D04"].ToString(),
                        d05 = row["D05"].ToString(),
                        d06 = row["D06"].ToString(),
                        d07 = row["D07"].ToString(),
                        d08 = row["D08"].ToString(),
                        d09 = row["D09"].ToString(),
                        d10 = row["D10"].ToString(),
                        d11 = row["D11"].ToString(),
                        d12 = row["D12"].ToString(),
                        d13 = row["D13"].ToString(),
                        d14 = row["D14"].ToString(),
                        d15 = row["D15"].ToString(),
                        d16 = row["D16"].ToString(),
                        d17 = row["D17"].ToString(),
                        d18 = row["D18"].ToString(),
                        d19 = row["D19"].ToString(),
                        d20 = row["D20"].ToString(),
                        d21 = row["D21"].ToString(),
                        d22 = row["D22"].ToString(),
                        d23 = row["D23"].ToString(),
                        d24 = row["D24"].ToString(),
                        d25 = row["D25"].ToString(),
                        d26 = row["D26"].ToString(),
                        d27 = row["D27"].ToString(),
                        d28 = row["D28"].ToString(),
                        d29 = row["D29"].ToString(),
                        d30 = row["D30"].ToString(),
                        d31 = row["D31"].ToString(),


                    }).ToList();
            return item;
        }
        public List<ReportItemStatementDetailLink> GetDetailLink(DefaultParameterBEO model)
        {

            //DateTime.DaysInMonth(1980, 08);
            string[] monthNumber = model.ToDate.Split('-');
            string DayInput = monthNumber[0];
            string MonthInput= monthNumber[1];
            string YearInput = monthNumber[2];

            DayInput = DayInput == "31" ? DateTime.DaysInMonth(Convert.ToInt32(YearInput), Convert.ToInt32(MonthInput)).ToString() : DayInput;

            model.ToDate= DayInput+"-" + MonthInput + "-" + YearInput;

            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate= dateFormat.StringDateDdMonYYYY(model.ToDate);
            string Qry = "Select  TO_Char(A.SET_DATE,'dd-mm-yyyy') SET_DATE,A.MP_GROUP,A.PRODUCT_CODE,REMARKS, " +
                " CASE WHEN ADJUSTMENT_TYPE = 'Gain' THEN GIVEN_QTY * (1) WHEN ADJUSTMENT_TYPE = 'Loss'  THEN GIVEN_QTY * (-1)  END  VARIABLE_QTY,"+
                " B.PRODUCT_NAME||' ('||B.PACK_SIZE||')' PRODUCT_NAME" +
                "  from INV_ITEM_ADJUSTMENT A JOIN  VW_INV_ITEM B ON A.PRODUCT_CODE = B.OP_CODE AND SUBSTR (A.MP_GROUP, INSTR (A.MP_GROUP, 'R1', -1), 5) = B.MARKET_GROUP || B.SBU " +
                " AND  A.SET_DATE between '" + model.FromDate + "' and '" + model.ToDate + "'  and A.PRODUCT_CODE='" + model.ProductCode + "' and A.MP_GROUP='" + model.MPGroup + "' ORDER BY A.SET_DATE ASC ";

           

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportItemStatementDetailLink> item;

            item = (from DataRow row in dt.Rows
                    select new ReportItemStatementDetailLink
                    {
                        MPGroup = row["MP_GROUP"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        Quantity = row["VARIABLE_QTY"].ToString(),
                        Remarks = row["REMARKS"].ToString(),
                    }).ToList();
            return item;
        }






















        public List<ReportItemStatementBEL> GetMainGridDataArchive(DefaultParameterBEO model)
        {

            string Qry = "Select PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,ITEM_TYPE||ITEM_FOR ITEM_TYPE,MP_GROUP,MARKET_NAME, REST_QTY,CENTRAL_QTY,FRST_WEEK,SECND_WEEK,THRD_WEEK, FRTH_WEEK," +
                        " D01, D02, D03, D04, D05, D06, D07, D08, D09, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31," +
                        " FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK TotalQtyAddDefi0130," +
                        " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK  Total_QTY," +
                        " REST_QTY+CENTRAL_QTY+FRST_WEEK FRST_WEEK_STK," +
                        " D01+D02+D03+D04+D05+D06+D07+D08 D0108," +
                        " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08) SECND_WEEK_STK," +
                        " D09+D10+D11+D12+D13+D14+D15 D0915," +
                        " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15) THRD_WEEK_STK," +
                        " D16+D17+D18+D19+D20+D21+D22+D23 D1623," +
                        " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23) FRTH_WEEK_STK," +
                        " D24+D25+D26+D27+D28+D29+D30+D31 D2431," +
                        " (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23+D24+D25+D26+D27+D28+D29+D30+D31) D0131," +
                        " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23+D24+D25+D26+D27+D28+D29+D30+D31) Closing_STK" +
                        " from VW_ARC_INV_SAMPLE_STATEMENT " +
                        "  Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            Qry = Qry + " AND PRODUCT_CODE!='SmRPPM'";
            if (model.ItemType != "All" && model.ItemType != null)
            {
                if (model.ItemType == "SlSmRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Sl','Sm') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "GtRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Gt') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "SlR" || model.ItemType == "SmR" || model.ItemType == "SmI" || model.ItemType == "GtR" || model.ItemType == "GtI")
                {
                    Qry = Qry + " and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }
            }

            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TSM_ID='" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }

            Qry = Qry + " Order by PRODUCT_NAME ";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportItemStatementBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportItemStatementBEL
                    {
                        SL = row["Col1"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),


                        ItemType = row["ITEM_TYPE"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),

                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),

                        //TotalQtyAddDefi0130 =(Convert.ToInt64(row["FRST_WEEK"].ToString())+Convert.ToInt64(row["SECND_WEEK"].ToString())+Convert.ToInt64(row["THRD_WEEK"].ToString())+Convert.ToInt64(row["FRTH_WEEK"].ToString())).ToString() ,
                        //TotalQty =(Convert.ToInt64(row["FRST_WEEK"].ToString())+Convert.ToInt64(row["SECND_WEEK"].ToString())+Convert.ToInt64(row["THRD_WEEK"].ToString())+Convert.ToInt64(row["FRTH_WEEK"].ToString())+Convert.ToInt64(row["Total_QTY"].ToString())).ToString(),
                        TotalQtyAddDefi0130 = row["TotalQtyAddDefi0130"].ToString(),
                        TotalQty = row["Total_QTY"].ToString(),

                        GivenQty0108 = row["FRST_WEEK"].ToString(),
                        GivenQty0915 = row["SECND_WEEK"].ToString(),
                        GivenQty1623 = row["THRD_WEEK"].ToString(),
                        GivenQty2431 = row["FRTH_WEEK"].ToString(),

                        TotalStock0108 = row["FRST_WEEK_STK"].ToString(),
                        TotalDCR0108 = row["D0108"].ToString(),
                        TotalStock0915 = row["SECND_WEEK_STK"].ToString(),
                        TotalDCR0915 = row["D0915"].ToString(),
                        TotalStock1623 = row["THRD_WEEK_STK"].ToString(),
                        TotalDCR1623 = row["D1623"].ToString(),
                        TotalStock2431 = row["FRTH_WEEK_STK"].ToString(),
                        TotalDCR2431 = row["D2431"].ToString(),
                        TotalDCR0131 = row["D0131"].ToString(),
                        ClosingStock = row["Closing_STK"].ToString(),

                        d01 = row["D01"].ToString(),
                        d02 = row["D02"].ToString(),
                        d03 = row["D03"].ToString(),
                        d04 = row["D04"].ToString(),
                        d05 = row["D05"].ToString(),
                        d06 = row["D06"].ToString(),
                        d07 = row["D07"].ToString(),
                        d08 = row["D08"].ToString(),
                        d09 = row["D09"].ToString(),
                        d10 = row["D10"].ToString(),
                        d11 = row["D11"].ToString(),
                        d12 = row["D12"].ToString(),
                        d13 = row["D13"].ToString(),
                        d14 = row["D14"].ToString(),
                        d15 = row["D15"].ToString(),
                        d16 = row["D16"].ToString(),
                        d17 = row["D17"].ToString(),
                        d18 = row["D18"].ToString(),
                        d19 = row["D19"].ToString(),
                        d20 = row["D20"].ToString(),
                        d21 = row["D21"].ToString(),
                        d22 = row["D22"].ToString(),
                        d23 = row["D23"].ToString(),
                        d24 = row["D24"].ToString(),
                        d25 = row["D25"].ToString(),
                        d26 = row["D26"].ToString(),
                        d27 = row["D27"].ToString(),
                        d28 = row["D28"].ToString(),
                        d29 = row["D29"].ToString(),
                        d30 = row["D30"].ToString(),
                        d31 = row["D31"].ToString(),


                    }).ToList();
            return item;
        }



        public Tuple<string, DataTable, List<ReportItemStatementBEL>> ExportArchive(DefaultParameterBEO model)
        {
            string vHeader = "";

            string Qry = " Select PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,ITEM_TYPE||ITEM_FOR ITEM_TYPE,MP_GROUP,MARKET_NAME, REST_QTY,CENTRAL_QTY,FRST_WEEK,SECND_WEEK,THRD_WEEK, FRTH_WEEK," +
     " D01, D02, D03, D04, D05, D06, D07, D08, D09, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31," +
     " FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK TotalQtyAddDefi0130," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK  Total_QTY," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK FRST_WEEK_STK," +
     " D01+D02+D03+D04+D05+D06+D07+D08 D0108," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08) SECND_WEEK_STK," +
     " D09+D10+D11+D12+D13+D14+D15 D0915," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15) THRD_WEEK_STK," +
     " D16+D17+D18+D19+D20+D21+D22+D23 D1623," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23) FRTH_WEEK_STK," +
     " D24+D25+D26+D27+D28+D29+D30+D31 D2431," +
     " (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23+D24+D25+D26+D27+D28+D29+D30+D31) D0131," +
     " REST_QTY+CENTRAL_QTY+FRST_WEEK+SECND_WEEK+THRD_WEEK+FRTH_WEEK - (D01+D02+D03+D04+D05+D06+D07+D08+D09+D10+D11+D12+D13+D14+D15+D16+D17+D18+D19+D20+D21+D22+D23+D24+D25+D26+D27+D28+D29+D30+D31) Closing_STK" +
     " from VW_ARC_INV_SAMPLE_STATEMENT Where Year=" + model.Year + " AND Month_Number='" + model.MonthNumber + "' ";

            Qry = Qry + " AND PRODUCT_CODE!='SmRPPM'";
            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;

            string Item = "All";
            if (model.ItemType != "All" && model.ItemType != null)
            {
                if (model.ItemType == "SlSmRI")
                {
                    Qry = Qry + " AND ITEM_TYPE IN ('Sl','Sm') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "GtRI")
                {
                    Qry = Qry + " AND ITEM_TYPE IN ('Gt') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "SlR" || model.ItemType == "SmR" || model.ItemType == "SmI" || model.ItemType == "GtR" || model.ItemType == "GtI")
                {
                    Qry = Qry + " AND ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }
                Item = model.ItemTypeName;
            }
            vHeader = vHeader + ", Item Type : " + Item;

            Qry += " AND MP_GROUP LIKE '%R1E%'"; //Add for Manual Export
         
             /*
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                Qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                Qry += " AND TSM_ID = '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
            }

            Qry = Qry + " Order by PRODUCT_NAME ";
            */

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);


            List<ReportItemStatementBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportItemStatementBEL
                    {
                        SL = row["Col1"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),

                        ItemType = row["ITEM_TYPE"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),

                        //TotalQtyAddDefi0130 = (Convert.ToInt64(row["FRST_WEEK"].ToString()) + Convert.ToInt64(row["SECND_WEEK"].ToString()) + Convert.ToInt64(row["THRD_WEEK"].ToString()) + Convert.ToInt64(row["FRTH_WEEK"].ToString())).ToString(),
                        //TotalQty = (Convert.ToInt64(row["FRST_WEEK"].ToString()) + Convert.ToInt64(row["SECND_WEEK"].ToString()) + Convert.ToInt64(row["THRD_WEEK"].ToString()) + Convert.ToInt64(row["FRTH_WEEK"].ToString()) + Convert.ToInt64(row["Total_QTY"].ToString())).ToString(),


                        TotalQtyAddDefi0130 = row["TotalQtyAddDefi0130"].ToString(),
                        TotalQty = row["Total_QTY"].ToString(),
                        GivenQty0108 = row["FRST_WEEK"].ToString(),
                        GivenQty0915 = row["SECND_WEEK"].ToString(),
                        GivenQty1623 = row["THRD_WEEK"].ToString(),
                        GivenQty2431 = row["FRTH_WEEK"].ToString(),

                        TotalStock0108 = row["FRST_WEEK_STK"].ToString(),
                        TotalDCR0108 = row["D0108"].ToString(),
                        TotalStock0915 = row["SECND_WEEK_STK"].ToString(),
                        TotalDCR0915 = row["D0915"].ToString(),
                        TotalStock1623 = row["THRD_WEEK_STK"].ToString(),
                        TotalDCR1623 = row["D1623"].ToString(),
                        TotalStock2431 = row["FRTH_WEEK_STK"].ToString(),
                        TotalDCR2431 = row["D2431"].ToString(),
                        TotalDCR0131 = row["D0131"].ToString(),
                        ClosingStock = row["Closing_STK"].ToString(),

                        d01 = row["D01"].ToString(),
                        d02 = row["D02"].ToString(),
                        d03 = row["D03"].ToString(),
                        d04 = row["D04"].ToString(),
                        d05 = row["D05"].ToString(),
                        d06 = row["D06"].ToString(),
                        d07 = row["D07"].ToString(),
                        d08 = row["D08"].ToString(),
                        d09 = row["D09"].ToString(),
                        d10 = row["D10"].ToString(),
                        d11 = row["D11"].ToString(),
                        d12 = row["D12"].ToString(),
                        d13 = row["D13"].ToString(),
                        d14 = row["D14"].ToString(),
                        d15 = row["D15"].ToString(),
                        d16 = row["D16"].ToString(),
                        d17 = row["D17"].ToString(),
                        d18 = row["D18"].ToString(),
                        d19 = row["D19"].ToString(),
                        d20 = row["D20"].ToString(),
                        d21 = row["D21"].ToString(),
                        d22 = row["D22"].ToString(),
                        d23 = row["D23"].ToString(),
                        d24 = row["D24"].ToString(),
                        d25 = row["D25"].ToString(),
                        d26 = row["D26"].ToString(),
                        d27 = row["D27"].ToString(),
                        d28 = row["D28"].ToString(),
                        d29 = row["D29"].ToString(),
                        d30 = row["D30"].ToString(),
                        d31 = row["D31"].ToString(),

                    }).ToList();


       

            return Tuple.Create(vHeader, dt, item);
        }

    }
}