using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaskMan.Models;

namespace GaskMan.Controllers
{
    public class MaterialThicknController : Controller
    {
        // GET: MaterialThickn
        public ActionResult MaterialThicknIndex()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult getRegisteredMaterialThickn()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.gMaterialThicknCL[] materialThickn = cl.getMaterialThickn(ident, 0);
            List<SmManager.gMaterialThicknCL> materialThicknList = materialThickn.Cast<SmManager.gMaterialThicknCL>().ToList();
            ViewBag.materialThicknList = materialThicknList;
            return View();
        }

        [HttpPost]
        public ActionResult selectMaterialThickn(int materialThicknId)
        {
            string errorTxt = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            CMaterialThickn model = new CMaterialThickn();                
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            if (materialThicknId > 0)
            {
                SmManager.gMaterialThicknCL[] materialThickn = cl.getMaterialThickn(ident, materialThicknId);
                if (materialThickn.Length == 1)
                {
                    if (materialThickn[0].ErrCode != 0)
                        errorTxt = materialThickn[0].ErrMessage;
                    else
                    {
                        model.materialThicknId = materialThickn[0].materialThicknId;
                        model.materialSizeId = materialThickn[0].materialSizeId;
                        model.description = materialThickn[0].description;
                        model.thicknShort = materialThickn[0].thicknShort;
                        model.thickness = materialThickn[0].thickness.ToString();
                        model.buyPrice = materialThickn[0].buyPrice.ToString();
                        model.sellPrice = materialThickn[0].sellPrice.ToString();
                        model.cuttingTime = materialThickn[0].cuttingTime.ToString();
                    }
                }
                else
                    errorTxt = "Kan ej hitta vald materialtjocklek";
            }
            // Get all material from db
            SmManager.gMaterialSizeCL[] materialSize = cl.getMaterialSize(ident, 0);
            // Convert to a list
            List<SmManager.gMaterialSizeCL> materialSizeList = materialSize.Cast<SmManager.gMaterialSizeCL>().ToList();
            // Sort that list by material
            materialSizeList = materialSizeList.OrderBy(x => x.materialName).ThenBy(x => x.description).ToList();
            List<CDropDown> ddList = new List<CDropDown>();
            foreach (SmManager.gMaterialSizeCL matSize in materialSizeList)
            {
                CDropDown dd = new CDropDown();
                dd.Id = matSize.materialSizeId;
                dd.Name = matSize.materialName + " " + matSize.description;
                ddList.Add(dd);
            }
            ViewBag.ddList = new SelectList(ddList, "Id", "Name");
            ViewBag.errorTxt = errorTxt;
            return View(model);
        }

        [HttpPost]
        public JsonResult saveMaterialThickn(CMaterialThickn model)
        {
            string message = "";
            string ErrMessage = "";
            int ErrCode = 0;
            int materialThicknId = 0;
            if (Session["ident"] == null)
                message = "identError";
            if (message == "")
            {
                string ident = Session["ident"].ToString();
                SmManager.gMaterialThicknCL materialThickn = new SmManager.gMaterialThicknCL();
                CMaterialSize ms = new CMaterialSize();
                Decimal thickness = 0;
                Decimal buyPrice = 0;
                Decimal sellPrice = 0;
                Decimal cuttingTime = 0;
                if (!ms.fromStrToDec(model.thickness, ref thickness, 0.01M, 100))
                {
                    message = "Materialtjocklek felaktigt angiven";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.buyPrice, ref buyPrice, 0.01M, 100000))
                        message = "Inköpspris felaktigt angivet";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.sellPrice, ref sellPrice, 0.01M, 100000))
                        message = "Försäljningspris felaktigt angivet";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.cuttingTime, ref cuttingTime, 0.01M, 100))
                        message = "Skärtid felaktigt angiven";
                }

                if (message == "")
                {
                    materialThickn.materialThicknId = model.materialThicknId;
                    materialThickn.materialSizeId = model.materialSizeId;
                    materialThickn.description = model.description;
                    materialThickn.thicknShort = model.thicknShort;
                    materialThickn.thickness = thickness;
                    materialThickn.buyPrice = buyPrice;
                    materialThickn.sellPrice = sellPrice;
                    materialThickn.cuttingTime = cuttingTime;
                    SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
                    materialThickn = cl.saveMaterialThickness(ident, materialThickn);
                    ErrMessage = materialThickn.ErrMessage;
                    ErrCode = materialThickn.ErrCode;
                    materialThicknId = materialThickn.materialThicknId;
                }
            }
            return Json(new
            {
                message = message,
                errMessage = ErrMessage,
                errCode = ErrCode.ToString(),
                materialThicknId = materialThicknId
            }, JsonRequestBehavior.AllowGet);
        }       

    }

}
