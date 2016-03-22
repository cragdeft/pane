namespace AplombTech.DWasa.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Value = c.String(),
                        AuditField_InsertedBy = c.String(nullable: false),
                        AuditField_InsertedDateTime = c.DateTime(nullable: false),
                        AuditField_LastUpdatedBy = c.String(nullable: false),
                        AuditField_LastUpdatedDateTime = c.DateTime(nullable: false),
                        PumpStation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PumpStations", t => t.PumpStation_Id, cascadeDelete: true)
                .Index(t => t.PumpStation_Id);
            
            CreateTable(
                "dbo.PumpStations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.String(),
                        AuditField_InsertedBy = c.String(nullable: false),
                        AuditField_InsertedDateTime = c.DateTime(nullable: false),
                        AuditField_LastUpdatedBy = c.String(nullable: false),
                        AuditField_LastUpdatedDateTime = c.DateTime(nullable: false),
                        Address_Street1 = c.String(),
                        Address_Street2 = c.String(),
                        Address_Zip = c.String(),
                        Address_City = c.String(),
                        DMA_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DMAs", t => t.DMA_Id, cascadeDelete: true)
                .Index(t => t.DMA_Id);
            
            CreateTable(
                "dbo.DMAs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.String(),
                        AuditField_InsertedBy = c.String(nullable: false),
                        AuditField_InsertedDateTime = c.DateTime(nullable: false),
                        AuditField_LastUpdatedBy = c.String(nullable: false),
                        AuditField_LastUpdatedDateTime = c.DateTime(nullable: false),
                        Address_Street1 = c.String(),
                        Address_Street2 = c.String(),
                        Address_Zip = c.String(),
                        Address_City = c.String(),
                        Zone_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zones", t => t.Zone_Id, cascadeDelete: true)
                .Index(t => t.Zone_Id);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.String(),
                        AuditField_InsertedBy = c.String(nullable: false),
                        AuditField_InsertedDateTime = c.DateTime(nullable: false),
                        AuditField_LastUpdatedBy = c.String(nullable: false),
                        AuditField_LastUpdatedDateTime = c.DateTime(nullable: false),
                        Address_Street1 = c.String(),
                        Address_Street2 = c.String(),
                        Address_Zip = c.String(),
                        Address_City = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SensorStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        LogDateTime = c.DateTime(nullable: false),
                        AuditField_InsertedBy = c.String(nullable: false),
                        AuditField_InsertedDateTime = c.DateTime(nullable: false),
                        AuditField_LastUpdatedBy = c.String(nullable: false),
                        AuditField_LastUpdatedDateTime = c.DateTime(nullable: false),
                        Device_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Device_Id, cascadeDelete: true)
                .Index(t => t.Device_Id);
            
            CreateTable(
                "dbo.Cameras",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Url = c.String(),
                        UId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Pumps",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Routers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MacId = c.String(),
                        Ip = c.String(),
                        Port = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sensors", "Id", "dbo.Devices");
            DropForeignKey("dbo.Routers", "Id", "dbo.Devices");
            DropForeignKey("dbo.Pumps", "Id", "dbo.Devices");
            DropForeignKey("dbo.Cameras", "Id", "dbo.Devices");
            DropForeignKey("dbo.SensorStatus", "Device_Id", "dbo.Devices");
            DropForeignKey("dbo.Devices", "PumpStation_Id", "dbo.PumpStations");
            DropForeignKey("dbo.PumpStations", "DMA_Id", "dbo.DMAs");
            DropForeignKey("dbo.DMAs", "Zone_Id", "dbo.Zones");
            DropIndex("dbo.Sensors", new[] { "Id" });
            DropIndex("dbo.Routers", new[] { "Id" });
            DropIndex("dbo.Pumps", new[] { "Id" });
            DropIndex("dbo.Cameras", new[] { "Id" });
            DropIndex("dbo.SensorStatus", new[] { "Device_Id" });
            DropIndex("dbo.DMAs", new[] { "Zone_Id" });
            DropIndex("dbo.PumpStations", new[] { "DMA_Id" });
            DropIndex("dbo.Devices", new[] { "PumpStation_Id" });
            DropTable("dbo.Sensors");
            DropTable("dbo.Routers");
            DropTable("dbo.Pumps");
            DropTable("dbo.Cameras");
            DropTable("dbo.SensorStatus");
            DropTable("dbo.Zones");
            DropTable("dbo.DMAs");
            DropTable("dbo.PumpStations");
            DropTable("dbo.Devices");
        }
    }
}
