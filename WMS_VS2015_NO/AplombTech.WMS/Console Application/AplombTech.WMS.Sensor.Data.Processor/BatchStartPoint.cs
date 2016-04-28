// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Messages.Commands;
using NakedObjects;
using NakedObjects.Architecture.Component;
using NakedObjects.Async;
using NServiceBus;

namespace AplombTech.WMS.Sensor.Data.Processor {
    public class BatchStartPoint : IWMSBatchStartPoint //, IHandleMessages<ProcessSensorData>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Injected Services
        private INakedObjectsFramework _framework;
        public AreaRepository AreaRepository { set; protected get; }
        public ProcessRepository ProcessRepository { set; protected get; }
        public IAsyncService AsyncService { private get; set; }
        #endregion

        //public enum JsonMessageType
        //{
        //    configuration,
        //    sensordata,
        //    feedback
        //}

        #region IBatchStartPoint Members

        public void Execute(INakedObjectsFramework objframework)
        {
            _framework = objframework;
            ServiceBus.Init();
        }

        #endregion

        //public void Handle(ProcessSensorData message)
        //{
        //    log.Info("Sensor Data process has started for Id : " + message.SensorDataLogId);
        //    DataLog dataLog = null;
        //    try
        //    {
        //        _framework.TransactionManager.StartTransaction();
        //        dataLog = ProcessRepository.GetDataLogById(message.SensorDataLogId);

        //        if (dataLog.Topic.Replace("/", String.Empty) == JsonMessageType.sensordata.ToString())
        //        {
        //            ProcessRepository.ParseNStoreSensorData(dataLog);
        //        }
        //        if (dataLog.Topic.Replace("/", String.Empty) == JsonMessageType.configuration.ToString())
        //        {
        //            ProcessRepository.ParseNStoreConfigurationData(dataLog);
        //        }
        //        _framework.TransactionManager.EndTransaction();
        //        log.Info("Sensor Data process has ended for Id : " + message.SensorDataLogId);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info("Error Occured in Sensor Data Process for Id : " + message.SensorDataLogId + ". Error: " + ex.ToString());
        //        _framework.TransactionManager.AbortTransaction();
        //        _framework.TransactionManager.StartTransaction();
        //        dataLog.ProcessingStatus = DataLog.ProcessingStatusEnum.Failed;
        //        dataLog.Remarks = "Error Occured in ProcessMessage method. Error: " + ex.ToString();
        //        _framework.TransactionManager.EndTransaction();
        //    }
        //}
    }
}