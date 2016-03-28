namespace AplombTech.DWasa.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Devices", "PumpStation_Id", "dbo.PumpStations");
            DropIndex("dbo.Devices", new[] { "PumpStation_Id" });
            CreateTable(
                "dbo.CommandJsons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommandJsonString = c.String(),
                        IsProcessed = c.Boolean(nullable: false),
                        CommandType = c.Int(nullable: false),
                        ProcessFailReason = c.String(),
                        AuditField_InsertedBy = c.String(nullable: false),
                        AuditField_InsertedDateTime = c.DateTime(nullable: false),
                        AuditField_LastUpdatedBy = c.String(nullable: false),
                        AuditField_LastUpdatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Devices", "PumpStation_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Devices", "PumpStation_Id");
            AddForeignKey("dbo.Devices", "PumpStation_Id", "dbo.PumpStations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "PumpStation_Id", "dbo.PumpStations");
            DropIndex("dbo.Devices", new[] { "PumpStation_Id" });
            AlterColumn("dbo.Devices", "PumpStation_Id", c => c.Int());
            DropTable("dbo.CommandJsons");
            CreateIndex("dbo.Devices", "PumpStation_Id");
            AddForeignKey("dbo.Devices", "PumpStation_Id", "dbo.PumpStations", "Id");
        }
    }
}
