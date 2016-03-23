using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Model.Models;

namespace AplombTech.DWasa.Service.Interfaces
{
    public interface ISensorService
    {
        IEnumerable<SensorEntity> GetSensorData(int pumpStationId);
    }
}
