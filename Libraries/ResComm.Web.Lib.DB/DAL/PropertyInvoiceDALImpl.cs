using AutoMapper;
using ResComm.Web.Lib.Interface.Common.Constants;
using ResComm.Web.Lib.DB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResComm.Web.Lib.Interface.Common.Enums;
using System.Data.Entity;
using ResComm.Web.Lib.Interface.Models.PropertyInvoice;

namespace ResComm.Web.Lib.DB.DAL
{
    public class PropertyInvoiceDALImpl
    {
        IMapper Mapper;
        public PropertyInvoiceDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_PROPERTY_INVOICE, PropertyInvoiceVO>();
                cfg.CreateMap<PropertyInvoiceVO, P_PROPERTY_INVOICE>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<PropertyInvoiceVO> MapReturnList(List<P_PROPERTY_INVOICE> aList)
        {
            // map
            var returnedResult = new List<PropertyInvoiceVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_PROPERTY_INVOICE, PropertyInvoiceVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get PropertyInvoice
        public PropertyInvoiceVO GetPropertyInvoice(long PropertyInvoiceId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_PROPERTY_INVOICE
                             where d.ROW_ID == PropertyInvoiceId
                             select d;

                if (result.Count() > 0)
                {
                    var P_PROPERTY_INVOICE = result.First();
                    var PropertyInvoiceVO = Mapper.Map<P_PROPERTY_INVOICE, PropertyInvoiceVO>(P_PROPERTY_INVOICE);
                    PropertyInvoiceVO.PropertyName = P_PROPERTY_INVOICE.P_PROPERTY.NAME;

                    return PropertyInvoiceVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<PropertyInvoiceVO> GetPropertyInvoices(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", PropertyInvoiceVO qFilter = null, DateTime? InvoiceDateStart = null, DateTime? InvoiceDateEnd = null)
        {
            List<PropertyInvoiceVO> result = new List<PropertyInvoiceVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_PROPERTY_INVOICE
                          select d;

                //filtering
                if (qFilter.PROPERTY_ID != null && qFilter.PROPERTY_ID != 0)
                {
                    ett = ett.Where(m => m.PROPERTY_ID == qFilter.PROPERTY_ID);
                }
                if (qFilter.PropertyName != null && qFilter.PropertyName != "")
                {
                    ett = ett.Where(m => m.P_PROPERTY.NAME == qFilter.PropertyName);
                }
                if (qFilter.STATUS != null && qFilter.STATUS != 0)
                {
                    ett = ett.Where(m => m.STATUS == qFilter.STATUS);
                }
                if (qFilter.PAYMENT_STATUS != null && qFilter.PAYMENT_STATUS != 0)
                {
                    ett = ett.Where(m => m.PAYMENT_STATUS == qFilter.PAYMENT_STATUS);
                }
                if (qFilter.INVOICE_NUM != null && qFilter.INVOICE_NUM != "")
                    ett = ett.Where(m => m.INVOICE_NUM.ToLower().Contains(qFilter.INVOICE_NUM.ToLower()));
                if (qFilter.CREATED != null)
                {
                    DateTime date = qFilter.CREATED.Value;
                    ett = ett.Where(m => m.CREATED != null);
                    ett = ett.Where(m => m.CREATED.Value.Year == date.Year && m.CREATED.Value.Month == date.Month && m.CREATED.Value.Day == date.Day);
                }
                if (InvoiceDateStart != null && InvoiceDateEnd != null)
                {
                    InvoiceDateEnd = InvoiceDateEnd.Value.AddDays(1);
                    ett = ett.Where(m => m.INVOICE_DATE != null
                        && m.INVOICE_DATE >= InvoiceDateStart && m.INVOICE_DATE < InvoiceDateEnd
                    );
                }
                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "PropertyName")
                        ett = ett.OrderBy(m => m.P_PROPERTY.NAME);
                    else if (orderBy == "InvoiceDate")
                        ett = ett.OrderBy(m => m.INVOICE_DATE);
                    else if (orderBy == "InvoiceNumber")
                        ett = ett.OrderBy(m => m.INVOICE_NUM);
                    else if (orderBy == "Amount")
                        ett = ett.OrderBy(m => m.AMOUNT);
                    else if (orderBy == "Status")
                        ett = ett.OrderBy(m => m.STATUS);
                    else if (orderBy == "PaymentMethod")
                        ett = ett.OrderBy(m => m.PAYMENT_METHOD);
                    else if (orderBy == "PaymentStatus")
                        ett = ett.OrderBy(m => (m.PAYMENT_STATUS));
                    else if (orderBy == "Description")
                        ett = ett.OrderBy(m => (m.DESCRIPTION));
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "PropertyName")
                        ett = ett.OrderByDescending(m => m.P_PROPERTY.NAME);
                    else if (orderBy == "InvoiceDate")
                        ett = ett.OrderByDescending(m => m.INVOICE_DATE);
                    else if (orderBy == "InvoiceNumber")
                        ett = ett.OrderByDescending(m => m.INVOICE_NUM);
                    else if (orderBy == "Amount")
                        ett = ett.OrderByDescending(m => m.AMOUNT);
                    else if (orderBy == "Status")
                        ett = ett.OrderByDescending(m => m.STATUS);
                    else if (orderBy == "PaymentMethod")
                        ett = ett.OrderByDescending(m => m.PAYMENT_METHOD);
                    else if (orderBy == "PaymentStatus")
                        ett = ett.OrderByDescending(m => (m.PAYMENT_STATUS));
                    else if (orderBy == "Description")
                        ett = ett.OrderByDescending(m => (m.DESCRIPTION));
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    var order = Mapper.Map<P_PROPERTY_INVOICE, PropertyInvoiceVO>(v);
                    order.PropertyName = v.P_PROPERTY.NAME;

                    result.Add(order);
                }
            }

            return result;
        }

