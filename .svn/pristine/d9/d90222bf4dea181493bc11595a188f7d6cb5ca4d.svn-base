namespace JahanAraShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JAHANARAName : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSiteRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.tblSiteUserRoles",
                c => new
                {
                    RoleId = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                    IdentityRole_Id = c.String(maxLength: 128),
                    IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.tblSiteRoles", t => t.IdentityRole_Id)
                .ForeignKey("dbo.tblSiteUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.IdentityUser_Id);

            CreateTable(
                "dbo.tblSiteUsers",
                c => new
                {
                    User_Id = c.String(nullable: false, maxLength: 128),
                    Email = c.String(),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(),
                    Discriminator = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.User_Id);

            CreateTable(
                "dbo.tblSiteUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                    IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblSiteUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);

            CreateTable(
                "dbo.tblSiteUserLogins",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    LoginProvider = c.String(),
                    ProviderKey = c.String(),
                    IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.tblSiteUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.tblSiteUserRoles", "IdentityUser_Id", "dbo.tblSiteUsers");
            DropForeignKey("dbo.tblSiteUserLogins", "IdentityUser_Id", "dbo.tblSiteUsers");
            DropForeignKey("dbo.tblSiteUserClaims", "IdentityUser_Id", "dbo.tblSiteUsers");
            DropForeignKey("dbo.tblSiteUserRoles", "IdentityRole_Id", "dbo.tblSiteRoles");
            DropIndex("dbo.tblSiteUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.tblSiteUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.tblSiteUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.tblSiteUserRoles", new[] { "IdentityRole_Id" });
            DropTable("dbo.tblSiteUserLogins");
            DropTable("dbo.tblSiteUserClaims");
            DropTable("dbo.tblSiteUsers");
            DropTable("dbo.tblSiteUserRoles");
            DropTable("dbo.tblSiteRoles");
        }
    }
}
