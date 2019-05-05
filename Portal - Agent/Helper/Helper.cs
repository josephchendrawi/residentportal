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
    }
}