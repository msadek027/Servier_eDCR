using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace eDCR.Universal.Common
{
    public class Terminal
    {
        public String GetLanIPAddress()
        {
            //The X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a
            //client connecting to a web server through an HTTP proxy or load balancer
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            return ip;


            //string clientMachineName = (Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName);
            //return clientMachineName;

        }

    }
}