using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDCR.DAL.Gateway;
using eDCR.Models;
using eDCR.Universal.Gateway;
using Systems.Universal;
using Systems.Controllers;

namespace eDCR.Controllers
{
    public class HomeController : ControllerController
    {
       
        public ActionResult frmHome()
        {  
            if (Session["UserID"] != null)
            {          
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration");
        }
  
    }
       

}