namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblOrganization")]
    public partial class tblOrganization
    {
        public int ID { get; set; }

        public int? ParentID { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string BossDescription { get; set; }

        public int? ExternalLocationID { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Phone1 { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(250)]
        public string WebAddress { get; set; }

        [StringLength(2500)]
        public string Notes { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(100)]
        public string CellPhone { get; set; }

        [StringLength(100)]
        public string Phone2 { get; set; }

        public int? RelatedPositionID { get; set; }

        public int? WorkActivityID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        [StringLength(10)]
        public string FarsiCreateDate { get; set; }

        public int? CustomerDegreeID { get; set; }

        public int? EducationID { get; set; }

        public int? JobID { get; set; }

        public int? PurchaseLocationID { get; set; }

        public int? ReservedID { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(10)]
        public string FarsiBirthDate { get; set; }

        public DateTime? MarriageDate { get; set; }

        [StringLength(10)]
        public string FarsiMarriageDate { get; set; }

        [StringLength(250)]
        public string City { get; set; }

        [StringLength(250)]
        public string Town { get; set; }

        [StringLength(250)]
        public string Street1 { get; set; }

        [StringLength(250)]
        public string Street2 { get; set; }

        [StringLength(250)]
        public string Alley1 { get; set; }

        [StringLength(250)]
        public string Alley2 { get; set; }

        [StringLength(250)]
        public string No { get; set; }

        [StringLength(250)]
        public string FloorNo { get; set; }

        [StringLength(250)]
        public string UnitNo { get; set; }

        [StringLength(11)]
        public string NationalCode { get; set; }

        public int? CreatorID { get; set; }

        [StringLength(250)]
        public string PostalCode { get; set; }

        [StringLength(250)]
        public string FirstName { get; set; }

        public string FullName { get; set; }

        [StringLength(250)]
        public string CodeBK { get; set; }

        [StringLength(250)]
        public string Code_better { get; set; }

        public float? Lat { get; set; }

        public float? Lng { get; set; }

        [StringLength(500)]
        public string Address2 { get; set; }

        [StringLength(500)]
        public string Address3 { get; set; }

        [StringLength(250)]
        public string StoreName { get; set; }

        public int? PreConditionID { get; set; }

        public int? TypeID { get; set; }

        public int? DataStatusTypeID { get; set; }

        public int? LockedUserID { get; set; }

        [StringLength(250)]
        public string CellPhone2 { get; set; }

        [StringLength(250)]
        public string RingNo { get; set; }

        public int? OriginalID { get; set; }

        public int? ContactStatusTypeID { get; set; }

        public DateTime? ReContactDate { get; set; }

        [StringLength(10)]
        public string FarsiReContactDate { get; set; }

        public int? SatisfactionTypeID { get; set; }

        public DateTime? EditDate { get; set; }

        [StringLength(10)]
        public string FarsiEditDate { get; set; }

        public DateTime? CardPrintDate { get; set; }

        [StringLength(10)]
        public string FarsiCardPrintDate { get; set; }

        public bool? IsMale { get; set; }

        public int? StateTaxOrganizationCode { get; set; }

        public int? CityTaxOrganizationCode { get; set; }

        [StringLength(128)]
        public string User_Id { get; set; }

        public int? FromMorningTime { get; set; }

        public int? ToMorningTime { get; set; }

        public int? FromEveningTime { get; set; }

        public int? ToEveningTime { get; set; }

        public int? CountryId { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public int? IntroductionTypeID { get; set; }

        public int? BankID { get; set; }
    }
}
