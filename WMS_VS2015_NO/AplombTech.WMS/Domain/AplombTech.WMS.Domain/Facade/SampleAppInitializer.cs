using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using AplombTech.WMS.Domain.Motors;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.UserAccounts;
using AplombTech.WMS.Utility;
using ChlorineMotor = AplombTech.WMS.Domain.Motors.ChlorineMotor;
using PumpMotor = AplombTech.WMS.Domain.Motors.PumpMotor;

namespace AplombTech.WMS.Domain.Facade
{
    public class SampleAppInitializer : DropCreateDatabaseIfModelChanges<CommandModelDatabase>
    {
        protected override void Seed(CommandModelDatabase context)
        {
            string dma810Boundary = @"90.41552734375,23.806289672851562|90.416259765625,23.806793212890625|90.4183349609375,23.807479858398438|90.41864013671875,23.807968139648438|90.4190673828125,23.808380126953125|90.419677734375,23.80877685546875|90.42083740234375,23.809249877929688|90.4212646484375,23.809249877929688|90.42138671875,23.808547973632812|90.4215087890625,23.807479858398438|90.4217529296875,23.805908203125|90.42193603515625,23.805068969726562|90.422119140625,23.804290771484375|90.4222412109375,23.803558349609375|90.42236328125,23.802978515625|90.42254638671875,23.802322387695312|90.42279052734375,23.800918579101562|90.42303466796875,23.79962158203125|90.42327880859375,23.798568725585938|90.4234619140625,23.797576904296875|90.4224853515625,23.79742431640625|90.4210205078125,23.797012329101562|90.4195556640625,23.796463012695312|90.41876220703125,23.796142578125|90.41815185546875,23.796722412109375|90.41754150390625,23.797805786132812|90.41748046875,23.79888916015625|90.41748046875,23.80047607421875|90.41754150390625,23.80206298828125|90.4173583984375,23.802947998046875|90.4171142578125,23.803848266601562|90.41668701171875,23.805023193359375|90.416259765625,23.805679321289062|90.41552734375,23.806289672851562";
            string dma811Boundary = @"542893.5,631827.5|543047,631874.5|543138.5,631892|543209.5,631510.5|543235.5,631366.5|543284.5,631090|543299.5,631003|543306.5,630962|543276.5,630946.5|543233.5,630922.5|543238,630903|543198,630884.5|543132.5,630867|543089.5,630851|543028.5,630834.5|542962,630851.5|542982,631036.5|542994.5,631148|542990,631216|542961.5,631315|542943,631377.5|542921,631451.5|542870.5,631506.5|542664.5,631729.5|542744.5,631765.5|542893.5,631827.5";
            string zone8Boundary =null;
            string baridhara1PumpBoundary = @"90.4196882371,23.805628|";
            string baridhara2PumpBoundary = null;
            string baridhara3PumpBoundary = null;
            string shahjadpurPumpBoundary = null;

            Zone zone8 = CreateZone("Zone 8", zone8Boundary, context);
            DMA dma810 = CreateDMA("DMA 810", zone8, dma810Boundary, context);
            PumpStation baridhara1 = CreatePumpStation("Baridhara1",dma810,baridhara1PumpBoundary, context);
            AddUnit("cubic/sec", context);

            CreatePumpMotor("456", "123KL", baridhara1, context);
            CreateCholorineMotor("456", "123KL", baridhara1, context);
            CreateFlowTransmitter("741", baridhara1, context, "");
            CreatePressureTransmitter("369", baridhara1, context, "");
            CreateLevelTransmitter("852", baridhara1, context, "");
            CreateEnergySensor("2465", baridhara1, context, "");
            CreateChlorinationSensor("87654", baridhara1, context);

            PumpStation baridhara3 = CreatePumpStation("Baridhara3",dma810, baridhara3PumpBoundary, context);
            //CreatePump("123", "456KL", baridhara3, context);

            DMA dma811 = CreateDMA("DMA 811", zone8, dma810Boundary, context);
            PumpStation Shahjadpur = CreatePumpStation("Shahjadpur", dma811,shahjadpurPumpBoundary, context);
            PumpStation baridhara2 = CreatePumpStation("Baridhara2", dma811,baridhara2PumpBoundary, context);

            CreateDesignation("PO", "Pump Operator", context);
            CreateDesignation("SAE", "Sub Asst. Engineer", context);
            CreateDesignation("SDE/AE", "Sub Divisional Engineer/Asst. Engineer", context);
            CreateDesignation("EE", "Executive Engineer", context);
            CreateDesignation("CO", "Complain Operator", context);

            CreateAlertType("Pump On/Off", "Pump is off. Pump Station Name is ", context);
            CreateAlertType("Under Threshold", "Under Threshold value of Sensor|of Pump Station ", context);
            CreateAlertType("Data Missing", " Data is missing of Sensor|of Pump Station ", context);

            CreateFeatureList(context);

            base.Seed(context);
        }
        
