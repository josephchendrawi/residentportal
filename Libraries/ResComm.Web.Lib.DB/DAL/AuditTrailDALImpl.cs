#region Using
using AutoMapper;
using ResComm.Web.Lib.Interface.Common.Constants;
using ResComm.Web.Lib.DB.EF;
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
    public class AuditTrailDALImpl : ResCommDALAbstract, IAuditTrailDAL
    {
        IMapper Mapper;
        public AuditTrailDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_AUDIT_TRAIL, AuditTrailVO>();
                cfg.CreateMap<AuditTrailVO, P_AUDIT_TRAIL>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region GetDefaultData
        public override BaseVO GetDefaultData(long rowId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region InsertAuditTrail
        public void InsertAuditTrail(AuditTrailVO auditTrailVO)
        {
            auditTrailVO.CREATED = DateTime.UtcNow;
            using (var DBContext = new PropComDbContext())
            {
                var note = Mapper.Map<AuditTrailVO, P_AUDIT_TRAIL>(auditTrailVO);

                DBContext.P_AUDIT_TRAIL.Add(note);
                DBContext.SaveChanges();
            }
        }

        public void InsertAuditLog(AuditLogVO auditLogVO)
        {
            auditLogVO.CREATED = DateTime.UtcNow;
            using (var DBContext = new PropComDbContext())
            {
                var note = Mapper.Map<AuditLogVO, P_AUDIT_LOG>(auditLogVO);

                DBContext.P_AUDIT_LOG.Add(note);
                DBContext.SaveChanges();
            }
        }
        #endregion

        #region GetAuditTrail
        public List<AuditTrailVO> GetAuditTrail(string moduleName, long auditRowId)
        {
            string displayModule = ConvertAuditTrailModule(moduleName);
            using (var DBContext = new PropComDbContext())
            {
                var k = from a in DBContext.P_AUDIT_TRAIL
                        join b in DBContext.P_USER on a.CREATED_BY equals b.ROW_ID
                        where a.MODULE_NAME == moduleName && a.AUDIT_ROW_ID == auditRowId
                        orderby a.ROW_ID descending
                        select new { a, b.USERNAME };

                var listOfResult = new List<AuditTrailVO>();
                foreach (var z in k)
                {
                    var auditTrailVO = Mapper.Map<P_AUDIT_TRAIL, AuditTrailVO>(z.a);
                    auditTrailVO.CreatedByUserName = z.USERNAME;
                    auditTrailVO.MODULE_NAME = displayModule;
                    listOfResult.Add(auditTrailVO);
                }

                return listOfResult;
            }
        }

        private string ConvertAuditTrailModule(string input)
        {
            string result = string.Empty;
            if (input.Equals(ResCommConstants.MODULE_ORDER, StringComparison.Ordinal))
            {
                result = "Order";
            }
            else if (input.Equals(ResCommConstants.MODULE_ORDER_ITEMS, StringComparison.Ordinal))
            {
                result = "Order Item";
            }
            else if (input.Equals(ResCommConstants.MODULE_ORDER_ACTIVITIES, StringComparison.Ordinal))
            {
                result = "Order Activity";
            }
            else if (input.Equals(ResCommConstants.MODULE_ACCOUNT, StringComparison.Ordinal))
            {
                result = "Account";
            }
            return result;
        }

        #endregion
    }
}
