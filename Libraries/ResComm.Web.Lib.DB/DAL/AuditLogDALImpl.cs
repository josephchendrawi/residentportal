#region Using
using ResComm.Web.Lib.Interface.Models.Audit;
using ResComm.Web.Lib.Interface.Models;
using ResComm.Web.Lib.Interface.Models.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion
namespace ResComm.Web.Lib.DB.DAL
{
    public class AuditLogDALImpl : ResCommDALAbstract, IAuditLogDAL
    {
        #region MyRegion
        public void InsertAuditLog(AuditLogVO auditLogVO)
        {
            
        }
        
        #endregion


        #region GetDefaultData
        public override BaseVO GetDefaultData(long rowId)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
