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

        List<DMAEntity> GetAllDMA();
        List<PumpStationEntity> GetAllPumpStation();
        void AddSensor(PumpStationSensorEntity entity);
        //void EditDMA(DMA dma);
        //void DeleteDMA(int dmaId);

        PumpStationEntity AddPumpStation(PumpStationEntity pump);
        //void EditPump(PumpStation pump);
        //void DeletePump( int pumpId);

        List<ZoneEntity> GetAllZone();

        bool IsZoneExists(string name);
    }
}
