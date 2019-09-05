using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaskMan.Models;

namespace GaskMan.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginUser(CLogin model)
        {
            SmManager.LoginAdm loginAdm = new SmManager.LoginAdm();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            loginAdm.AnvID = model.AnvID;
            loginAdm.pwd = model.pwd;
            try
            {
                loginAdm = cl.GLogin(loginAdm);
            }
            catch (Exception ex)
            {
                loginAdm.ErrMessage = "can not reach loginadmin. Message : " + ex.Message;
            }
            if (loginAdm.ErrMessage == "")
            {
                Session["AnvID"] = loginAdm.AnvID;
                Session["reparator"] = loginAdm.reparator;
                Session["ident"] = loginAdm.ident;
                Session["accessLevel"] = loginAdm.gasketLevel;
            }
            return Json(new
            {                
                errorMessage = loginAdm.ErrMessage,

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }



        public ActionResult Index()
        {
            if (Session["AnvID"] == null)
                return View("Login");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}