using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Model.Models;

namespace AplombTech.DWasa.Service.Interfaces
{
    public interface IConfigurationService
    {
        ZoneEntity AddZone(ZoneEntity zone);
        void EditZone(ZoneEntity zone);
        //void DeleteZone(Zone zone);
        DMAEntity AddDMA(DMAEntity dma);
        ZoneEntity FindZone(int zoneId);
        PumpStationEntity FindPumpStation(int pumpStationId);

        List<DMAEntity> GetAllDMA();
        List<PumpStationEntity> GetAllPumpStation();
        void AddSensor(PumpStationSensorEntity entity);

        void AddRouter(PumpStationRouterEntity entity);
        void AddCamera(PumpStationCameraEntity entity);

        void AddPump(PumpStationPumpEntity entity);

        PumpStationEntity AddPumpStation(PumpStationEntity pump);
        List<DeviceEntity> GetAllDevice();

        //List<SensorStatusEntity> GetSensorStatus(int deviceId);
        List<SensorStatusEntity> GetOverViewDataOfPumpStation(int pumpStationId);

        SensorStatusEntity GetSinleSensorStatus(int sensorId);

        List<ZoneEntity> GetAllZone();
        List<ZoneEntity> GetAll();
        bool IsZoneExists(string name);
    }
}
