using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Facade
{
    public class SampleAppInitializer : DropCreateDatabaseIfModelChanges<CommandModelDatabase>
    {
        protected override void Seed(CommandModelDatabase context)
        {
            Zone zone8 = CreateZone("Zone 8", context);
            DMA dma810 = CreateDMA("DMA 810", zone8, context);
            PumpStation baridhara1 = CreatePumpStation("Baridhara1",dma810, context);
            CreatePump("456", "123KL", baridhara1, context);
            CreateFlowTransmitter("741",baridhara1, context);
            CreatePressureTransmitter("369", baridhara1, context);
            CreateLevelTransmitter("852", baridhara1, context);
            CreateEnergySensor("2465", baridhara1, context);
            CreateChlorinationSensor("87654", baridhara1, context);

            PumpStation baridhara3 = CreatePumpStation("Baridhara3", dma810, context);
            CreatePump("123", "456KL", baridhara3, context);

            DMA dma811 = CreateDMA("DMA 811", zone8, context);
            PumpStation Shahjadpur = CreatePumpStation("Shahjadpur", dma811, context);
            PumpStation baridhara2 = CreatePumpStation("Baridhara2", dma811, context);

            AddUnit("Bar",context);
            AddUnit("KW/H", context);
            AddUnit("cubic/sec", context);
            AddUnit("meter", context);
            AddUnit("litre/min", context);
            //AddSensorDataLog(context);

            base.Seed(context);
        }

        private Zone CreateZone(string name, CommandModelDatabase context)
        {
            Zone zone = new Zone();
            zone.Name = name;
            zone.AuditFields.InsertedBy = "Automated";
            zone.AuditFields.InsertedDateTime = DateTime.Now;
            zone.AuditFields.LastUpdatedBy = "Automated";
            zone.AuditFields.LastUpdatedDateTime = DateTime.Now;

            context.Areas.Add(zone);

            return zone;
        }
        private DMA CreateDMA(string name, Zone zone, CommandModelDatabase context)
        {
            DMA dma = new DMA();
            dma.Name = name;
            dma.Parent = zone;

            dma.AuditFields.InsertedBy = "Automated";
            dma.AuditFields.InsertedDateTime = DateTime.Now;
            dma.AuditFields.LastUpdatedBy = "Automated";
            dma.AuditFields.LastUpdatedDateTime = DateTime.Now;

            context.Areas.Add(dma);

            return dma;
        }
        private PumpStation CreatePumpStation(string name, DMA dma, CommandModelDatabase context)
        {
            PumpStation station = new PumpStation();
            station.Name = name;
            station.Parent = dma;
            station.AuditFields.InsertedBy = "Automated";
            station.AuditFields.InsertedDateTime = DateTime.Now;
            station.AuditFields.LastUpdatedBy = "Automated";
            station.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Areas.Add(station);

            return station;
        }
        private void CreatePump(string modelNo, string uuid,PumpStation station, CommandModelDatabase context)
        {
            Pump pump = new Pump();
            pump.ModelNo = modelNo;
            pump.UUID = uuid;
            pump.PumpStation = station;
            pump.AuditFields.InsertedBy = "Automated";
            pump.AuditFields.InsertedDateTime = DateTime.Now;
            pump.AuditFields.LastUpdatedBy = "Automated";
            pump.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Devices.Add(pump);
        }
        private void CreateFlowTransmitter(string uuid, PumpStation station, CommandModelDatabase context)
        {
            FlowSensor sensor = new FlowSensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = 0;
            sensor.CumulativeValue = 0;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Sensors.Add(sensor);
        }
        private void CreatePressureTransmitter(string uuid, PumpStation station, CommandModelDatabase context)
        {
            PressureSensor sensor = new PressureSensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = 0;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Sensors.Add(sensor);
        }
        private void CreateLevelTransmitter(string uuid, PumpStation station, CommandModelDatabase context)
        {
            LevelSensor sensor = new LevelSensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = 0;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Sensors.Add(sensor);
        }
        private void CreateEnergySensor(string uuid, PumpStation station, CommandModelDatabase context)
        {
            EnergySensor sensor = new EnergySensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = 0;
            sensor.CumulativeValue = 0;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Sensors.Add(sensor);
        }
        private void CreateChlorinationSensor(string uuid, PumpStation station, CommandModelDatabase context)
        {
            ChlorinationSensor sensor = new ChlorinationSensor();
            sensor.UUID = uuid;
            sensor.CurrentValue = 0;
            sensor.PumpStation = station;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            context.Sensors.Add(sensor);
        }

        private void AddUnit(string name, CommandModelDatabase context)
        {
            Unit unit = new Unit();
            unit.Name = name;
            unit.AuditFields.InsertedBy = "Automated";
            unit.AuditFields.InsertedDateTime = DateTime.Now;
            unit.AuditFields.LastUpdatedBy = "Automated";
            unit.AuditFields.LastUpdatedDateTime = DateTime.Now;

            context.Units.Add(unit);
        }

        private void AddSensorDataLog(CommandModelDatabase context)
        {
            SensorDataLog log = new SensorDataLog();
            log.Topic = "test";
            log.Message = "test message";
            log.MessageReceivedAt = DateTime.Now;
            log.LoggedAtSensor = DateTime.Now;
            log.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.Done;

            context.SensorDataLogs.Add(log);
        }
    }
}
