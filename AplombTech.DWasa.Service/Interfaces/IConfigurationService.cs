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
        Zone AddZone(ZoneEntity zone);
        void EditZone(ZoneEntity zone);
        //void DeleteZone(Zone zone);
        DMA AddDMA(DMA dma);
        //void EditDMA(DMA dma);
        //void DeleteDMA(int dmaId);

        //PumpStation AddPumpStation(PumpStation pump);
        //void EditPump(PumpStation pump);
        //void DeletePump( int pumpId);

        List<ZoneEntity> GetAllZone();

        bool IsZoneExists(string name);
    }
}
