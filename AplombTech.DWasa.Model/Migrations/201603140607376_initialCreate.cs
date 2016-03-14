namespace AplombTech.DWasa.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DMAs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        AuditField_InsertedBy = c.String(),
                        AuditField_InsertedDateTime = c.DateTime(),
                        AuditField_LastUpdatedBy = c.String(),
                        AuditField_LastUpdatedDateTime = c.DateTime(),
                        Address_Street1 = c.String(),
                        Address_Street2 = c.String(),
                        Address_Zip = c.String(),
                        Address_City = c.String(),
                        Zone_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zones", t => t.Zone_Id)
                .Index(t => t.Zone_Id);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        AuditField_InsertedBy = c.String(),
                        AuditField_InsertedDateTime = c.DateTime(),
                        AuditField_LastUpdatedBy = c.String(),
                        AuditField_LastUpdatedDateTime = c.DateTime(),
                        Address_Street1 = c.String(),
                        Address_Street2 = c.String(),
                        Address_Zip = c.String(),
                        Address_City = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PumpStations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        AuditField_InsertedBy = c.String(),
                        AuditField_InsertedDateTime = c.DateTime(),
                        AuditField_LastUpdatedBy = c.String(),
                        AuditField_LastUpdatedDateTime = c.DateTime(),
                        Address_Street1 = c.String(),
                        Address_Street2 = c.String(),
                        Address_Zip = c.String(),
                        Address_City = c.String(),
                        DMA_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DMAs", t => t.DMA_Id)
                .Index(t => t.DMA_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PumpStations", "DMA_Id", "dbo.DMAs");
            DropForeignKey("dbo.DMAs", "Zone_Id", "dbo.Zones");
            DropIndex("dbo.PumpStations", new[] { "DMA_Id" });
            DropIndex("dbo.DMAs", new[] { "Zone_Id" });
            DropTable("dbo.PumpStations");
            DropTable("dbo.Zones");
            DropTable("dbo.DMAs");
        }
    }
}
