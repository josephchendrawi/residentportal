using NLog;
using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Models.Billing;
using ResComm.Web.Lib.Interface.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Scheduler.BillCompile
{
    class Program
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private static BillingPaymentBLL BillingPaymentBLL = new BillingPaymentBLL();
        private static PropertyBLL PropertyBLL = new PropertyBLL();
        private static BillingCompileBLL BillingCompileBLL = new BillingCompileBLL();

        static void Main(string[] args)
        {
            Logger.Info("Task ran.");

            try
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (DateTime.Now.Day == 15)
                {
                    FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
                }
                else if(DateTime.Now.Day == DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month)) // == LastDay of the Month
                {
                    FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 16);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                }

                if (FromDate != null && ToDate != null)
                {
                    var TotalCount = 0;
                    var PropertyList = PropertyBLL.GetAll(0, int.MaxValue, ref TotalCount, "", "", new PropertyVO());

                    foreach (var v in PropertyList)
                    {
                        decimal TotalPaymentCompiled = BillingPaymentBLL.GetAll(v.ROW_ID, FromDate, ToDate.Value.AddDays(1)).Sum(m => m.AMOUNT) ?? 0;

                        BillingCompileBLL.CreateBillingCompile(new BillingCompileVO()
                        {
                            AMOUNT = TotalPaymentCompiled,
                            PROPERTY_ID = v.ROW_ID,
                            DESCRIPTION = "Scheduled Billing Compiling Task on Property : " + v.NAME + " with Total Compiled Amount : " + TotalPaymentCompiled.ToString("f0")
                                            + ", From : " + FromDate.Value.ToString("dd MMM yyyy") + " To : " + ToDate.Value.ToString("dd MMM yyyy")
                        });
                    }
                }                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
