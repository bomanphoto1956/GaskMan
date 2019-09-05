using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaskMan.Models;

namespace GaskMan.Controllers
{
    public class CostController : Controller
    {
        // GET: Cost
        public ActionResult CostIndex()
        {
            string errorTxt = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            CCost cc = new CCost();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.gWorkingCostCL wCost = cl.getWorkingCosts(ident);
            if (wCost.ErrCode == 0)
            {
                cc.workingCostId = wCost.workingCostId;
                cc.cuttingHourNet = wCost.cuttingHourNet.ToString();
                cc.cuttingHourSales = wCost.cuttingHourSales.ToString();
                cc.handlingHourNet = wCost.handlingHourNet.ToString();
                cc.handlingHourSales = wCost.handlingHourSales.ToString();
                cc.cuttingMargin = wCost.cuttingMargin.ToString();
            }
            else if (wCost.ErrCode != -100)
            {
                errorTxt = wCost.ErrMessage;
            }
            ViewBag.errorTxt = errorTxt;
            return View(cc);        
            //asdfasdf
        }

        [HttpPost]
        public JsonResult saveCost(CCost model)
        {
            string message = "";
            string ErrMessage = "";
            int ErrCode = 0;
            int workingCostId = 0;
            if (Session["ident"] == null)
                message = "identErr";            
            if (message == "")
            {
                string ident = Session["ident"].ToString();

                SmManager.gWorkingCostCL workingCost = new SmManager.gWorkingCostCL();
                CMaterialSize ms = new CMaterialSize();
                Decimal cuttingHourNet = 0;
                Decimal cuttingHourSales = 0;
                Decimal handlingHourNet = 0;
                Decimal handlingHourSales = 0;
                Decimal cuttingMargin = 0;
                if (!ms.fromStrToDec(model.cuttingHourNet, ref cuttingHourNet, 0.01M, 100000))
                {
                    message = "Timdebitering skärtid netto utanför intervall";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.cuttingHourSales, ref cuttingHourSales, 0.01M, 100000))
                        message = "Timdebitering skärtid brutto utanför intervall";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.handlingHourNet, ref handlingHourNet, 0, 100000))
                        message = "Timdebitering hantering netto utanför intervall";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.handlingHourSales, ref handlingHourSales, 0, 100000))
                        message = "Timdebitering hantering brutto utanför intervall";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.cuttingMargin, ref cuttingMargin, 0, 1000))
                        message = "Skärmarginal utanför intervall";
                }
                if (message == "")
                {
                    workingCost.workingCostId = model.workingCostId;
                    workingCost.cuttingHourNet = cuttingHourNet;
                    workingCost.cuttingHourSales = cuttingHourSales;
                    workingCost.handlingHourNet = handlingHourNet;
                    workingCost.handlingHourSales = handlingHourSales;
                    workingCost.cuttingMargin = cuttingMargin;
                    SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
                    workingCost = cl.saveWorkingCost(ident, workingCost);
                    ErrMessage = workingCost.ErrMessage;
                    ErrCode = workingCost.ErrCode;
                    workingCostId = workingCost.workingCostId;

                }
            }
            return Json(new
            {
                message = message,
                errMessage = ErrMessage,
                errCode = ErrCode.ToString(),
                workingCostId = workingCostId
            }, JsonRequestBehavior.AllowGet);

        }

    }
}