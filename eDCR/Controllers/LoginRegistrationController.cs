using eDCR.DAL.DAO;
using eDCR.DAL.Gateway;
using eDCR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Systems.Universal;

namespace eDCR.Controllers
{
    public class LoginRegistrationController : Controller
    {
        LoginRegistrationDAO loginRegistrationDAO = new LoginRegistrationDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        Encryption encryption = new Encryption();

        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon(); // it will clear the session at the end of request


          //  loginRegistrationDAO.UserLoginLOG(Session["EmpID"].ToString(), Session["RoleID"].ToString(),"Out");

            //return RedirectToAction("Index", "LoginRegistration");
            return View();
        }


        [HttpPost]
        public ActionResult CheckUser(LoginRegistrationModel master)
        {
            try
            {
                string uid = encryption.Encrypt(master.UserID);
                string pwd = encryption.Encrypt(master.Password);
                if ((master.UserID != null) && (master.Password != null))
                {
                   var obj = loginRegistrationDAO.CheckUserCredential().Where(m => m.UserID.Equals(encryption.Encrypt(master.UserID)) && m.Password.Equals(encryption.Encrypt(master.Password))).FirstOrDefault();

                    if (obj != null)
                    {
                        string FilePath = WebConfigurationManager.AppSettings["FilePath"].ToString();
                        Session["FilePath"] = FilePath;


                        Session["UserID"] = obj.UserID;
                        Session["Password"] = master.Password;
                        Session["enUserID"] = obj.UserID;
                        Session["deUserID"] = encryption.Decrypt(obj.UserID);
                        Session["RoleID"] = obj.RoleID;
                        Session["RoleName"] = obj.RoleName;
                        Session["EmpID"] = obj.EmpID;
                        Session["EmpName"] = obj.EmpName;
                        Session["Designation"] = obj.EmpID == "7187" ? "Sr. Executive" : obj.Designation;
                        Session["EmpType"] = obj.Designation;
                        Session["LocName"] = obj.LocName;
                        Session["LocCode"] = obj.LocCode;

                        loginRegistrationDAO.UserLoginLOG(obj.EmpID, obj.UserID, encryption.Encrypt(master.Password), obj.RoleID, "In");
                        loginRegistrationDAO.GetSubordinateFrmUser(obj.EmpID, obj.LocCode);


                        bool IsTrue = loginRegistrationDAO.MenuPopulate(obj.UserID);

                        Session["IsLogged"] = IsTrue;
                        return RedirectToAction("frmHome", "Home");
                        //return Json(new { Status = "success" });
                    }
                    else
                    { 
                        TempData["msg"] = "<div id=\"msgDiv\" class=\"alert alert-warning\">Incorrect userid or password. Type the correct userid and password, and try again!</div>";
                        return RedirectToAction("Index", "LoginRegistration");
                    }

                }

            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
            //  return Json(new { Status = "failed" });



         

            //return RedirectToAction()
            //return RedirectToAction("Index", "LoginRegistration", "<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>");
            //Content("<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>");
            // return RedirectToAction("Index", "LoginRegistration");
            //TempData["msg"] = "Record Saved Successfully.";

            return RedirectToAction("Index", "LoginRegistration");

          
        }


        //--------------------
        //public static bool IsYourLoginStillTrue(string userId, string sid)
        //{
        //    CapWorxQuikCapContext context = new CapWorxQuikCapContext();

        //    IEnumerable<Logins> logins = (from i in context.Logins
        //                                  where i.LoggedIn == true &&
        //                                  i.UserId == userId && i.SessionId == sid
        //                                  select i).AsEnumerable();
        //    return logins.Any();
        //}

        //public static bool IsUserLoggedOnElsewhere(string userId, string sid)
        //{
        //    CapWorxQuikCapContext context = new CapWorxQuikCapContext();

        //    IEnumerable<Logins> logins = (from i in context.Logins
        //                                  where i.LoggedIn == true &&
        //                                  i.UserId == userId && i.SessionId != sid
        //                                  select i).AsEnumerable();
        //    return logins.Any();
        //}

        //public static void LogEveryoneElseOut(string userId, string sid)
        //{
        //    CapWorxQuikCapContext context = new CapWorxQuikCapContext();

        //    IEnumerable<Logins> logins = (from i in context.Logins
        //                                  where i.LoggedIn == true &&
        //                                  i.UserId == userId &&
        //                                  i.SessionId != sid // need to filter by user ID
        //                                  select i).AsEnumerable();

        //    foreach (Logins item in logins)
        //    {
        //        item.LoggedIn = false;
        //    }

        //    context.SaveChanges();
        //}













    }
}
