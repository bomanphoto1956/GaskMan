using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaskMan.Models;

namespace GaskMan.Controllers
{
    public class ReuseMatController : Controller
    {
        // GET: ReuseMat
        public ActionResult ReuseMatIndex()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            return View();
        }


        [HttpPost]
        public ActionResult getRegisteredReuseMat()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.gReuseMatCL[] rm = cl.getReuseMaterial(ident, 0);
            List<SmManager.gReuseMatCL> rmList = rm.Cast<SmManager.gReuseMatCL>().ToList();
            ViewBag.rmList = rmList;
            return View();
        }


        [HttpPost]
        public ActionResult selectReuseMat(int reuseMatId)
        {
            string errorTxt = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            CReuseMat model = new CReuseMat();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            if (reuseMatId > 0)
            {
                SmManager.gReuseMatCL[] reuseMatList = cl.getReuseMaterial(ident, reuseMatId);
                if (reuseMatList.Length == 1)
                {
                    if (reuseMatList[0].ErrCode != 0)
                        errorTxt = reuseMatList[0].ErrMessage;
                    else
                    {
                        model.reuseMatId = reuseMatList[0].reuseMatId;
                        model.minDiam = reuseMatList[0].minDiam.ToString();
                        model.reusePercentage = reuseMatList[0].reusePercentage.ToString();
                    }
                }
                else
                    errorTxt = "Kan ej hitta vald post";
            }
            ViewBag.errorTxt = errorTxt;
            return View(model);
        }


        [HttpPost]
        public JsonResult saveReuseMat(CReuseMat model)
        {
            string message = "";
            string ErrMessage = "";
            int ErrCode = 0;
            int reuseMatId = 0;
            if (Session["ident"] == null)
                message = "identError";
            if (message == "")
            {
                string ident = Session["ident"].ToString();
                SmManager.gReuseMatCL reuseMat = new SmManager.gReuseMatCL();
                CMaterialSize ms = new CMaterialSize();
                Decimal minDiam = 0;
                Decimal reusePercentage = 0;
                if (!ms.fromStrToDec(model.minDiam, ref minDiam, 0.01M, 15000))
                {
                    message = "Min diameter felaktigt angiven";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.reusePercentage, ref reusePercentage, 0, 99))
                        message = "Återanvändbar procent felaktigt angiven";
                }
                if (message == "")
                {
                    reuseMat.reuseMatId = model.reuseMatId;
                    reuseMat.minDiam = minDiam;
                    reuseMat.reusePercentage = reusePercentage;
                    SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
                    reuseMat = cl.saveReuseMaterial(ident, reuseMat);
                    ErrMessage = reuseMat.ErrMessage;
                    ErrCode = reuseMat.ErrCode;
                    reuseMatId = reuseMat.reuseMatId;
                }
            }
            return Json(new
            {
                message,
                errMessage = ErrMessage,
                errCode = ErrCode.ToString(),
                reuseMatId
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalcReusableDef(Decimal innerDiam)
        {
            string message = "";
            Decimal reusablePercentage = 0;
            if (Session["ident"] == null)
                message = "identError";
            if (message == "")
            {
                string ident = Session["ident"].ToString();
                SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
                SmManager.gReuseMatCL reuseMat = cl.getReusablePercentage(ident, innerDiam);
                if (reuseMat.ErrCode != 0)
                    message = reuseMat.ErrMessage;
                else
                {
                    reusablePercentage = reuseMat.reusePercentage;
                }

            }
            return Json(new
            {
                message,
                reusableMaterial = reusablePercentage
            }, JsonRequestBehavior.AllowGet);

        }



    }
}