        #endregion

        #region Create New PropertyInvoice
        public long CreatePropertyInvoice(PropertyInvoiceVO _newPropertyInvoice, long ByUserId)
        {
            using (var db = new PropComDbContext())
            {
                var ett = from d in db.P_PROPERTY_INVOICE
                          where d.CREATED != null && d.CREATED.Value.Month == DateTime.UtcNow.Month && d.CREATED.Value.Year == DateTime.UtcNow.Year
                          select d;
                var InvoiceThisMonthCount = ett.Count();

                P_PROPERTY_INVOICE P_PROPERTY_INVOICE = Mapper.Map<PropertyInvoiceVO, P_PROPERTY_INVOICE>(_newPropertyInvoice);
                P_PROPERTY_INVOICE.CREATED = DateTime.UtcNow;
                P_PROPERTY_INVOICE.CREATED_BY = ByUserId;
                db.P_PROPERTY_INVOICE.Add(P_PROPERTY_INVOICE);
                db.SaveChanges();

                P_PROPERTY_INVOICE.INVOICE_NUM = "RP-" + DateTime.UtcNow.ToString("yyyyMM") + "-" + (InvoiceThisMonthCount + 1).ToString().PadLeft(5, '0');
                db.SaveChanges();                

                return P_PROPERTY_INVOICE.ROW_ID;
            }
        }
        #endregion

        #region Update PropertyInvoice
        public void UpdatePropertyInvoice(PropertyInvoiceVO _updatedPropertyInvoice, long ByUserId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_PROPERTY_INVOICE = Mapper.Map<PropertyInvoiceVO, P_PROPERTY_INVOICE>(_updatedPropertyInvoice);
                P_PROPERTY_INVOICE.LAST_UPDATED = DateTime.UtcNow;
                P_PROPERTY_INVOICE.LAST_UPDATED_BY = ByUserId;

                DBContext.P_PROPERTY_INVOICE.Attach(P_PROPERTY_INVOICE);
                DBContext.Entry(P_PROPERTY_INVOICE).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
