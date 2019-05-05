namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_PROPERTY_SUBSCRIPTION
    {
        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? PROPERTY_ID { get; set; }

        public long? SUBSCRIPTION_PACKAGE_ID { get; set; }

        public int? STATUS { get; set; }

        public DateTime? START_DATE { get; set; }

        public DateTime? END_DATE { get; set; }

        public long? SUBSCRIPTION_ORDER_ID { get; set; }

        public virtual P_PROPERTY P_PROPERTY { get; set; }

        public virtual P_SUBSCRIPTION_PACKAGE P_SUBSCRIPTION_PACKAGE { get; set; }

        public virtual P_PROPERTY_SUBSCRIPTION_ORDER P_PROPERTY_SUBSCRIPTION_ORDER { get; set; }
    }
}
