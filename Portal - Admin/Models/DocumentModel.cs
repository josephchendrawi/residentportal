using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Models
{
    public class Document
    {
        public long AccountNoteId { get; set; }
        [Required]
        [AllowHtml]
        public string Content { get; set; }
        [Display(Name = "Subject")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Last Updated")]
        public string LastUpdatedText { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Updated By")]
        public string LastUpdatedBy { get; set; }
    }
}