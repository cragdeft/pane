// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using NakedObjects.Core.Configuration;
using NakedObjects.Persistor.Entity.Configuration;
using NakedObjects.Architecture.Menu;
using NakedObjects.Menu;
using NakedObjects.Meta.Audit;
using NakedObjects.Meta.Authorization;
using NakedObjects.Meta.Profile;
using NakedObjects.Web.Mvc.Models;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Objects;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Facade;
using AplombTech.WMS.QueryModel.Repositories;
using AplombTech.WMS.QueryModel.Facade;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.UserAccounts;

namespace AplombTech.WMS.Site {

    // Use this class to configure the application running under Naked Objects
    public class NakedObjectsRunSettings {

	   //Returning e.g. "restapi" creates the Restful Objects API on that root.
	   //Returning "" creates the Restful Objects API at the top level
	   //Returning null means the Restful Objects API will not be generated
	   public static string RestRoot {
            get { return null; }  
        }

		private static string[] ModelNamespaces { 
            get {
                return new string[] { "AplombTech.WMS.Domain", "AplombTech.WMS.QueryModel" }; //Add top-level namespace(s) that cover the domain model
            }			
		}
        
        private static Type[] Services {
            get {
                return new Type[] {
                    typeof(AreaRepository),
                    typeof(ReportRepository),
                    typeof(AlertConfigurationRepository),
                    typeof(ProcessRepository),
                    typeof(UserAccountRepository)
                };
            }
        }

		private static Type[] Types {
            get {
                return new Type[] {
                    //These types must be registered because they are defined in
                    //NakedObjects.Mvc, not in Core.
                    typeof (ActionResultModelQ<>),
                    typeof (ActionResultModel<>),
					typeof (PropertyViewModel),
                    typeof (FindViewModel),
                    typeof(AplombTech.WMS.Domain.Sensors.Sensor),
                    typeof(AplombTech.WMS.Domain.Sensors.FlowSensor),
                    typeof(AplombTech.WMS.Domain.Sensors.ChlorinationSensor),
                    typeof(AplombTech.WMS.Domain.Sensors.EnergySensor),
                    typeof(AplombTech.WMS.Domain.Sensors.LevelSensor),
                    typeof(AplombTech.WMS.Domain.Sensors.PressureSensor),
                    typeof(AplombTech.WMS.Domain.Sensors.SensorData),
                    typeof(AplombTech.WMS.Domain.Sensors.DataLog),
                    typeof(AplombTech.WMS.QueryModel.Sensors.Sensor),
                    typeof(AplombTech.WMS.QueryModel.Sensors.FlowSensor),
                    typeof(AplombTech.WMS.QueryModel.Sensors.ChlorinationSensor),
                    typeof(AplombTech.WMS.QueryModel.Sensors.EnergySensor),
                    typeof(AplombTech.WMS.QueryModel.Sensors.LevelSensor),
                    typeof(AplombTech.WMS.QueryModel.Sensors.PressureSensor),
                    typeof(AplombTech.WMS.QueryModel.Sensors.SensorData),
                    typeof(UserRoles)

                    //Add any domain types that cannot be reached by traversing model from the registered services
                };
            }
        }

        public static ReflectorConfiguration ReflectorConfig() {
            return new ReflectorConfiguration(Types, Services, ModelNamespaces, MainMenus);
        }

        public static EntityObjectStoreConfiguration EntityObjectStoreConfig() {
            var config = new EntityObjectStoreConfiguration();
            config.UsingCodeFirstContext(() => new CommandModelDatabase());
            config.UsingCodeFirstContext(() => new QueryModelDatabase());
            config.SpecifyTypesNotAssociatedWithAnyContext(() => new[] { typeof(PropertyViewModel), typeof(FindViewModel) });
			return config;
        }

		public static IAuditConfiguration AuditConfig() {
            return null; //No auditing set up
			//Example:
            //var config = new AuditConfiguration<MyDefaultAuditor>();
            //config.AddNamespaceAuditor<FooAuditor>("MySpace.Foo");
            //return config;
        }

		public static IAuthorizationConfiguration AuthorizationConfig() {
            return null; //No authorization set up
			//Example:
			//var config = new AuthorizationConfiguration<MyDefaultAuthorizer>();
    		//config.AddTypeAuthorizer<Foo, FooAuthorizer>();
			//config.AddNamespaceAuthorizer<BarAuthorizer>("MySpace.Bar");
			//return config;
        }

		public static IProfileConfiguration ProfileConfig() {
            return null;
			//Example:
			//var events = new HashSet<ProfileEvent>() { ProfileEvent.ActionInvocation }; //etc
            //return new ProfileConfiguration<MyProfiler>() { EventsToProfile = events };
        }

		/// <summary>
        /// Return an array of IMenus (obtained via the factory, then configured) to
        /// specify the Main Menus for the application. If none are returned then
        /// the Main Menus will be derived automatically from the Services.
        /// </summary>
		public static IMenu[] MainMenus(IMenuFactory factory) {
            var areaMenu = factory.NewMenu<AreaRepository>();
            AreaRepository.Menu(areaMenu);

            var reportMenu = factory.NewMenu<ReportRepository>();
            ReportRepository.Menu(reportMenu);

            var reportAlert = factory.NewMenu<AlertConfigurationRepository>();
            AlertConfigurationRepository.Menu(reportAlert);

            var userAccounts = factory.NewMenu<UserAccountRepository>();
            UserAccountRepository.Menu(userAccounts);
            

            return new IMenu[] {
                //factory.NewMenu<CentralKhelaGharRepository>(true),
                areaMenu,
                reportAlert,
                userAccounts,
                reportMenu
            };
        }
    }
}