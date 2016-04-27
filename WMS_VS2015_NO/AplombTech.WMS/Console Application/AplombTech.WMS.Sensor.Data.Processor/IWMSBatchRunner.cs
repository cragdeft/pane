using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Sensor.Data.Processor
{
    public interface IWMSBatchRunner
    {
        void Run(IWMSBatchStartPoint batchStartPoint);
    }
}
