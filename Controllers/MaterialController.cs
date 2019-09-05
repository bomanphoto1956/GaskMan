using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaskMan.Models;

namespace GaskMan.Controllers
{
    public class MaterialController : Controller
    {
        // GET: Material
        public ActionResult Index()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult getRegisteredMaterial()
        {
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            // List<SmManager.gMaterialCL> material = new List<SmManager.gMaterialCL>();
            SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
            SmManager.gMaterialCL[] material = cl.getAllMaterial(ident);
            List<SmManager.gMaterialCL> materialList = material.Cast<SmManager.gMaterialCL>().ToList();
            ViewBag.materialList = materialList;
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }



        [HttpPost]
        public ActionResult selectMaterial(int materialId)
        {
            string errorTxt = "";
            if (Session["ident"] == null)
                RedirectToAction("Login", "Home");
            string ident = Session["ident"].ToString();
            CMaterial model = new CMaterial();            
            if (materialId > 0)
            {
                SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
                SmManager.gMaterialCL[] material = cl.getMaterial(ident, materialId);
                if (material.Length == 1)
                {
                    if (material[0].ErrCode != 0)
                        errorTxt = material[0].ErrMessage;
                    else
                    {
                        model.materialId = material[0].materialId;
                        model.material = material[0].material;
                        model.materialShort = material[0].materialShort;
                    }
                }
                else
                    errorTxt = "Kan ej hitta valt material";
            }
            ViewBag.errorTxt = errorTxt;
            return View(model);
        }

        [HttpPost]
        public JsonResult saveMaterial(CMaterial model)
        {
            string message = "";
            string ErrMessage = "";
            int ErrCode = 0;
            int materialId = 0;
            if (Session["ident"] == null)
                message = "identError";
            if (message == "")
            {
                string ident = Session["ident"].ToString();

                SmManager.gMaterialCL material = new SmManager.gMaterialCL();
                material.materialId = model.materialId;
                material.material = model.material;
                material.materialShort = model.materialShort;
                SmManager.SmManagerClient cl = new SmManager.SmManagerClient();
                material = cl.saveMaterial(ident, material);
                ErrMessage = material.ErrMessage;
                ErrCode = material.ErrCode;
                materialId = material.materialId;
            }
            return Json(new
            {
                message = message,
                errMessage = ErrMessage,
                errCode = ErrCode.ToString(),
                materialId = materialId
            }, JsonRequestBehavior.AllowGet);
        }
    }
}