using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaskMan.Models;

namespace GaskMan.Controllers
{
    public class GasketController : Controller
    {
        // GET: Gasket
        public ActionResult GasketIndex()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult getRegisteredGasket()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.gGasketCL[] gaskets = cl.getGasket(ident, 0);
            List<SmManager.gGasketCL> gasketList = gaskets.Cast<SmManager.gGasketCL>().ToList();
            ViewBag.gasketTest = "";
            ViewBag.gasketList = gasketList;
            return View();
        }


        [HttpPost]
        public ActionResult selectGasket(int gasketId)
        {
            string errorTxt = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            CGasket model = new CGasket();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.gWorkingCostCL workingCost = cl.getWorkingCosts(ident);
            model.cuttingMargin = workingCost.cuttingMargin.ToString();
            model.price = "0";
            model.Type2SecHoleCount = 0;
            model.Type2SecHoleDiam = "0";
            //model.Type3GasketLength = "0";
            //model.Type3GasketWidth = "0";
            //string saveInnerDiam = "0";
            if (gasketId > 0)
            {
                SmManager.gGasketCL[] gasket = cl.getGasket(ident, gasketId);
                if (gasket.Length == 1)
                {
                    if (gasket[0].ErrCode != 0)
                        errorTxt = gasket[0].ErrMessage;
                    else
                    {
                        model.gasketId = gasket[0].gasketId;
                        model.gasketTypeId = gasket[0].gasketTypeId;
                        model.materialThicknId = gasket[0].materialThicknId;
                        model.outerDiam = gasket[0].outerDiam.ToString();
                        model.innerDiam = gasket[0].innerDiam.ToString();
                        //saveInnerDiam = gasket[0].innerDiam.ToString();
                        model.reusableMaterial = gasket[0].reusableMaterial.ToString();
                        model.cuttingMargin = gasket[0].cuttingMargin.ToString();
                        model.standardPriceProduct = gasket[0].standardPriceProduct;
                        model.handlingTime = gasket[0].handlingTime.ToString();
                        model.Type2SecHoleCount = gasket[0].Type2SecHoleCount;
                        model.Type2SecHoleDiam = gasket[0].Type2SecHoleCount.ToString();
                        model.Type3GasketLength = gasket[0].Type3GasketLength.ToString();
                        model.Type3GasketWidth = gasket[0].Type3GasketWidth.ToString();
                        model.price = gasket[0].price.ToString();
                        model.note = gasket[0].note;
                        model.description = gasket[0].description;
                        model.materialCostMm2 = gasket[0].materialCostMm2;
                        model.materialMarginPercent = gasket[0].materialMarginPercent;
                        model.cuttingLengthOuterMm = gasket[0].cuttingLengthOuterMm;
                        model.cuttingLengthInnerMm = gasket[0].cuttingLengthInnerMm;
                        model.cuttingSpeedMSek = gasket[0].cuttingSpeedMmSek;
                        model.materialArea = gasket[0].materialArea;
                    }
                }
                else
                    errorTxt = "Kan ej hitta vald packning";
            }
            // Get all material from db
            SmManager.gMaterialThicknCL[] materialThickns = cl.getMaterialThickn(ident, 0);
            List<SmManager.gMaterialThicknCL> materialThicknsList = materialThickns.Cast<SmManager.gMaterialThicknCL>().ToList();
            materialThicknsList = materialThicknsList.OrderBy(x => x.materialName).ThenBy(x => x.materialSize).ThenBy(x => x.description).ToList();
            List<CDropDown> ddList = new List<CDropDown>();
            foreach (SmManager.gMaterialThicknCL matThickn in materialThicknsList)
            {
                CDropDown dd = new CDropDown();
                dd.Id = matThickn.materialThicknId;
                dd.Name = matThickn.materialName + " " + matThickn.materialSize + " " + matThickn.description;
                ddList.Add(dd);
            }
            ViewBag.ddList = new SelectList(ddList, "Id", "Name");
            ViewBag.errorTxt = errorTxt;
            ViewBag.workingCost = workingCost;
            // 2018-08-29 KJBO
            KeyValuePair<int, string>[] gasketTypeL = cl.getGasketType(ident);
            List<KeyValuePair<int, string>> gasketTypeList = gasketTypeL.Cast<KeyValuePair<int, string>>().ToList();
            ViewBag.gasketTypeList = new SelectList(gasketTypeList, "Key", "Value");
            return View(model);
        }


        [HttpPost]
        public ActionResult getGasketCalc(int gasketId)
        {
            string errorTxt = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            CGasket model = new CGasket();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.gWorkingCostCL workingCost = cl.getWorkingCosts(ident);
            if (gasketId > 0)
            {
                SmManager.gGasketCL[] gasket = cl.getGasket(ident, gasketId);
                ViewBag.gasket = gasket[0];
            }
            // Get all material from db
            ViewBag.errorTxt = errorTxt;
            ViewBag.workingCost = workingCost;

            return View();
        }





