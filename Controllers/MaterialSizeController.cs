using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaskMan.Models;

namespace GaskMan.Controllers
{
    public class MaterialSizeController : Controller
    {
        // GET: MaterialSize
        public ActionResult MaterialSizeIndex()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult getRegisteredMaterialSize()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.gMaterialSizeCL[] materialSize = cl.getMaterialSize(ident, 0);
            List<SmManager.gMaterialSizeCL> materialSizeList = materialSize.Cast<SmManager.gMaterialSizeCL>().ToList();
            ViewBag.materialSizeList = materialSizeList;
            return View();
        }

        [HttpPost]
        public ActionResult selectMaterialSize(int materialSizeId)
        {
            string errorTxt = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            CMaterialSize model = new CMaterialSize();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            if (materialSizeId > 0)
            {                
                SmManager.gMaterialSizeCL[] materialSize = cl.getMaterialSize(ident, materialSizeId);
                if (materialSize.Length == 1)
                {
                    if (materialSize[0].ErrCode != 0)
                        errorTxt = materialSize[0].ErrMessage;
                    else
                    {
                        model.materialSizeId = materialSize[0].materialSizeId;
                        model.materialId = materialSize[0].materialId;
                        model.description = materialSize[0].description;
                        model.sizeShort = materialSize[0].sizeShort;
                        model.materialLength = materialSize[0].materialLength.ToString();
                        model.materialWidth = materialSize[0].materialWidth.ToString();
                        model.defaultVal = materialSize[0].defaultVal;
                    }
                }
                else
                    errorTxt = "Kan ej hitta vald materialstorlek";                
            }
            // Get all material from db
            SmManager.gMaterialCL[] material = cl.getAllMaterial(ident);
            // Convert to a list
            List<SmManager.gMaterialCL> materialList = material.Cast<SmManager.gMaterialCL>().ToList();
            // Sort that list by material
            materialList = materialList.OrderBy(x => x.material).ToList();
            ViewBag.materialList = new SelectList(materialList, "materialId", "material");
            ViewBag.errorTxt = errorTxt;
            return View(model);
        }

        [HttpPost]
        public JsonResult saveMaterialSize(CMaterialSize model)
        {
            string message = "";
            string ErrMessage = "";
            int ErrCode = 0;
            int materialSizeId = 0;
            if (Session["ident"] == null)
                message = "identError";
            if (message == "")
            {
                string ident = Session["ident"].ToString();
                SmManager.gMaterialSizeCL materialSize = new SmManager.gMaterialSizeCL();
                CMaterialSize ms = new CMaterialSize();
                Decimal materialLength = 0;
                Decimal materialWidth = 0;
                if (!ms.fromStrToDec(model.materialLength, ref materialLength))
                {
                    message = "Materiallängd felaktigt angiven";
                }
                if (message == "")
                {
                    if (!ms.fromStrToDec(model.materialWidth, ref materialWidth))
                        message = "Materialbredd felaktigt angiven";
                }
                if (message == "")
                {
                    materialSize.materialSizeId = model.materialSizeId;
                    materialSize.materialId = model.materialId;
                    materialSize.description = model.description;
                    materialSize.sizeShort = model.sizeShort;
                    materialSize.materialLength = materialLength;
                    materialSize.materialWidth = materialWidth;
                    materialSize.defaultVal = model.defaultVal;
                    SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
                    materialSize = cl.saveMaterialSize(ident, materialSize);
                    ErrMessage = materialSize.ErrMessage;
                    ErrCode = materialSize.ErrCode;
                    materialSizeId = materialSize.materialSizeId;
                }
            }
            return Json(new
            {
                message = message,
                errMessage = ErrMessage,
                errCode = ErrCode.ToString(),
                materialSizeId = materialSizeId
            }, JsonRequestBehavior.AllowGet);
        }       

    }
}