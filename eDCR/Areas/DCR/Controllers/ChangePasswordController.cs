using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class ChangePasswordController : Controller
    {
        //
        // GET: /DCR/ChangePassword/
        public ActionResult frmChangePassword()
        {
            string EmpTypeSession = Session["EmpType"].ToString();

            if (Session["UserID"] != null)
            {

                //if (EmpTypeSession == "Manager" || EmpTypeSession == "DSM" || EmpTypeSession == "EMA")
                //{
                    return View();
               // }

            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
            // return RedirectToAction("Index", "LoginRegistration");
        }
	}
}