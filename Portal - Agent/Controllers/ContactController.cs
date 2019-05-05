using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Models.Affiliate;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class ContactController : AffiliateBaseController
    {
        AffiliateContactDALImpl ContactBLL = new AffiliateContactDALImpl();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListContact(DTParameterModel param, string Name, string PhoneNo)
        {
            try
            {
                
                var UserId = long.Parse((string)Session["UserId"]);
                var CustomerId = AccountBLL.GetRelation(UserId).CustomerId;

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                AffiliateContactVO qFilter = new AffiliateContactVO()
                {
                    NAME = Name,
                    PHONE_NO = PhoneNo
                };
                var List = ContactBLL.GetAffiliateContacts(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Contact> ContactList = new List<Contact>();

                foreach (var v in List)
                {
                    Contact VM = new Contact();
                    VM.ContactId = v.ROW_ID;
                    VM.Name = v.NAME;
                    VM.Email = v.EMAIL;
                    VM.PhoneNo = v.PHONE_NO;
                    VM.Rating = v.RATING ?? 0;
                    VM.Remarks = v.REMARKS;

                    ContactList.Add(VM);
                }

                DTResult<Contact> model = new DTResult<Contact>
                {
                    draw = param.Draw,
                    data = ContactList,
                    recordsFiltered = TotalCount,
                    recordsTotal = TotalCount
                };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public ActionResult Add()
        {
            Contact model = new Contact();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Contact Model)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    var CustomerId = AccountBLL.GetRelation(UserId).CustomerId;

                    var AffiliateContactVO = new AffiliateContactVO();

                    AffiliateContactVO.AFFILIATE_ID = UserId;
                    AffiliateContactVO.NAME = Model.Name;
                    AffiliateContactVO.EMAIL = Model.Email;
                    AffiliateContactVO.PHONE_NO = Model.PhoneNo;
                    AffiliateContactVO.RATING = Model.Rating;
                    AffiliateContactVO.REMARKS = Model.Remarks;

                    AffiliateContactVO.CREATED_BY = UserId;

                    var result = ContactBLL.CreateAffiliateContact(AffiliateContactVO);

                    if (result != 0)
                    {
                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("List", "Contact");
                    }
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

            return View(Model);
        }

        public ActionResult Edit(int Id)
        {
            Contact model = new Contact();

            var v = ContactBLL.GetAffiliateContact(Id);

            model.ContactId = v.ROW_ID;
            model.Name = v.NAME;
            model.Email = v.EMAIL;
            model.PhoneNo = v.PHONE_NO;
            model.Rating = v.RATING ?? 0;
            model.Remarks = v.REMARKS;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Contact Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);

                    var AffiliateContactVO = ContactBLL.GetAffiliateContact(Model.ContactId);
                    AffiliateContactVO.NAME = Model.Name;
                    AffiliateContactVO.EMAIL = Model.Email;
                    AffiliateContactVO.PHONE_NO = Model.PhoneNo;
                    AffiliateContactVO.RATING = Model.Rating;
                    AffiliateContactVO.REMARKS = Model.Remarks;

                    AffiliateContactVO.LAST_UPDATED_BY = UserId;

                    ContactBLL.UpdateAffiliateContact(AffiliateContactVO);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("Edit", "Contact", new { Id = Model.ContactId });
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

            return View(Model);
        }

    }
}