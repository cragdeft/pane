﻿using AplombTech.WMS.MQTT.Client;
using NakedObjects;
using NakedObjects.Architecture.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NakedObjects.Core.Component
{
    public sealed class MyBatchRunner : IMyBatchRunner
    {
        private readonly INakedObjectsFramework framework;

        public MyBatchRunner(INakedObjectsFramework framework)
        {
            //Assert.AssertNotNull(framework);
            this.framework = framework;
        }

        #region IBatchRunner Members

        public void Run(IMyBatchStartPoint batchStartPoint)
        {
            framework.DomainObjectInjector.InjectInto(batchStartPoint);
            //StartTransaction();
            batchStartPoint.Execute(framework);
            //EndTransaction();
            while (true)
            {

            }
        }

        #endregion

        private void StartTransaction()
        {
            framework.TransactionManager.StartTransaction();
        }

        private void EndTransaction()
        {
            framework.TransactionManager.EndTransaction();
        }
    }
}