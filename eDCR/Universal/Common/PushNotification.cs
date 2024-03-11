using eDCR.DAL.Gateway;
using eDCR.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace eDCR.Universal.Common
{
    public class PushNotification
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.CurrentCulture);



      

        public bool SaveToDatabase(string MPGroup,string OperationType,string Title,string DetailMsg,string Year,string MonthNumber)
        {
            bool isTrue = false;
            string SL = idGenerated.getMAXSL("Push_Notification", "MST_SL").ToString();
            string QryNotice = "Insert Into Push_Notification(MST_SL, MP_GROUP, OP_TYPE, TITLE, MESSAGE, SET_DATE,YEAR,MONTH_NUMBER,SET_LOC_CODE,TERMINAL) " +
                " Values(" + SL + ",'" + MPGroup + "','" + OperationType + "','" + Title + "','" + DetailMsg + "',TO_Date('" + CntDate + "','dd-mm-yyyy HH24:MI:SS')," + Year + ",'" + MonthNumber + "','" + HttpContext.Current.Session["LocCode"].ToString() + "','Web' )";
            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryNotice))
            {
                isTrue = true;
            }
            return isTrue;
        }
        public bool SaveToBillDatabase(string MPGroup, string OperationType, string Title, string DetailMsg, string Year, string MonthNumber,string MiscManualTADABill)
        {
            bool isTrue = false;
            string SL = idGenerated.getMAXSL("Push_Notification", "MST_SL").ToString();
            string QryNotice = "Insert Into Push_Notification(MST_SL, MP_GROUP, OP_TYPE, TITLE, MESSAGE, SET_DATE,YEAR,MONTH_NUMBER,SET_LOC_CODE,TERMINAL,BILL_AMT) " +
                " Values(" + SL + ",'" + MPGroup + "','" + OperationType + "','" + Title + "','" + DetailMsg + "',TO_Date('" + CntDate + "','dd-mm-yyyy HH24:MI:SS')," + Year + ",'" + MonthNumber + "','" + HttpContext.Current.Session["LocCode"].ToString() + "','Web',"+MiscManualTADABill+" )";
            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryNotice))
            {
                isTrue = true;
            }
            return isTrue;
        }
        public Tuple<Boolean, string> SinglePushNotification(string deviceId, string tag, string Title, string DetailMsg)
        {

               bool isTrue = false;          
                string str = "";            
                var applicationID = "AAAAHC2Ukso:APA91bG_oTTIdvFDrpeWOpqLEwAOYyq26Rl7iyxcxAttyCmG5hf-pIDoJcsdcMECPWcdPNsaqcngcSRS_AsXzOR12KNVKTMm8Oocve4rvaORXO65uyQMxftJk-1CqXkeS5G08B1p-Zyl";//Authorization Key


                //var applicationID = "AAAAHC2Ukso:APA91bG_oTTIdvFDrpeWOpqLEwAOYyq26Rl7iyxcxAttyCmG5hf-pIDoJcsdcMECPWcdPNsaqcngcSRS_AsXzOR12KNVKTMm8Oocve4rvaORXO65uyQMxftJk-1CqXkeS5G08B1p-Zyl";
                var senderId = "121023795914";//Sender ID
                //string deviceId = "cRrSJPBrRcc:APA91bFf8Ak6eirwrYlfh4iHhyZXOHE8j_QyDjldXZf5kS15VJqYylVOTzYWhUe48RUfkHg53IIEO-YRjZwJgb7diioINcW6v2KwyabXXwoiN6i3Xz_IY2BJkONxsGk3HczVj9w-ygD4"; //Change able
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    //notification = new
                    //{
                    //    body = "Okay",
                    //    title = "Ok",
                    //    icon = "myicon"
                    //}   
                    notification = new
                    {
                        title = Title,
                        body = DetailMsg,
                        tag = tag,
                        count = 0,
                        datetime = CntDate
                    },
                    data = new
                    {
                        Tag = tag,
                        Title = Title,
                        Detail = DetailMsg,
                        Count = 0,
                        Datetime = CntDate
                    }
                };
      
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                 str = sResponseFromServer;
                            }
                        }
                    }
                }
                return Tuple.Create(isTrue, str);
        }


        public Tuple<Boolean, string> PushNotification3(string[] deviceId)
        {
            bool isTrue = false;

            string str = "";
           var applicationID = "AIzaSyAPiiSKcOQekLLZb46Gs3QR9ZKb3CaytiY";//Authorization Key
           // var applicationID = "AAAAHC2Ukso:APA91bG_oTTIdvFDrpeWOpqLEwAOYyq26Rl7iyxcxAttyCmG5hf-pIDoJcsdcMECPWcdPNsaqcngcSRS_AsXzOR12KNVKTMm8Oocve4rvaORXO65uyQMxftJk-1CqXkeS5G08B1p-Zyl";//Authorization Key
            var senderId = "23977311302";//Sender ID
            //string deviceId = "cRrSJPBrRcc:APA91bFf8Ak6eirwrYlfh4iHhyZXOHE8j_QyDjldXZf5kS15VJqYylVOTzYWhUe48RUfkHg53IIEO-YRjZwJgb7diioINcW6v2KwyabXXwoiN6i3Xz_IY2BJkONxsGk3HczVj9w-ygD4"; //Change able
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var data = new
            {
                to = deviceId,
                notification = new
                {
                    body = "Okay",
                    title = "Ok",
                    icon = "myicon"
                }
            };

            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
            tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {

                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                            str = sResponseFromServer;
                        }
                    }
                }
            }


            return Tuple.Create(isTrue, str);
        }
    
      public bool IsConnectedToInternet()
        {

            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }



    }
}