        private Zone CreateZone(string name, string location, CommandModelDatabase context)
        {
            Zone zone = new Zone();
            zone.Name = name;
            zone.Location = location;
            zone.AuditFields.InsertedBy = "Automated";
            zone.AuditFields.InsertedDateTime = DateTime.Now;
            zone.AuditFields.LastUpdatedBy = "Automated";
            zone.AuditFields.LastUpdatedDateTime = DateTime.Now;

            context.Areas.Add(zone);

            return zone;
        }
        private DMA CreateDMA(string name, Zone zone,string location, CommandModelDatabase context)
        {
            DMA dma = new DMA();
            dma.Name = name;
            dma.Parent = zone;
            dma.Location = location;
            dma.AuditFields.InsertedBy = "Automated";
            dma.AuditFields.InsertedDateTime = DateTime.Now;
            dma.AuditFields.LastUpdatedBy = "Automated";
            dma.AuditFields.LastUpdatedDateTime = DateTime.Now;

            context.Areas.Add(dma);

            return dma;
        }
        private PumpStation CreatePumpStation(string name, DMA dma,string location, CommandModelDatabase context)
        {
            PumpStation station = new PumpStation();
            station.Name = name;
            station.Parent = dma;
            station.Location = location;
            station.AuditFields.InsertedBy = "Automated";
            station.AuditFields.InsertedDateTime = DateTime.Now;
            station.AuditFields.LastUpdatedBy = "Automated";
            station.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Areas.Add(station);

            return station;
        }
        private void CreatePumpMotor(string modelNo, string uuid,PumpStation station, CommandModelDatabase context)
        {
            PumpMotor pump = new PumpMotor();
            pump.ModelNo = modelNo;
            pump.UUID = uuid;
            pump.Controllable = true;
            pump.Auto = true;
            pump.PumpStation = station;
            pump.AuditFields.InsertedBy = "Automated";
            pump.AuditFields.InsertedDateTime = DateTime.Now;
            pump.AuditFields.LastUpdatedBy = "Automated";
            pump.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Motors.Add(pump);
        }

