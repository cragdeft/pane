// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.MQTTService;
using AplombTech.WMS.Domain.Repositories;
using NakedObjects.Architecture.Component;
using NakedObjects.Async;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AplombTech.WMS.MQTT.Client {
    public class BatchStartPoint : IBatchStartPoint {
        public IAsyncService AsyncService { private get; set; }
        public AreaRepository AreaRepository { set; protected get; }
        public MqttClientFacade MqttClientFacade { set; protected get; }
        #region IBatchStartPoint Members

        public void Execute() {
            //AsyncService.RunAsync
            //    (domainObjectContainer => { });
            //IList<Zone> zones = AreaRepository.AllZones().ToList();
            //Console.ReadLine();
            MqttClientFacade.MQTTClientInstance(false);
        }

        #endregion
    }
}