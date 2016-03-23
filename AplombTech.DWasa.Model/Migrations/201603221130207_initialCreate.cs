namespace AplombTech.DWasa.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SensorStatus", "Value", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SensorStatus", "Value", c => c.String(nullable: false));
        }
    }
}