        private void CreateCholorineMotor(string modelNo, string uuid, PumpStation station, CommandModelDatabase context)
        {
            ChlorineMotor pump = new ChlorineMotor();
            pump.UUID = uuid;
            pump.Controllable = false;
            pump.Auto = false;
            pump.PumpStation = station;
            pump.AuditFields.InsertedBy = "Automated";
            pump.AuditFields.InsertedDateTime = DateTime.Now;
            pump.AuditFields.LastUpdatedBy = "Automated";
            pump.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Motors.Add(pump);
        }
        private void CreateFlowTransmitter(string uuid, PumpStation station, CommandModelDatabase context,string unit)
        {
            FlowSensor sensor = new FlowSensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = "0";
            sensor.CumulativeValue = "0";
            sensor.IsActive = true;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            sensor.UnitName = unit;
            context.Sensors.Add(sensor);
        }
        private void CreatePressureTransmitter(string uuid, PumpStation station, CommandModelDatabase context, string unit)
        {
            PressureSensor sensor = new PressureSensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = "0";
            sensor.IsActive = true;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            sensor.UnitName = unit;
            context.Sensors.Add(sensor);
        }
        private void CreateLevelTransmitter(string uuid, PumpStation station, CommandModelDatabase context, string unit)
        {
            LevelSensor sensor = new LevelSensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = "0";
            sensor.IsActive = true;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            sensor.UnitName = unit;
            context.Sensors.Add(sensor);
        }
        private void CreateEnergySensor(string uuid, PumpStation station, CommandModelDatabase context, string unit)
        {
            EnergySensor sensor = new EnergySensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = "0";
            sensor.CumulativeValue = "0";
            sensor.IsActive = true;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            sensor.UnitName = unit;
            context.Sensors.Add(sensor);
        }
        private void CreateChlorinationSensor(string uuid, PumpStation station, CommandModelDatabase context)
        {
            ChlorinationSensor sensor = new ChlorinationSensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = "0";
            sensor.IsActive = true;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Sensors.Add(sensor);
        }
        private Unit AddUnit(string name, CommandModelDatabase context)
        {
            Unit unit = new Unit();
            unit.Name = name;
            unit.AuditFields.InsertedBy = "Automated";
            unit.AuditFields.InsertedDateTime = DateTime.Now;
            unit.AuditFields.LastUpdatedBy = "Automated";
            unit.AuditFields.LastUpdatedDateTime = DateTime.Now;

            context.Units.Add(unit);

            return unit;
        }
        private void CreateDesignation(string shortName, string fullName, CommandModelDatabase context)
        {
            Designation desig = new Designation();
            desig.DesignationShortName = shortName;
            desig.DesignationName = fullName;
            
            desig.AuditFields.InsertedBy = "Automated";
            desig.AuditFields.InsertedDateTime = DateTime.Now;
            desig.AuditFields.LastUpdatedBy = "Automated";
            desig.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Designations.Add(desig);
        }
        private void CreateAlertType(string alertName, string alertMessage, CommandModelDatabase context)
        {
            AlertType alert = new AlertType();
            alert.AlertName = alertName;
            alert.AlertMessage = alertMessage;

            alert.AuditFields.InsertedBy = "Automated";
            alert.AuditFields.InsertedDateTime = DateTime.Now;
            alert.AuditFields.LastUpdatedBy = "Automated";
            alert.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.AlertTypes.Add(alert);
        }
        private void CreateFeatureList(CommandModelDatabase context)
        {
            Role role = CreateRole("Admin", context);
            LoginUser adminUser = CreateAdminUser(role, context);
            AssignRoleToAdminUser(adminUser, role, context);

            FeatureType areaFeaturetype = CreateFeatureType(FeatureType.FeatureTypeEnums.Area.ToString(), context);
            AddAreaFeatures(areaFeaturetype, role, context);

            FeatureType alertFeaturetype = CreateFeatureType(FeatureType.FeatureTypeEnums.Alert.ToString(), context);
            AddAlertFeatures(alertFeaturetype, role, context);

            FeatureType userAccountsFeaturetype = CreateFeatureType(FeatureType.FeatureTypeEnums.UserAccount.ToString(), context);
            AddUserAccountsFeatures(userAccountsFeaturetype, role, context);

            FeatureType reportFeaturetype = CreateFeatureType(FeatureType.FeatureTypeEnums.Report.ToString(), context);
            AddUReportFeatures(reportFeaturetype, role, context);
        }      
        private FeatureType CreateFeatureType(string typeName, CommandModelDatabase context)
        {
            FeatureType type = new FeatureType();
            type.FeatureTypeName = typeName;

            context.FeatureTypes.Add(type);

            return type;
        }
        private void AddAreaFeatures(FeatureType areaFeaturetype, Role role, CommandModelDatabase context)
        {
            var values = Enum.GetValues(typeof (Feature.AreaFeatureEnums));
            foreach (int value in values)
            {
                Feature feature = new Feature();
                feature.FeatureType = areaFeaturetype;
                feature.FeatureName = Enum.GetName(typeof (Feature.AreaFeatureEnums), value);
                feature.FeatureCode = value;               
                
                context.Features.Add(feature);
                CreateRoleFeatures(feature, role,context);
            }
        }        
        private void AddAlertFeatures(FeatureType alertFeaturetype, Role role, CommandModelDatabase context)
        {
            var values = Enum.GetValues(typeof(Feature.AlertFeatureEnums));
            foreach (int value in values)
            {
                Feature feature = new Feature();
                feature.FeatureType = alertFeaturetype;
                feature.FeatureName = Enum.GetName(typeof(Feature.AlertFeatureEnums), value);
                feature.FeatureCode = value;

                context.Features.Add(feature);
                CreateRoleFeatures(feature, role, context);
            }
        }
        private void AddUserAccountsFeatures(FeatureType userAccountsFeaturetype, Role role, CommandModelDatabase context)
        {
            var values = Enum.GetValues(typeof(Feature.UserAccountsFeatureEnums));
            foreach (int value in values)
            {
                Feature feature = new Feature();
                feature.FeatureType = userAccountsFeaturetype;
                feature.FeatureName = Enum.GetName(typeof(Feature.UserAccountsFeatureEnums), value);
                feature.FeatureCode = value;

                context.Features.Add(feature);
                CreateRoleFeatures(feature, role, context);
            }
        }
        private void AddUReportFeatures(FeatureType reportFeaturetype, Role role, CommandModelDatabase context)
        {
            var values = Enum.GetValues(typeof(Feature.ReportFeatureEnums));
            foreach (int value in values)
            {
                Feature feature = new Feature();
                feature.FeatureType = reportFeaturetype;
                feature.FeatureName = Enum.GetName(typeof(Feature.ReportFeatureEnums), value);
                feature.FeatureCode = value;

                context.Features.Add(feature);
                CreateRoleFeatures(feature, role, context);
            }
        }
        private LoginUser CreateAdminUser(Role role, CommandModelDatabase context)
        {
            LoginUser user = new LoginUser();
            user.Id = Guid.NewGuid().ToString();
            user.UserName = "admin@gmail.com";
            user.Email = "admin@gmail.com";
            user.EmailConfirmed = false;
            user.PasswordHash = PasswordHash.HashPassword("123456");
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = false;
            user.AccessFailedCount = 0;

            context.LoginUsers.Add(user);

            return user;
        }
        private Role CreateRole(string roleName, CommandModelDatabase context)
        {
            Role role = new Role();
            role.Id = Guid.NewGuid().ToString();
            role.Name = roleName;

            context.Roles.Add(role);

            return role;
        }
        private void AssignRoleToAdminUser(LoginUser adminUser, Role role, CommandModelDatabase context)
        {
            UserRoles userRole = new UserRoles();
            userRole.LoginUser = adminUser;
            userRole.Role = role;

            context.UserRoles.Add(userRole);
        }
        private void CreateRoleFeatures(Feature feature, Role role, CommandModelDatabase context)
        {
            RoleFeatures roleFeature = new RoleFeatures();
            roleFeature.Feature = feature;
            roleFeature.Role = role;

            context.RoleFeatures.Add(roleFeature);
        }
    }
}
