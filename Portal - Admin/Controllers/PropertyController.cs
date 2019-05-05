using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Models.Unit;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class PropertyController : AdminBaseController
    {
        UnitBLL UnitBLL = new UnitBLL();
        AccountBLL AccountBLL = new AccountBLL();
        PropertyBLL PropertyBLL = new PropertyBLL();

        public ActionResult Details()
        {
            PropertyInfo model = new PropertyInfo();

            
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            var PropertyVO = PropertyBLL.Get(PropertyId);

            model.PropertyId = PropertyId;
            model.PropertyName = PropertyVO.NAME;
            model.Address1 = PropertyVO.ADDR_1;
            model.Address2 = PropertyVO.ADDR_2;
            model.City = PropertyVO.CITY;
            model.State = PropertyVO.STATE;
            model.ZIP = PropertyVO.POSTAL_CD;
            model.Country = PropertyVO.COUNTRY;
            model.LogoURL = PropertyVO.LOGO_URL;

            return View(model);
        }

        [HttpPost]
        public ActionResult Details(PropertyInfo model, HttpPostedFileBase file, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

                    //create/update property
                    var PropertyVO = PropertyBLL.Get(PropertyId);
                    PropertyVO.NAME = model.PropertyName;
                    PropertyVO.ADDR_1 = model.Address1;
                    PropertyVO.ADDR_2 = model.Address2;
                    PropertyVO.CITY = model.City;
                    PropertyVO.STATE = model.State;
                    PropertyVO.POSTAL_CD = model.ZIP;
                    PropertyVO.COUNTRY = model.Country;
                    PropertyVO.LAST_UPD_BY = UserId;
                    PropertyBLL.UpdateProperty(PropertyVO);

                    if (file != null)
                    {
                        var FilePath = FileHelper.SaveFile(file, DateTime.Now.ToString("yyyyMMddHHmmss"));

                        PropertyVO.LOGO_URL = FilePath;
                        PropertyBLL.UpdateProperty(PropertyVO);
                    }

                    TempData["Message"] = "Successfully done.";

                    return RedirectToAction("Details", "Property");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }
            else
            {
                TempData["Message"] = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            return View(model);
        }

    }
}