using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Entity.JsonCommandEntity;
using AplombTech.DWasa.Model.ModelDataContext;
using AplombTech.DWasa.Service;
using AplombTech.DWasa.Service.Interfaces;
using AplombTech.DWasa.Utility.Enums;
using Newtonsoft.Json;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.UnitOfWork;
using SensorData = AplombTech.DWasa.Entity.SensorData;
using SensorDataJsonEntity = AplombTech.DWasa.Entity.SensorDataJsonEntity;
using SensorJsonRootObject = AplombTech.DWasa.Entity.SensorJsonRootObject;

namespace AplombTech.DWasa.Json
{
    public class SensorDataJsonManager
    {
        #region Property

        private IUnitOfWorkAsync _unitOfWorkAsync;
        private IConfigurationService _configurationService;
        private ICommandJsonService _commandJsonService;
        public SensorDataJsonEntity SensorDataCommandJson { get; set; }
        public CommandJsonEntity CommandJson { get; }

        public SensorDataJsonManager(string jsonString,CommandType type)
        {
            CommandJson = new CommandJsonEntity();
            ConfigureJson(jsonString, type);

            InitializeRepositories();
        } 
        #endregion

        #region Public Methods
        public void Parse()
        {
            if (SensorDataCommandJson != null)
            {
                if(IsPumpStationExists(Convert.ToInt32(SensorDataCommandJson.PumoStationId)))
                    SaveData();
                else
                {
                    SetCommandJsonProperty(false, "PumpStation not found\n");
                }
                
            }
            LogCommand();
        }

        private bool IsPumpStationExists(int pumpStationId)
        {
            return _configurationService.PumpStationExists(pumpStationId);
        }

        private void SaveData()
        {
            foreach (var sensorData in SensorDataCommandJson.SensorData)
            {
                try
                {
                    DeviceEntity sensor = _configurationService.GetSensor(sensorData.UId);
                    if (sensor != null)
                    {
                        _configurationService.SaveSensorStatus(PrepareSensorStatus(sensor, sensorData));
                    }
                    else
                    {
                        SetCommandJsonProperty(false, "Sensor id=" + sensor.Id.ToString() + " not found\n");
                    }
                }
                catch (Exception ex)
                {
                    SetCommandJsonProperty(false, "unable to save in DB\n");
                }
            }
        }

        private SensorStatusEntity PrepareSensorStatus(DeviceEntity sensor, SensorData sensorData)
        {
            var sensorStatus = new SensorStatusEntity();
            sensorStatus.Device = new DeviceEntity() {Id = sensor.Id};
            sensorStatus.Value = Convert.ToDouble(sensorData.value);
            sensorStatus.LogDateTime = SensorDataCommandJson.LogDateTime;
            return sensorStatus;
        }

        #endregion

        #region Private Methods
        private void ConfigureJson(string jsonString, CommandType type)
        {
            CommandJson.CommandJsonString = jsonString;
            CommandJson.CommandType = type;
            try
            {
                SensorDataCommandJson = JsonDesrialized<SensorJsonRootObject>(jsonString).SensorDataJson;
            }
            catch (Exception ex)
            {
                SetCommandJsonProperty(false,"failed to deserialize\n");
            }
        }

        private void LogCommand()
        {
            _commandJsonService.Add(CommandJson);
        }

        private void SetCommandJsonProperty(bool isProcessed,string msg)
        {
            CommandJson.IsProcessed = isProcessed;
            CommandJson.ProcessFailReason = msg;
        }

        private void InitializeRepositories()
        {
            IDataContextAsync context = new DWasaDataContext();
            _unitOfWorkAsync = new UnitOfWork(context);
            _configurationService = new ConfigurationService(_unitOfWorkAsync);
            _commandJsonService = new CommandJsonService(_unitOfWorkAsync);
        }

        private T JsonDesrialized<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        } 
        #endregion


    }
}
