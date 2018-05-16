namespace JahanAraShop.Data.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Runtime.Remoting.Contexts;
    using System.Collections.Generic;
    using Domain.DomainModel;

    public partial class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base("name=DataBaseContext")
        {
        }

        public virtual DbSet<tblGood> tblGoods { get; set; }
        public virtual DbSet<tblGoodGroup> tblGoodGroups { get; set; }
        public virtual DbSet<tblGoodSaleInfo> tblGoodSaleInfoes { get; set; }
        public virtual DbSet<tblGoodType> tblGoodTypes { get; set; }
        public virtual DbSet<tblOrganization> tblOrganizations { get; set; }
        public virtual DbSet<tblSaleDeliverType> tblSaleDeliverTypes { get; set; }
        public virtual DbSet<tblSaleInvoice> tblSaleInvoices { get; set; }
        public virtual DbSet<tblSaleInvoiceDetail> tblSaleInvoiceDetails { get; set; }
        public virtual DbSet<tblSiteCounty> tblSiteCounties { get; set; }
        public virtual DbSet<tblSiteInformation> tblSiteInformations { get; set; }
        public virtual DbSet<tblSiteInformationType> tblSiteInformationTypes { get; set; }
        public virtual DbSet<tblSiteState> tblSiteStates { get; set; }
  

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblGood>()
                .Property(e => e.BarCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.RetailPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.CurrencyRetailPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.StandardDiscountValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.StandardPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.CustomerPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.OldRetailPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.PurchaseRetailPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGood>()
                .Property(e => e.FarsiCreateDate)
                .IsFixedLength()
                .IsUnicode(false);



            modelBuilder.Entity<tblGood>()
                .HasMany(e => e.tblGoodSaleInfoes)
                .WithRequired(e => e.tblGood)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblGood>()
                .HasMany(e => e.tblSaleInvoiceDetails)
                .WithRequired(e => e.tblGood)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblGoodGroup>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblGoodGroup>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<tblGoodGroup>()
                .Property(e => e.Name3)
                .IsUnicode(false);

            modelBuilder.Entity<tblGoodGroup>()
                .Property(e => e.Name2)
                .IsUnicode(false);

            modelBuilder.Entity<tblGoodGroup>()
                .HasMany(e => e.tblGoodGroup1)
                .WithOptional(e => e.tblGoodGroup2)
                .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<tblGoodSaleInfo>()
                .Property(e => e.Barcode)
                .IsUnicode(false);

            modelBuilder.Entity<tblGoodSaleInfo>()
                .Property(e => e.RetailPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGoodSaleInfo>()
                .Property(e => e.DiscountPercent)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGoodSaleInfo>()
                .Property(e => e.DiscountValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblGoodType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.BossDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.WebAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.CellPhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Phone2)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FarsiCreateDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FarsiBirthDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FarsiMarriageDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Town)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Street1)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Street2)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Alley1)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Alley2)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.No)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FloorNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.UnitNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.NationalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.CodeBK)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Code_better)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.Address3)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.StoreName)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.CellPhone2)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.RingNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FarsiReContactDate)
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FarsiEditDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblOrganization>()
                .Property(e => e.FarsiCardPrintDate)
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleDeliverType>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.FarsiCreateDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.DiscountPercent)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.DiscountValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.SuchargeValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.TotalValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.CustomerMessage)
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.PureTotalValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.FarsiClearDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.ManualCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.ShortageValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.FarsiDeliverDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.RemCredit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.UsedCredit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.FarsiUpdateDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoice>()
                .Property(e => e.FarsiLastLotteryPrintDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoice>()
                .HasMany(e => e.tblSaleInvoiceDetails)
                .WithRequired(e => e.tblSaleInvoice)
                .HasForeignKey(e => e.InvoiceID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblSaleInvoiceDetail>()
                .Property(e => e.BarCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoiceDetail>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<tblSaleInvoiceDetail>()
                .Property(e => e.DiscountPercent)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoiceDetail>()
                .Property(e => e.DiscountValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoiceDetail>()
                .Property(e => e.RetailPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblSaleInvoiceDetail>()
                .Property(e => e.ServicedBarCode)
                .IsUnicode(false);

        }
    }
}
