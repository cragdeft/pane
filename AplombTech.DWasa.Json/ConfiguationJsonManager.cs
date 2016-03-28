using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Entity.JsonCommandEntity;
using AplombTech.DWasa.Model.ModelDataContext;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service;
using AplombTech.DWasa.Service.Interfaces;
using AplombTech.DWasa.Utility.Enums;
using Newtonsoft.Json;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.UnitOfWork;

namespace AplombTech.DWasa.Json
{
    public class ConfiguationJsonManager
    {
        private IUnitOfWorkAsync _unitOfWorkAsync;
        private IConfigurationService _configurationService;
        private ICommandJsonService _commandJsonService;
        public ConfigurationCommandJsonEntity ConfigurationCommandJson { get; set; }
        public CommandJsonEntity CommandJson { get; }

        public ConfiguationJsonManager(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            
        }

        private T JsonDesrialized<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public ConfiguationJsonManager(string jsonString, CommandType type)
        {
            CommandJson = new CommandJsonEntity();
            CommandJson.IsProcessed = true;
            ConfigureJson(jsonString, type);
            InitializeRepositories();
        }

        private void ConfigureJson(string jsonString, CommandType type)
        {
            CommandJson.CommandJsonString = jsonString;
            CommandJson.CommandType = type;
            try
            {
                ConfigurationCommandJson = JsonDesrialized<ConfigurationJsonRootObject>(jsonString).ConfigureDataJson;
            }
            catch (Exception ex)
            {
                SetCommandJsonProperty(false, "Failed to Deserialize Json");
            }
        }

        private void SetCommandJsonProperty(bool isProcessed, string msg)
        {
            CommandJson.IsProcessed = isProcessed;
            CommandJson.ProcessFailReason = msg;
        }

        private bool IsPumpStationExists(int pumpStationId)
        {
            return _configurationService.PumpStationExists(pumpStationId);
        }

        private void InitializeRepositories()
        {
            IDataContextAsync context = new DWasaDataContext();
            _unitOfWorkAsync = new UnitOfWork(context);
            _configurationService = new ConfigurationService(_unitOfWorkAsync);
            _commandJsonService = new CommandJsonService(_unitOfWorkAsync);
        }

        public void Parse()
        {
            if (ConfigurationCommandJson != null)
            {
                if (IsPumpStationExists(Convert.ToInt32(ConfigurationCommandJson.PumoStationId)))
                {
                    SavePump();
                    SaveCameras();
                    SaveRouters();
                    SaveSensors();
                }
                else
                {
                    SetCommandJsonProperty(false, "PumpStation not found\n");
                }

            }
            LogCommand();
        }

        private void SaveSensors()
        {
            if (ConfigurationCommandJson.Sensor != null || ConfigurationCommandJson.Sensor.Count > 0)
            {
                try
                {
                    foreach (var sensor in ConfigurationCommandJson.Sensor)
                    {
                        SensorType type = (SensorType)Enum.Parse(typeof(SensorType), sensor.Type);
                        PumpStationSensorEntity entity = new PumpStationSensorEntity() { SensorType = type, Sensor = sensor, PumpStationId = Convert.ToInt32(ConfigurationCommandJson.PumoStationId)};
                        _configurationService.AddSensor(entity);
                    }
                }
                catch (Exception ex)
                {
                    SetCommandJsonProperty(false, "failed to save sensor\n");
                }
            }
        }

        private void SaveRouters()
        {
            if (ConfigurationCommandJson.Router != null || ConfigurationCommandJson.Router.Count > 0)
            {
                try
                {
                    foreach (var router in ConfigurationCommandJson.Router)
                    {
                        PumpStationRouterEntity entity = new PumpStationRouterEntity() { Router = router, PumpStationId = Convert.ToInt32(ConfigurationCommandJson.PumoStationId) };
                        _configurationService.AddRouter(entity);
                    }
                }
                catch (Exception ex)
                {
                    SetCommandJsonProperty(false, "failed to save Router\n");
                }
            }
        }

        private void SaveCameras()
        {
            if (ConfigurationCommandJson.Camera != null || ConfigurationCommandJson.Camera.Count > 0)
            {
                try
                {
                    foreach (var camera in ConfigurationCommandJson.Camera)
                    {
                        PumpStationCameraEntity entity = new PumpStationCameraEntity() { Camera = camera, PumpStationId = Convert.ToInt32(ConfigurationCommandJson.PumoStationId) };
                        _configurationService.AddCamera(entity);
                    }
                }
                catch (Exception ex)
                {
                    SetCommandJsonProperty(false, "failed to save Camera\n");
                }
            }
        }

        private void SavePump()
        {
            if (ConfigurationCommandJson.Pump != null)
            {
                try
                {
                    PumpStationPumpEntity entity = new PumpStationPumpEntity();
                    entity.Pump = ConfigurationCommandJson.Pump;
                    entity.PumpStationId = Convert.ToInt32(ConfigurationCommandJson.PumoStationId);
                    _configurationService.AddPump(entity);
                }
                catch (Exception ex)
                {
                    SetCommandJsonProperty(false, "failed to save pump\n");
                }
            }
        }

        private void LogCommand()
        {
            _commandJsonService.Add(CommandJson);
        }
    }
}
