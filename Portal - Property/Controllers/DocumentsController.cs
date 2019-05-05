using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Models.AccountNote;
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
    public class DocumentsController : MemberBaseController
    {
        DocumentBLL DocumentBLL = new DocumentBLL();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListDocument(DTParameterModel param)
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
                AccountNoteVO qFilter = new AccountNoteVO()
                {
                    ///
                };
                var List = DocumentBLL.GetDocumentsByCustomerId(CustomerId, param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Document> DocumentList = new List<Document>();

                foreach (var v in List)
                {
                    Document VM = new Document();
                    VM.AccountNoteId = v.ROW_ID;
                    VM.Name = v.NAME;
                    VM.Content = Regex.Replace(v.NOTE.Substring(0, v.NOTE.IndexOf("\r\n")), "<.*?>", string.Empty);
                    VM.LastUpdatedText = (v.LAST_UPD ?? v.CREATED.Value).ToMalaysiaTime().ToString("dd MMM yyyy");
                    VM.CreatedBy = v.CreatedBy;
                    VM.LastUpdatedBy = v.LastUpdatedBy;

                    DocumentList.Add(VM);
                }

                DTResult<Document> model = new DTResult<Document>
                {
                    draw = param.Draw,
                    data = DocumentList,
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

        public ActionResult View(int Id)
        {
            Document model = new Document();

            var v = DocumentBLL.Get(Id);

            model.AccountNoteId = v.ROW_ID;
            model.Name = v.NAME;
            model.Content = v.NOTE;
            model.LastUpdatedText = (v.LAST_UPD ?? v.CREATED.Value).ToMalaysiaTime().ToString("dd MMM yyyy HH:mm:ss AA");

            return View(model);
        }


    }
}