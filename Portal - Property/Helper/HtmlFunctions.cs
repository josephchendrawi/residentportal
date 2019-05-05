using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;

namespace ResComm.Web
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString DropDownEnumListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> modelExpression, Type enumType, object htmlAttributes = null, bool EnumDescriptionAsValue = true, string optionLabel = null)
        {
            var typeOfProperty = enumType;
            if (!typeOfProperty.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", typeOfProperty));

            List<SelectListItem> enumValues = new List<SelectListItem>();
            foreach (var v in Enum.GetValues(typeOfProperty))
            {
                var text = v.GetType().GetMember(v.ToString()).First().GetCustomAttribute<DescriptionAttribute>();

                if (EnumDescriptionAsValue == true)
                {
                    enumValues.Add(new SelectListItem()
                    {
                        Text = text == null ? v.ToString() : text.Description,
                        Value = text == null ? v.ToString() : text.Description,
                    });
                }
                else
                {
                    enumValues.Add(new SelectListItem()
                    {
                        Text = text == null ? v.ToString() : text.Description,
                        Value = ((int)v).ToString(),
                    });
                }
            }

            return htmlHelper.DropDownListFor(modelExpression, enumValues, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString DropDownEnumList(this HtmlHelper htmlHelper, string name, Type enumType, object htmlAttributes = null, bool EnumDescriptionAsValue = true, string optionLabel = null)
        {
            var typeOfProperty = enumType;
            if (!typeOfProperty.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", typeOfProperty));

            List<SelectListItem> enumValues = new List<SelectListItem>();
            foreach (var v in Enum.GetValues(typeOfProperty))
            {
                var text = v.GetType().GetMember(v.ToString()).First().GetCustomAttribute<DescriptionAttribute>();

                if (EnumDescriptionAsValue == true)
                {
                    enumValues.Add(new SelectListItem()
                    {
                        Text = text == null ? v.ToString() : text.Description,
                        Value = text == null ? v.ToString() : text.Description,
                    });
                }
                else
                {
                    enumValues.Add(new SelectListItem()
                    {
                        Text = text == null ? v.ToString() : text.Description,
                        Value = ((int)v).ToString(),
                    });
                }
            }

            return htmlHelper.DropDownList(name, enumValues, optionLabel, htmlAttributes);
        }

    }
}