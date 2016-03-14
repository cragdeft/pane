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
                        Address_Line1 = c.String(),
                        Address_Line2 = c.String(),
                        Address_Line3 = c.String(),
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
                        Address_Line1 = c.String(),
                        Address_Line2 = c.String(),
                        Address_Line3 = c.String(),
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
                        Address_Line1 = c.String(),
                        Address_Line2 = c.String(),
                        Address_Line3 = c.String(),
                        Dma_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DMAs", t => t.Dma_Id)
                .Index(t => t.Dma_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PumpStations", "Dma_Id", "dbo.DMAs");
            DropForeignKey("dbo.DMAs", "Zone_Id", "dbo.Zones");
            DropIndex("dbo.PumpStations", new[] { "Dma_Id" });
            DropIndex("dbo.DMAs", new[] { "Zone_Id" });
            DropTable("dbo.PumpStations");
            DropTable("dbo.Zones");
            DropTable("dbo.DMAs");
        }
    }
}
