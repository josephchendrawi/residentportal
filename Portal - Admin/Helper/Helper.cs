using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Enums;
using ResComm.Web.Lib.Interface.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web
{
    public class Helper
    {
        public static List<SelectListItem> GetEnumItemList(string EnumName)
        {
            List<SelectListItem> ItemList = new List<SelectListItem>();
            Type enumType = null;
            if (EnumName == "PROPERTY_TYPE")
                enumType = typeof(PROPERTY_TYPE);
            else if (EnumName == "PRIORITY")
                enumType = typeof(PRIORITY);
            else if (EnumName == "TICKET_STATUS")
                enumType = typeof(TICKET_STATUS);
            else if (EnumName == "BILLING_STATUS")
                enumType = typeof(BILLING_STATUS);
            else if (EnumName == "PROPERTY_INVOICE_STATUS")
                enumType = typeof(PROPERTY_INVOICE_STATUS);
            else if (EnumName == "PROPERTY_INVOICE_PAYMENT_METHOD")
                enumType = typeof(PROPERTY_INVOICE_PAYMENT_METHOD);
            else if (EnumName == "PROPERTY_INVOICE_PAYMENT_STATUS")
                enumType = typeof(PROPERTY_INVOICE_PAYMENT_STATUS);

            if (enumType != null)
            {
                foreach (var v in Enum.GetValues(enumType))
                {
                    ItemList.Add(new SelectListItem()
                    {
                        Text = v.ToString(),
                        Value = ((int)v).ToString()
                    });
                }
            }

            return ItemList;
        }

        public static List<SelectListItem> GetTicketCategoryList(long UserId)
        {
            List<SelectListItem> ItemList = new List<SelectListItem>();

            var PropertyId = new AccountBLL().GetRelation(UserId).PropertyId;
            var TotalCount = 0;
            foreach (var v in new TicketBLL().GetTicketCategories(0, int.MaxValue, ref TotalCount, "", "asc", new Lib.Interface.Models.Ticket.TicketCategoryVO { ACCNT_ID = PropertyId }))
            {
                ItemList.Add(new SelectListItem()
                {
                    Text = v.NAME,
                    Value = v.ROW_ID.ToString()
                });
            }

            return ItemList;
        }


    }
}