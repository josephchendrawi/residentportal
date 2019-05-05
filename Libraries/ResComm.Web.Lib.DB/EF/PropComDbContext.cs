namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PropComDbContext : DbContext
    {
        public PropComDbContext()
            : base("name=PropComDbContext")
        {
        }

        public virtual DbSet<P_ACCNT> P_ACCNT { get; set; }
        public virtual DbSet<P_ACCNT_ACT> P_ACCNT_ACT { get; set; }
        public virtual DbSet<P_ACCNT_ATT> P_ACCNT_ATT { get; set; }
        public virtual DbSet<P_ACCNT_NOTE> P_ACCNT_NOTE { get; set; }
        public virtual DbSet<P_ADDR> P_ADDR { get; set; }
        public virtual DbSet<P_AFFLIATE_LEADS> P_AFFLIATE_LEADS { get; set; }
        public virtual DbSet<P_ASSET> P_ASSET { get; set; }
        public virtual DbSet<P_ASSET_CON> P_ASSET_CON { get; set; }
        public virtual DbSet<P_ASSET_OM> P_ASSET_OM { get; set; }
        public virtual DbSet<P_AUDIT_LOG> P_AUDIT_LOG { get; set; }
        public virtual DbSet<P_AUDIT_TRAIL> P_AUDIT_TRAIL { get; set; }
        public virtual DbSet<P_BILLING> P_BILLING { get; set; }
        public virtual DbSet<P_BILLING_COMPILE> P_BILLING_COMPILE { get; set; }
        public virtual DbSet<P_BILLING_PAYMENT> P_BILLING_PAYMENT { get; set; }
        public virtual DbSet<P_CLIENT_MAP> P_CLIENT_MAP { get; set; }
        public virtual DbSet<P_CON_ACCNT> P_CON_ACCNT { get; set; }
        public virtual DbSet<P_CON_ADDR> P_CON_ADDR { get; set; }
        public virtual DbSet<P_CONTACT> P_CONTACT { get; set; }
        public virtual DbSet<P_DEMAND_LST> P_DEMAND_LST { get; set; }
        public virtual DbSet<P_EMAIL_LOG> P_EMAIL_LOG { get; set; }
        public virtual DbSet<P_FACILITY> P_FACILITY { get; set; }
        public virtual DbSet<P_FACILITY_SLOT> P_FACILITY_SLOT { get; set; }
        public virtual DbSet<P_FACILITY_SLOT_BOOKING> P_FACILITY_SLOT_BOOKING { get; set; }
        public virtual DbSet<P_GROUP> P_GROUP { get; set; }
        public virtual DbSet<P_GROUP_ACCESS> P_GROUP_ACCESS { get; set; }
        public virtual DbSet<P_LKP_GENERAL> P_LKP_GENERAL { get; set; }
        public virtual DbSet<P_MODULE> P_MODULE { get; set; }
        public virtual DbSet<P_ORDER> P_ORDER { get; set; }
        public virtual DbSet<P_ORDER_ACT> P_ORDER_ACT { get; set; }
        public virtual DbSet<P_ORDER_ATT> P_ORDER_ATT { get; set; }
        public virtual DbSet<P_ORDER_ITEM> P_ORDER_ITEM { get; set; }
        public virtual DbSet<P_ORDER_ITEM_OM> P_ORDER_ITEM_OM { get; set; }
        public virtual DbSet<P_ORDER_NOTE> P_ORDER_NOTE { get; set; }
        public virtual DbSet<P_PROD> P_PROD { get; set; }
        public virtual DbSet<P_PROD_ATTR> P_PROD_ATTR { get; set; }
        public virtual DbSet<P_PROD_ISP> P_PROD_ISP { get; set; }
        public virtual DbSet<P_PROD_ITEM> P_PROD_ITEM { get; set; }
        public virtual DbSet<P_PROD_OM> P_PROD_OM { get; set; }
        public virtual DbSet<P_PROPERTY> P_PROPERTY { get; set; }
        public virtual DbSet<P_PROPERTY_INVOICE> P_PROPERTY_INVOICE { get; set; }
        public virtual DbSet<P_PROPERTY_SUBSCRIPTION> P_PROPERTY_SUBSCRIPTION { get; set; }
        public virtual DbSet<P_PROPERTY_SUBSCRIPTION_ORDER> P_PROPERTY_SUBSCRIPTION_ORDER { get; set; }
        public virtual DbSet<P_SRV_ACT> P_SRV_ACT { get; set; }
        public virtual DbSet<P_SRV_ATT> P_SRV_ATT { get; set; }
        public virtual DbSet<P_SRV_CATEGORY> P_SRV_CATEGORY { get; set; }
        public virtual DbSet<P_SRV_KEYVAL> P_SRV_KEYVAL { get; set; }
        public virtual DbSet<P_SRV_NOTE> P_SRV_NOTE { get; set; }
        public virtual DbSet<P_SRV_TIC> P_SRV_TIC { get; set; }
        public virtual DbSet<P_SUBSCRIPTION_PACKAGE> P_SUBSCRIPTION_PACKAGE { get; set; }
        public virtual DbSet<P_UNIT> P_UNIT { get; set; }
        public virtual DbSet<P_UNIT_TYPE> P_UNIT_TYPE { get; set; }
        public virtual DbSet<P_USER> P_USER { get; set; }
        public virtual DbSet<P_USER_COMMISSION> P_USER_COMMISSION { get; set; }
        public virtual DbSet<P_USER_GROUP> P_USER_GROUP { get; set; }
        public virtual DbSet<P_USER_GROUP_DEPT> P_USER_GROUP_DEPT { get; set; }
        public virtual DbSet<P_USER_PAYOUT> P_USER_PAYOUT { get; set; }
        public virtual DbSet<P_USER_TRX> P_USER_TRX { get; set; }
        public virtual DbSet<P_VAL_LST> P_VAL_LST { get; set; }
        public virtual DbSet<P_VAL_MST> P_VAL_MST { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<P_ACCNT>()
                .Property(e => e.BANK_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<P_ACCNT>()
                .HasMany(e => e.P_FACILITY)
                .WithOptional(e => e.P_ACCNT)
                .HasForeignKey(e => e.ACCNT_ID);

            modelBuilder.Entity<P_ACCNT>()
                .HasMany(e => e.P_PROPERTY)
                .WithOptional(e => e.P_ACCNT)
                .HasForeignKey(e => e.ACCNT_ID);

            modelBuilder.Entity<P_ACCNT>()
                .HasMany(e => e.P_SRV_CATEGORY)
                .WithOptional(e => e.P_ACCNT)
                .HasForeignKey(e => e.ACCNT_ID);

            modelBuilder.Entity<P_ACCNT>()
                .HasMany(e => e.P_USER)
                .WithOptional(e => e.P_ACCNT)
                .HasForeignKey(e => e.ACCNT_ID);

            modelBuilder.Entity<P_ADDR>()
                .HasMany(e => e.P_ACCNT)
                .WithOptional(e => e.P_ADDR)
                .HasForeignKey(e => e.ADDR_ID);

            modelBuilder.Entity<P_BILLING>()
                .Property(e => e.AMOUNT)
                .HasPrecision(18, 0);

            modelBuilder.Entity<P_BILLING>()
                .HasMany(e => e.P_BILLING_PAYMENT)
                .WithOptional(e => e.P_BILLING)
                .HasForeignKey(e => e.BILLING_ID);

            modelBuilder.Entity<P_BILLING_COMPILE>()
                .Property(e => e.AMOUNT)
                .HasPrecision(18, 0);

            modelBuilder.Entity<P_BILLING_PAYMENT>()
                .Property(e => e.AMOUNT)
                .HasPrecision(18, 0);

            modelBuilder.Entity<P_FACILITY>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<P_FACILITY>()
                .HasMany(e => e.P_FACILITY_SLOT)
                .WithOptional(e => e.P_FACILITY)
                .HasForeignKey(e => e.FACILITY_ID);

            modelBuilder.Entity<P_FACILITY_SLOT>()
                .HasMany(e => e.P_FACILITY_SLOT_BOOKING)
                .WithOptional(e => e.P_FACILITY_SLOT)
                .HasForeignKey(e => e.FACILITY_SLOT_ID);

            modelBuilder.Entity<P_GROUP>()
                .Property(e => e.ACTIVE_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_LKP_GENERAL>()
                .Property(e => e.ACTIVE_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_ORDER>()
                .Property(e => e.CUST_REP_ID)
                .IsUnicode(false);

            modelBuilder.Entity<P_ORDER_ITEM>()
                .Property(e => e.ORDER_ASSET_NUM)
                .IsUnicode(false);

            modelBuilder.Entity<P_ORDER_ITEM>()
                .Property(e => e.CIP_PRICE)
                .HasPrecision(19, 4);

            modelBuilder.Entity<P_ORDER_ITEM>()
                .Property(e => e.ETC_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_ORDER_ITEM>()
                .Property(e => e.OTC_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<P_ORDER_NOTE>()
                .Property(e => e.NOTE)
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.PRD_PRICE)
                .HasPrecision(19, 4);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.ACTIVE_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.VIS_ASSET)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.VIS_INVOICE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.VAS_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.SVC_ID_REQ)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.EQ_ID_REQ)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.PWD_REQ)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.PROVI_REQ)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD>()
                .Property(e => e.OTC_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD_ITEM>()
                .Property(e => e.ACTIVE_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROD_ITEM>()
                .Property(e => e.REQ_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_PROPERTY>()
                .HasMany(e => e.P_BILLING_COMPILE)
                .WithOptional(e => e.P_PROPERTY)
                .HasForeignKey(e => e.PROPERTY_ID);

            modelBuilder.Entity<P_PROPERTY>()
                .HasMany(e => e.P_PROPERTY_INVOICE)
                .WithOptional(e => e.P_PROPERTY)
                .HasForeignKey(e => e.PROPERTY_ID);

            modelBuilder.Entity<P_PROPERTY>()
                .HasMany(e => e.P_PROPERTY_SUBSCRIPTION)
                .WithOptional(e => e.P_PROPERTY)
                .HasForeignKey(e => e.PROPERTY_ID);

            modelBuilder.Entity<P_PROPERTY>()
                .HasMany(e => e.P_PROPERTY_SUBSCRIPTION_ORDER)
                .WithOptional(e => e.P_PROPERTY)
                .HasForeignKey(e => e.PROPERTY_ID);

            modelBuilder.Entity<P_PROPERTY>()
                .HasMany(e => e.P_UNIT_TYPE)
                .WithOptional(e => e.P_PROPERTY)
                .HasForeignKey(e => e.PROPERTY_ID);

            modelBuilder.Entity<P_PROPERTY_INVOICE>()
                .Property(e => e.AMOUNT)
                .HasPrecision(18, 0);

            modelBuilder.Entity<P_PROPERTY_SUBSCRIPTION_ORDER>()
                .Property(e => e.PRICE)
                .HasPrecision(18, 0);

            modelBuilder.Entity<P_PROPERTY_SUBSCRIPTION_ORDER>()
                .HasMany(e => e.P_PROPERTY_SUBSCRIPTION)
                .WithOptional(e => e.P_PROPERTY_SUBSCRIPTION_ORDER)
                .HasForeignKey(e => e.SUBSCRIPTION_ORDER_ID);

            modelBuilder.Entity<P_SRV_CATEGORY>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<P_SRV_CATEGORY>()
                .HasMany(e => e.P_SRV_TIC)
                .WithOptional(e => e.P_SRV_CATEGORY)
                .HasForeignKey(e => e.CATEGORY_ID);

            modelBuilder.Entity<P_SRV_KEYVAL>()
                .Property(e => e.KV_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<P_SRV_TIC>()
                .Property(e => e.DISPUTE_AMT)
                .HasPrecision(19, 4);

            modelBuilder.Entity<P_SRV_TIC>()
                .Property(e => e.CUST_COMP_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_SUBSCRIPTION_PACKAGE>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<P_SUBSCRIPTION_PACKAGE>()
                .Property(e => e.PRICE)
                .HasPrecision(18, 0);

            modelBuilder.Entity<P_SUBSCRIPTION_PACKAGE>()
                .HasMany(e => e.P_PROPERTY_SUBSCRIPTION)
                .WithOptional(e => e.P_SUBSCRIPTION_PACKAGE)
                .HasForeignKey(e => e.SUBSCRIPTION_PACKAGE_ID);

            modelBuilder.Entity<P_SUBSCRIPTION_PACKAGE>()
                .HasMany(e => e.P_PROPERTY_SUBSCRIPTION_ORDER)
                .WithOptional(e => e.P_SUBSCRIPTION_PACKAGE)
                .HasForeignKey(e => e.SUBSCRIPTION_PACKAGE_ID);

            modelBuilder.Entity<P_UNIT>()
                .Property(e => e.UNIT_NO)
                .IsUnicode(false);

            modelBuilder.Entity<P_UNIT>()
                .HasMany(e => e.P_BILLING)
                .WithOptional(e => e.P_UNIT)
                .HasForeignKey(e => e.UNIT_ID);

            modelBuilder.Entity<P_UNIT_TYPE>()
                .HasMany(e => e.P_UNIT)
                .WithOptional(e => e.P_UNIT_TYPE)
                .HasForeignKey(e => e.UNIT_TYPE_ID);

            modelBuilder.Entity<P_USER>()
                .Property(e => e.ACTIVE_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_USER>()
                .HasMany(e => e.P_AFFLIATE_LEADS)
                .WithOptional(e => e.P_USER)
                .HasForeignKey(e => e.AFFILIATE_ID);

            modelBuilder.Entity<P_USER>()
                .HasMany(e => e.P_BILLING_PAYMENT)
                .WithOptional(e => e.P_USER)
                .HasForeignKey(e => e.PAID_BY);

            modelBuilder.Entity<P_USER>()
                .HasMany(e => e.P_FACILITY_SLOT_BOOKING)
                .WithOptional(e => e.P_USER)
                .HasForeignKey(e => e.USER_ID);

            modelBuilder.Entity<P_USER>()
                .HasMany(e => e.P_SRV_TIC)
                .WithOptional(e => e.P_USER)
                .HasForeignKey(e => e.CASE_OWNER_ID);

            modelBuilder.Entity<P_USER>()
                .HasMany(e => e.P_UNIT)
                .WithOptional(e => e.P_USER)
                .HasForeignKey(e => e.OWNER_ID);

            modelBuilder.Entity<P_USER>()
                .HasMany(e => e.P_UNIT1)
                .WithOptional(e => e.P_USER1)
                .HasForeignKey(e => e.TENANT_ID);

            modelBuilder.Entity<P_VAL_LST>()
                .Property(e => e.ACTIVE_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<P_VAL_MST>()
                .Property(e => e.ACTIVE_FLG)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
