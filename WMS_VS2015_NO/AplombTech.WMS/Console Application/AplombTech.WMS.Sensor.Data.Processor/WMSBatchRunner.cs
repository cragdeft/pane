using NakedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Sensor.Data.Processor
{
    public sealed class WMSBatchRunner : IWMSBatchRunner
    {
        private readonly INakedObjectsFramework _framework;

        public WMSBatchRunner(INakedObjectsFramework objframework)
        {
            //Assert.AssertNotNull(framework);
            this._framework = objframework;
        }

        #region IBatchRunner Members

        public void Run(IWMSBatchStartPoint batchStartPoint)
        {
            _framework.DomainObjectInjector.InjectInto(batchStartPoint);
            batchStartPoint.Execute(_framework);
            while (true)
            {

            }
        }

        #endregion

        private void StartTransaction()
        {
            _framework.TransactionManager.StartTransaction();
        }

        private void EndTransaction()
        {
            _framework.TransactionManager.EndTransaction();
        }
    }
}
