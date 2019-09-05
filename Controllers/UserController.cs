using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaskMan.Models;

namespace GaskMan.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserIndex()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            return View();
        }


        [HttpPost]
        public ActionResult getRegisteredUsers()
        {
            string error = "";            
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            SmServ.SmServClient servCl = new SmServ.SmServClient();
            SmServ.ReparatorCL[] reparators = servCl.getReparators(ident);
            List<SmServ.ReparatorCL> userList = reparators.Cast<SmServ.ReparatorCL>().ToList();
            userList = userList.Where(u => u.gasketLevel > 0).ToList();            
            ViewBag.userList = userList;
            ViewBag.errorTxt = error;
            return View();
        }

        [HttpPost]
        public ActionResult selectUser(string userId)
        {
            string errorTxt = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            CUser model = new CUser();
            model.AnvID = "";
            SmServ.SmServClient servCl = new SmServ.SmServClient();
            if (userId != "")
            {
                SmServ.ReparatorCL[] reparator = servCl.gGetReparators(ident, userId);
                if (reparator.Length == 1)
                    if (reparator[0].ErrCode != 0)
                        errorTxt = reparator[0].ErrMessage;
                    else
                    {
                        model.AnvID = reparator[0].AnvID;
                        model.Reparator = reparator[0].Reparator;
                        model.gasketLevel = reparator[0].gasketLevel;
                    }

                else
                    errorTxt = "Kan ej hitta vald användare";
            }
            else
            {
                // Get all anvandare from db
                SmServ.ReparatorCL[] reparators = servCl.getReparators(ident);
                List<SmServ.ReparatorCL> reparatorList = reparators.Cast<SmServ.ReparatorCL>().ToList();
                reparatorList = reparatorList.Where(x => x.gasketLevel == 0).ToList();
                reparatorList = reparatorList.OrderBy(x => x.Reparator).ToList();
                ViewBag.reparatorList = new SelectList(reparatorList, "AnvID", "Reparator");
            }
            KeyValuePair<int, string>[] access = servCl.gGetGasketLevels(ident);
            List<KeyValuePair<int, string>> accessList = access.Cast<KeyValuePair<int, string>>().ToList();
            //List<CDropDown> accessDd = new List<CDropDown>();
            ViewBag.access = new SelectList(accessList, "Key", "Value");
            ViewBag.errorTxt = errorTxt;
            return View(model);
        }


        [HttpPost]
        public JsonResult saveAccessLevel(CUser model)
        {
            string message = "";
            string ErrMessage = "";
            int ErrCode = 0;
            string AnvID = "";
            if (Session["ident"] == null)
                message = "identError";            
            if (message == "")
            {
                string ident = Session["ident"].ToString();
                SmServ.ReparatorCL rep = new SmServ.ReparatorCL();
                rep.AnvID = model.AnvID;
                rep.gasketLevel = model.gasketLevel;
                SmServ.SmServClient servCl = new SmServ.SmServClient();
                rep = servCl.saveGasketLevel(ident, rep);
                ErrMessage = rep.ErrMessage;
                ErrCode = rep.ErrCode;
                AnvID = rep.AnvID;
            }
            return Json(new
            {
                message = message,
                errMessage = ErrMessage,
                errCode = ErrCode.ToString(),
                AnvID = AnvID
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
