using ResComm.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ResComm.Web.Controllers
{
    public class CKEditorController : AdminBaseController
    {
        public void UploadFile(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                string ImageName = upload.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/plugins/ckeditor/upload/"), ImageName);

                upload.SaveAs(path);
                
                string CKEditorFuncNum = Request["CKEditorFuncNum"];
                string url = Url.Content("~/Content/plugins/ckeditor/upload/" + ImageName);
                Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
                Response.End();
            }
        }
        public ActionResult BrowseFile()
        {
            var appData = Server.MapPath("~/Content/plugins/ckeditor/upload/");
            List<string> URLs = Directory.GetFiles(appData).Select(x => Url.Content("~/Content/plugins/ckeditor/upload/" + Path.GetFileName(x))).ToList();

            ViewBag.URLs = URLs;

            return View();
        }

    }
}