        [HttpPost]
        public JsonResult saveGasket(CGasket model)
        {
            string message = "";
            string ErrMessage = "";
            int ErrCode = 0;
            int gasketId = 0;
            int gasketTypeId = 0;
            if (Session["ident"] == null)
                message = "identError";
            if (message == "")
            {
                string ident = Session["ident"].ToString();

                SmManager.gGasketCL gasket = new SmManager.gGasketCL();
                CMaterialSize ms = new CMaterialSize();
                Decimal outerDiam = 0;
                Decimal innerDiam = 0;
                Decimal reusableMaterial = 0;
                Decimal cuttingMargin = 0;
                Decimal handlingTime = 0;
                Decimal price = 0;
                Decimal Type2SecHoleDiam = 0;
                // 2019-09-05
                Decimal Type3GasketLength = 0;
                Decimal Type3GasketWidth = 0;

                if (model.price == "")
                    model.price = "0";


                if (model.gasketTypeId == 1 || model.gasketTypeId == 2)
                {
                    if (!ms.fromStrToDec(model.outerDiam, ref outerDiam, 0.01M, 100000))
                    {
                        message = "Ytterdiameter måste vara större än 0 (och mindre än 100000)";
                    }
                }

                if (message == "")
                {
                    if (!ms.fromStrToDec(model.innerDiam, ref innerDiam, 0.01M, 100000))
                    {
                        message = "Innerdiameter måste vara större än 0 (och mindre än 100000)";
                    }
                }

                if (message == "" && (model.gasketTypeId == 1 || model.gasketTypeId == 2))
                {
                    if (innerDiam >= outerDiam)
                        message = "Innerdiameter måste vara mindre än ytterdiameter";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.reusableMaterial, ref reusableMaterial, 0, 95))
                    {
                        message = "Återanvändbart material bör vara mellan 0 och 95%";
                    }

                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.cuttingMargin, ref cuttingMargin, 0, 100))
                    {
                        message = "Felaktig skärmarginal";
                    }

                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.handlingTime, ref handlingTime, 0, 100000))
                    {
                        message = "Felaktig hanteringstid";
                    }
                }

                if (message == "")
                {
                    if (!ms.fromStrToDec(model.price, ref price, 0, 1000000))
                    {
                        message = "Felaktigt pris";
                    }
                }
                if (message == "")
                {
                    if (model.gasketTypeId ==2 && model.Type2SecHoleCount == 0)
                    {
                        message = "Antal yttre hål måste vara större än 0";
                    }
                }
                if (message == "")
                {
                    if (model.gasketTypeId == 2 || (model.gasketTypeId == 3 && model.Type2SecHoleCount > 0))
                    {
                        if (!ms.fromStrToDec(model.Type2SecHoleDiam, ref Type2SecHoleDiam, 0.01M, 100000))
                        {
                            message = "Felaktigt angiven diameter på yttre hål";
                        }
                    }
                }
                if (message == "")
                {
                    if (model.gasketTypeId == 3)
                    {
                        if (!ms.fromStrToDec(model.Type3GasketLength, ref Type3GasketLength, 0.01M, 100000))
                        {
                            message = "Packningens längd måste vara större än 0 (och mindre än 100000)";
                        }
                        if (!ms.fromStrToDec(model.Type3GasketWidth, ref Type3GasketWidth, 0.01M, 100000))
                        {
                            message = "Packningens bredd måste vara större än 0 (och mindre än 100000)";
                        }
                    }
                }

                if (message == "" && model.gasketTypeId == 3)
                {
                    if (Type3GasketLength <= innerDiam)
                        message = "Packningens längd måste vara större än innerdiametern";
                }
                if (message == "" && model.gasketTypeId == 3)
                {
                    if (Type3GasketWidth <= innerDiam)
                        message = "Packningens bredd måste vara större än innerdiametern";
                }

                if (message == "")
                {
                    gasket.gasketId = model.gasketId;
                    gasket.gasketTypeId = model.gasketTypeId;
                    gasket.materialThicknId = model.materialThicknId;
                    gasket.outerDiam = outerDiam;
                    gasket.innerDiam = innerDiam;
                    gasket.reusableMaterial = reusableMaterial;
                    gasket.cuttingMargin = cuttingMargin;
                    gasket.standardPriceProduct = model.standardPriceProduct;
                    gasket.handlingTime = handlingTime;
                    gasket.Type2SecHoleCount = model.Type2SecHoleCount;
                    gasket.Type2SecHoleDiam = Type2SecHoleDiam;
                    // 2019-09-05 KJBO
                    if (model.gasketTypeId == 3)
                    {
                        gasket.Type3GasketLength = Type3GasketLength;
                        gasket.Type3GasketWidth = Type3GasketWidth;
                    }
                    gasket.price = price;
                    gasket.note = model.note;
                    gasket.description = model.description;
                    SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
                    gasket = cl.saveGasket(ident, gasket);
                    ErrMessage = gasket.ErrMessage;
                    ErrCode = gasket.ErrCode;
                    gasketId = gasket.gasketId;
                    gasketTypeId = gasket.gasketTypeId;
                }
            }
            return Json(new
            {
                message = message,
                errMessage = ErrMessage,
                errCode = ErrCode.ToString(),
                gasketId = gasketId,
                gasketTypeId = gasketTypeId
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteGasket(int gasketId)
        {
            string message = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.ErrorCL err = cl.deleteGasket(ident, gasketId);
            return Json(new
            {
                message = message,
                errMessage = err.ErrMessage,
                errCode = err.ErrCode.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

    }
}