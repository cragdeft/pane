using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Model.ModelDataContext;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service.Interfaces;
using AplombTech.DWasa.Utility.Enums;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using AutoMapper;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;

namespace AplombTech.DWasa.Service
{
    public class ConfigurationService : IConfigurationService
    {
        #region PrivateProperty
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IRepositoryAsync<Zone> _zoneRepository;
        private readonly IRepositoryAsync<DMA> _dmaRepository;
        private readonly IRepositoryAsync<PumpStation> _pumpStationRepository;
        private readonly IRepositoryAsync<Sensor> _sensorRepository;
        private readonly IRepositoryAsync<Camera> _cameraRepository;
        private readonly IRepositoryAsync<Router> _routerRepository;
        private readonly IRepositoryAsync<Pump> _pumpRepository;
        private readonly IRepositoryAsync<SensorStatus> _sensorStatusRepository;
        private readonly IRepositoryAsync<WaterLevelSensor> _waterLevelSensorRepository;
        private readonly IRepositoryAsync<ProductionSensor> _productionSensorRepository;
        private readonly IRepositoryAsync<PressureSensor> _pressureSensorRepository;
        private readonly IRepositoryAsync<EnergySensor> _energySensorrRepository;
        private readonly IRepositoryAsync<CholorinationSensor> _cholorinationSensorRepository;
        private readonly string _name;
        private IMapper mapper;
        #endregion

        public ConfigurationService(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _zoneRepository = _unitOfWorkAsync.RepositoryAsync<Zone>();
            _dmaRepository = _unitOfWorkAsync.RepositoryAsync<DMA>();
            _pumpStationRepository = _unitOfWorkAsync.RepositoryAsync<PumpStation>();
            _sensorRepository = _unitOfWorkAsync.RepositoryAsync<Sensor>();
            _cameraRepository = _unitOfWorkAsync.RepositoryAsync<Camera>();
            _routerRepository = _unitOfWorkAsync.RepositoryAsync<Router>();
            _pumpRepository = _unitOfWorkAsync.RepositoryAsync<Pump>();
            _sensorStatusRepository = _unitOfWorkAsync.RepositoryAsync<SensorStatus>();
            _waterLevelSensorRepository = _unitOfWorkAsync.RepositoryAsync<WaterLevelSensor>();
            _productionSensorRepository = _unitOfWorkAsync.RepositoryAsync<ProductionSensor>();
            _pressureSensorRepository = _unitOfWorkAsync.RepositoryAsync<PressureSensor>();
            _energySensorrRepository = _unitOfWorkAsync.RepositoryAsync<EnergySensor>();
            _cholorinationSensorRepository = _unitOfWorkAsync.RepositoryAsync<CholorinationSensor>();
            //ToDO name will assign from login info
            _name = "admin"; 
            ConfigMapper();
        }

        private void ConfigMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ZoneEntity, Zone>();
                cfg.CreateMap<Zone, ZoneEntity>();
                cfg.CreateMap<AddressEntity, Address>();
                cfg.CreateMap<DMAEntity, DMA>();
                cfg.CreateMap<DMA, DMAEntity>();
                cfg.CreateMap<Address, AddressEntity>();
                cfg.CreateMap<PumpStation, PumpStationEntity>();
                cfg.CreateMap<PumpStationEntity, PumpStation>();
                cfg.CreateMap<CameraEntity, Camera>();
                cfg.CreateMap<RouterEntity, Router>();
                cfg.CreateMap<PumpEntity, Pump>();
                cfg.CreateMap<Device, DeviceEntity>();
                cfg.CreateMap<SensorStatus, SensorStatusEntity>();
                cfg.CreateMap<SensorStatusEntity, SensorStatus>();
                cfg.CreateMap<WaterLevelSensor, WaterLevelSensorEntity>();
                cfg.CreateMap<ProductionSensor, ProductionSensorEntity>();
                cfg.CreateMap<PressureSensor, PressureSensorEntity>();
                cfg.CreateMap<EnergySensor, EnergySensorEntity>();
                cfg.CreateMap<CholorinationSensor, CholorinationSensorEntity>();

            });

            mapper = config.CreateMapper();
        }

        public ZoneEntity AddZone(ZoneEntity entity)
        {
            Zone zone = mapper.Map<ZoneEntity, Zone>(entity);
            SaveZoneEntity(zone);
            return entity;
        }

        private void SaveZoneEntity(Zone zone)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                zone.AuditField = new AuditFields(_name, DateTime.Now, _name, DateTime.Now);
                zone.ObjectState = ObjectState.Added;
                _zoneRepository.Insert(zone);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
            }
        }

        public void EditZone(ZoneEntity entity)
        {
            Zone zone = mapper.Map<ZoneEntity, Zone>(entity);
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                zone.AuditField = new AuditFields(zone.AuditField.InsertedBy, zone.AuditField.InsertedDateTime, _name, DateTime.Now);
                zone.ObjectState = ObjectState.Modified;
                _zoneRepository.Update(zone);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception)
            {
                _unitOfWorkAsync.Rollback();
            }
        }

        public void DeleteZone(Zone zone)
        {
            throw new NotImplementedException();
        }

        public DMAEntity AddDMA(DMAEntity entity)
        {
            DMA dma = mapper.Map<DMAEntity, DMA>(entity);

            Zone zone = new Zone() { Id = entity.Zone.Id };
            dma.Zone = zone;

            SaveDMA(dma);
            return entity;
        }

        private void SaveDMA(DMA dma)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                dma.AuditField = new AuditFields(_name, DateTime.Now, _name, DateTime.Now); ;
                dma.ObjectState = ObjectState.Added;

                _dmaRepository.Insert(dma);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
            }
        }



        public ZoneEntity FindZone(int zoneId)
        {
            Zone zone = _zoneRepository
                .Query(u => u.Id == zoneId)

                .Select()

                .FirstOrDefault();

            return mapper.Map<Zone, ZoneEntity>(zone);
        }

        public PumpStationEntity FindPumpStation(int pumpStationId)
        {
            PumpStation pumpStation = _pumpStationRepository
                .Query(u => u.Id == pumpStationId)

                .Select()

                .FirstOrDefault();

            return mapper.Map<PumpStation, PumpStationEntity>(pumpStation);
        }

        public List<ZoneEntity> GetAllZone()
        {
            IEnumerable<Zone> zoneList = _zoneRepository
                .Query()
                .Select();

            return mapper.Map<IEnumerable<Zone>, IEnumerable<ZoneEntity>>(zoneList).ToList();
        }

        public List<ZoneEntity> GetAll()
        {
            IEnumerable<Zone> zoneList = _zoneRepository
                .Query()
                .Select();

            List<ZoneEntity> zoneEntityList = mapper.Map<IEnumerable<Zone>, IEnumerable<ZoneEntity>>(zoneList).ToList();

            foreach (var zoneEntity in zoneEntityList)
            {
                IEnumerable<DMA> dmaList = _dmaRepository
                .Query(x => x.Zone.Id == zoneEntity.Id)
                .Select();
                List<DMAEntity> dmaEntityList = mapper.Map<IEnumerable<DMA>, IEnumerable<DMAEntity>>(dmaList).ToList();
                zoneEntity.DMAList = dmaEntityList;
                foreach (var dmaEntity in dmaEntityList)
                {
                    IEnumerable<PumpStation> pumpStationList = _pumpStationRepository
                    .Query(y => y.DMA.Id == dmaEntity.Id)
                    .Select();

                    List<PumpStationEntity> pumpStationEntityList = mapper.Map<IEnumerable<PumpStation>, IEnumerable<PumpStationEntity>>(pumpStationList).ToList();
                    dmaEntity.PumpStationList = pumpStationEntityList;
                    foreach (var pumpStationEntity in pumpStationEntityList)
                    {
                        var wList = GetWaterLevelSensorsList(pumpStationEntity);

                        pumpStationEntity.WaterLevelSensorList = mapper.Map<IEnumerable<WaterLevelSensor>, IEnumerable<WaterLevelSensorEntity>>(wList).ToList();

                        var prList = GetPressureSensorList(pumpStationEntity);

                        pumpStationEntity.PressureSensorList = mapper.Map<IEnumerable<PressureSensor>, IEnumerable<PressureSensorEntity>>(prList).ToList();

                        var pdList = GetProductionSensorsList(pumpStationEntity);

                        pumpStationEntity.ProductionSensorList = mapper.Map<IEnumerable<ProductionSensor>, IEnumerable<ProductionSensorEntity>>(pdList).ToList();

                        var eList = GetEnergySensorsList(pumpStationEntity);

                        pumpStationEntity.EnergySensorList = mapper.Map<IEnumerable<EnergySensor>, IEnumerable<EnergySensorEntity>>(eList).ToList();

                        var cList = GetCholorinationSensorsList(pumpStationEntity);

                        pumpStationEntity.CholorinationSensorList = mapper.Map<IEnumerable<CholorinationSensor>, IEnumerable<CholorinationSensorEntity>>(cList).ToList();

                    }
                }
            }

            return zoneEntityList;
        }

        private IEnumerable<CholorinationSensor> GetCholorinationSensorsList(PumpStationEntity pumpStationEntity)
        {
            IEnumerable<CholorinationSensor> cList = _cholorinationSensorRepository
                .Query(z => z.PumpStation.Id == pumpStationEntity.Id)
                .Select();
            return cList;
        }

        private IEnumerable<EnergySensor> GetEnergySensorsList(PumpStationEntity pumpStationEntity)
        {
            IEnumerable<EnergySensor> eList = _energySensorrRepository
                .Query(z => z.PumpStation.Id == pumpStationEntity.Id)
                .Select();
            return eList;
        }

        private IEnumerable<ProductionSensor> GetProductionSensorsList(PumpStationEntity pumpStationEntity)
        {
            IEnumerable<ProductionSensor> pdList = _productionSensorRepository
                .Query(z => z.PumpStation.Id == pumpStationEntity.Id)
                .Select();
            return pdList;
        }

        private IEnumerable<PressureSensor> GetPressureSensorList(PumpStationEntity pumpStationEntity)
        {
            IEnumerable<PressureSensor> prList = _pressureSensorRepository
                .Query(z => z.PumpStation.Id == pumpStationEntity.Id)
                .Select();
            return prList;
        }

        private IEnumerable<WaterLevelSensor> GetWaterLevelSensorsList(PumpStationEntity pumpStationEntity)
        {
            IEnumerable<WaterLevelSensor> wList = _waterLevelSensorRepository
                .Query(z => z.PumpStation.Id == pumpStationEntity.Id)
                .Select();
            return wList;
        }

        public List<DMAEntity> GetAllDMA()
        {
            IEnumerable<DMA> dmaList = _dmaRepository
                .Query()
                .Select();

            return mapper.Map<IEnumerable<DMA>, IEnumerable<DMAEntity>>(dmaList).ToList();
        }

        public List<PumpStationEntity> GetAllPumpStation()
        {
            IEnumerable<PumpStation> pumpStationList = _pumpStationRepository
                .Query()
                .Select();

            return mapper.Map<IEnumerable<PumpStation>, IEnumerable<PumpStationEntity>>(pumpStationList).ToList();
        }

        public List<DeviceEntity> GetAllDevice()
        {
            IEnumerable<Device> deviceList = _sensorRepository
                .Query()
                .Select();

            return mapper.Map<IEnumerable<Device>, IEnumerable<DeviceEntity>>(deviceList).ToList();
        }

        public List<DeviceEntity> GetPumpStationDevice(int pumpStationId)
        {
            IEnumerable<Device> deviceList = _sensorRepository
                .Query(x=>x.PumpStation.Id == pumpStationId)
                .Select();

            return mapper.Map<IEnumerable<Device>, IEnumerable<DeviceEntity>>(deviceList).ToList();
        }

        public bool PumpStationExists(int pumpStationId)
        {
            var isExists = true;
            //return true;
            //return _pumpStationRepository
            //    .Queryable()
            //    .Where(u => u.Id == pumpStationId)
            //    .AsEnumerable().Count() != 0;

            #region MyRegion
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<PumpStation> pumpStationRepository = new Repository<PumpStation>(context, unitOfWork);
                isExists = pumpStationRepository
                .Queryable()
                .Where(u => u.Id == pumpStationId)
                .AsEnumerable().Count() != 0;

            }
            #endregion

            return isExists;
        }

        public List<WaterLevelSensorEntity> GetPumpStationWaterLevelSensor(int pumpStationId)
        {
                IEnumerable<WaterLevelSensor> deviceList = _waterLevelSensorRepository
               .Query(x => x.PumpStation.Id == pumpStationId)
               .Select();

                return mapper.Map<IEnumerable<WaterLevelSensor>, IEnumerable<WaterLevelSensorEntity>>(deviceList).ToList();
            
        }




        public bool IsZoneExists(string name)
        {
            return _zoneRepository
                .Queryable()
                .Where(u => u.Name == name)
                .AsEnumerable().Count() != 0;
        }

        //public DMA FindDMA(int dmaId)
        //{
        //    return _dmaRepository
        //        .Query(u => u.DMAId == dmaId)
        //        .Select()
        //        .FirstOrDefault();
        //}

        //public PumpStation FindPumpStation(int pumpStationId)
        //{
        //    return _pumpRepository
        //        .Query(u => u.PumpId == pumpStationId)
        //        .Select()
        //        .FirstOrDefault();
        //}

        //public void EditDMA( DMA dma)
        //{
        //    _unitOfWorkAsync.BeginTransaction();
        //    try
        //    {
        //        dma.AuditField = new AuditFields(dma.AuditField.InsertedBy, dma.AuditField.InsertedDateTime, null, DateTime.Now);
        //        dma.ObjectState = ObjectState.Modified;
        //        _dmaRepository.Update(dma);
        //        var changes = _unitOfWorkAsync.SaveChanges();
        //        _unitOfWorkAsync.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        _unitOfWorkAsync.Rollback();
        //    }
        //}

        //public void DeleteDMA(int dmaId)
        //{
        //    throw new NotImplementedException();
        //}


        public PumpStationEntity AddPumpStation(PumpStationEntity entity)
        {
            PumpStation pump = mapper.Map<PumpStationEntity, PumpStation>(entity);
            DMA dma = new DMA() { Id = entity.DMA.Id };
            pump.DMA = dma;
            SavePumpStation(pump);
            return entity;
        }

        private void SavePumpStation(PumpStation pump)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                pump.AuditField = new AuditFields(_name, DateTime.Now, _name, DateTime.Now); ;
                pump.ObjectState = ObjectState.Added;
                _pumpStationRepository.Insert(pump);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
            }
        }

        public List<SensorStatusEntity> GetOverViewDataOfPumpStation(int pumpStationId)
        {
            List<SensorStatusEntity> sensorStatusEntities = new List<SensorStatusEntity>();
            List<DeviceEntity> deviceList = GetPumpStationDevice(pumpStationId);

            foreach (var deviceEntity in deviceList)
            {
                 SensorStatus sensorStatus = _sensorStatusRepository
                .Query(x=>x.Device.Id==deviceEntity.Id)
                .Select().FirstOrDefault();
                sensorStatusEntities.Add(mapper.Map<SensorStatus,SensorStatusEntity> (sensorStatus));
            }
            return sensorStatusEntities;

        }

        public double GetAverageSensorData(DateTime from, DateTime to,int deviceId)
        {
            IEnumerable<SensorStatus> deviceList = _sensorStatusRepository.Query(
                x => (x.Device.Id == deviceId && x.LogDateTime >= from && x.LogDateTime <= to))
                .Select();
            if (deviceList!= null && deviceList.Count() == 0)
                return 0;
            return deviceList.Average(x=>x.Value);

        }

        public ReportEntity GetReportData(ReportEntity model)
        {
            if (model.ReportType == ReportType.Daily)
            {
                model.ToDateTime = new DateTime(model.Year,(int)model.Month,model.Day);
                return GeneratetSeriesDataDaily(model);
            }

            else if (model.ReportType == ReportType.Monthly)
            {
                model.ToDateTime = new DateTime(model.Year, (int)model.Month, 1);
                return GeneratetSeriesDataMonthly(model);
            }
            else if (model.ReportType == ReportType.Hourly)
            {
                model.ToDateTime = new DateTime(model.Year, (int)model.Month, model.Day, model.Hour-1, 0, 0);
                return GeneratetSeriesDataHourly(model);
            }
            else if (model.ReportType == ReportType.Weekly)
            {
                model.ToDateTime = new DateTime(model.Year, 1, 1).AddDays((model.Week-1)*7);
                return GeneratetSeriesDataWeekly(model);
            }

            else if (model.ReportType == ReportType.Realtime)
            {
                model.ToDateTime = DateTime.Now;
                return GeneratetSeriesDataRealtime(model);
            }

            return model;
        }
        public ReportEntity GeneratetSeriesDataRealtime(ReportEntity model)
        {

            model.GraphTitle = "Realtime Data Review";
            model.GraphSubTitle = "Data for Minute of " + model.ToDateTime.Minute;


            model.XaxisCategory = new string[1];

            if (model.SensorType == SensorType.FT)
            {
                var sensorList = GetWaterLevelSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetRealTimeData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.CT)
            {
                var sensorList = GetCholorinationSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetRealTimeData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.PT)
            {
                var sensorList = GetProductionSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetRealTimeData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.EM)
            {
                var sensorList = GetEnergySensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetRealTimeData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.LT)
            {

                var sensorList = GetPressureSensorList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetRealTimeData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }
            }

            return model;
        }
        public ReportEntity GeneratetSeriesDataHourly(ReportEntity model)
        {
           
            model.GraphTitle = "Hourly Data Review";
            model.GraphSubTitle = "Data for Hour no=" + model.ToDateTime.Hour;


            model.XaxisCategory = new string[13];

            if (model.SensorType == SensorType.FT)
            {
                var sensorList = GetWaterLevelSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=(GetAvarageHourlyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.CT)
            {
                var sensorList = GetCholorinationSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=(GetAvarageHourlyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.PT)
            {
                var sensorList = GetProductionSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=(GetAvarageHourlyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.EM)
            {
                var sensorList = GetEnergySensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=(GetAvarageHourlyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.LT)
            {

                var sensorList = GetPressureSensorList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=(GetAvarageHourlyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }
            }
            
            return model;
        }

        public ReportEntity GeneratetSeriesDataWeekly(ReportEntity model)
        {

            model.GraphTitle = "Weekly Data Review";
            model.GraphSubTitle = "Data for Week no=" + model.Week;

            model.XaxisCategory = new string[7];

            if (model.SensorType == SensorType.FT)
            {
                var sensorList = GetWaterLevelSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetAvarageWeeklyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.CT)
            {
                var sensorList = GetCholorinationSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetAvarageWeeklyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.PT)
            {

                var sensorList = GetProductionSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetAvarageWeeklyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.EM)
            {
                var sensorList = GetEnergySensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetAvarageWeeklyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.LT)
            {
                var sensorList = GetPressureSensorList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = (GetAvarageWeeklyData(ref model, sensorEntity.Id));
                    model.Series.Add(data);
                }
            }

            return model;
        }
        public ReportEntity GeneratetSeriesDataDaily(ReportEntity model)
        {
            
            model.GraphTitle = "Daily Data Review";
            model.GraphSubTitle = "Data for " + model.ToDateTime.DayOfWeek;

            model.XaxisCategory = new string[25];

            if (model.SensorType == SensorType.FT)
            {
                var sensorList = GetWaterLevelSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId }); 
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=GetAvarageDailyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }
                
            }

            if (model.SensorType == SensorType.CT)
            {
                var sensorList = GetCholorinationSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=GetAvarageDailyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.PT)
            {
                var sensorList = GetProductionSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=GetAvarageDailyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.EM)
            {
                var sensorList = GetEnergySensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=GetAvarageDailyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.LT)
            {
                var sensorList = GetPressureSensorList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=GetAvarageDailyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }
            return model;
        }
        public ReportEntity GeneratetSeriesDataMonthly(ReportEntity model)
        {
            model.GraphTitle = "Monthly Data Review";
            model.GraphSubTitle = "Data for " + model.ToDateTime.ToString("MMM");

            model.XaxisCategory = new string[30];

            if (model.SensorType == SensorType.FT)
            {
                var sensorList = GetWaterLevelSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.CT)
            {
                var sensorList = GetCholorinationSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data=GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.PT)
            {
                var sensorList = GetProductionSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.EM)
            {
                var sensorList = GetEnergySensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.LT)
            {
                var sensorList = GetPressureSensorList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }
            return model;
        }

        public ReportEntity GeneratetSeriesDataRealTime(ReportEntity model)
        {
            model.GraphTitle = "Real time Data Review";

            model.XaxisCategory = new string[30];

            if (model.SensorType == SensorType.FT)
            {
                var sensorList = GetWaterLevelSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.CT)
            {
                var sensorList = GetCholorinationSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.PT)
            {
                var sensorList = GetProductionSensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.EM)
            {
                var sensorList = GetEnergySensorsList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }

            if (model.SensorType == SensorType.LT)
            {
                var sensorList = GetPressureSensorList(new PumpStationEntity() { Id = model.PumpStation.PumpStationId });
                foreach (var sensorEntity in sensorList)
                {
                    model.Unit = sensorEntity.Unit;
                    int id = sensorEntity.Id;
                    ReportSeriesEntity data = new ReportSeriesEntity();
                    data.name = sensorEntity.Name;
                    data.data = GetAvarageMonthlyData(ref model, sensorEntity.Id);
                    model.Series.Add(data);
                }

            }
            return model;
        }
        private List<double> GetRealTimeData(ref ReportEntity model, int id)
        {
            List<double> values = new List<double>();
            double value = 0;
            DateTime from = DateTime.Now.AddMinutes(-5);
            DateTime to = DateTime.Now;
            SensorStatus status = _sensorStatusRepository.Query(
               x => (x.Device.Id == id && x.LogDateTime >= from && x.LogDateTime <= to))
               .Select().FirstOrDefault();
            value = new Random().Next(30);
            //if (status != null)
            //    value = 0;
            //else
            //    value = status.Value;
            values.Add(value);
                model.XaxisCategory[0] = DateTime.Now.ToShortTimeString();
            
            return values;

        }

        private List<double> GetAvarageHourlyData(ref ReportEntity model,int id)
        {
            List<double> avgValue = new List<double>();
            for (int i = 0; i <= 12; i++)
            {
                avgValue.Add(GetAverageSensorData(model.ToDateTime.AddMinutes(i==0?0:i+5),
                    model.ToDateTime.AddMinutes(i == 0 ? 5 : i * 5), id));
                model.XaxisCategory[i] = model.ToDateTime.AddMinutes(i*5).ToShortTimeString();
                
            }
            return avgValue;

        }

        private List<double> GetAvarageWeeklyData(ref ReportEntity model, int id)
        {
            List<double> avgValue = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                avgValue.Add(GetAverageSensorData(model.ToDateTime.AddDays(i),
                    model.ToDateTime.AddDays(i+1), id));
                model.XaxisCategory[i] = (i+1).ToString();

            }
            return avgValue;

        }
        private List<double> GetAvarageDailyData(ref ReportEntity model, int id)
        {
            List < double > avgValue = new List<double>();
            for (int i = 0; i <= 24; i++)
            {
                 avgValue.Add(GetAverageSensorData(model.ToDateTime.AddHours(i),
                    model.ToDateTime.AddHours(i+1), id));
                model.XaxisCategory[i] = model.ToDateTime.AddHours(i).ToShortTimeString();
            }
            return avgValue;
        }
        private List<double> GetAvarageMonthlyData(ref ReportEntity model, int id)
        {
            List<double> avgValue = new List<double>();
            for (int i = 0; i < 30; i++)
            {
                avgValue.Add(GetAverageSensorData(model.ToDateTime.AddDays(i),
                    model.ToDateTime.AddDays(i+1), id));
                model.XaxisCategory[i] = model.ToDateTime.AddDays(i).Day.ToString();
            }
            return avgValue;
        }

        public SensorStatusEntity GetSinleSensorStatus(int sensorId)
        {
            SensorStatus sensorStatus = 
            _sensorStatusRepository
                .Query(x => x.Device.Id == sensorId)
                .Select().FirstOrDefault();

            return mapper.Map<SensorStatus, SensorStatusEntity>(sensorStatus);
        }

        public void AddSensor(PumpStationSensorEntity entity)
        {
            Sensor sensor = GetSensor(entity);
            sensor.PumpStation = new PumpStation() { Id = entity.PumpStationId };

            Sensor device =
       _sensorRepository
           .Query(x => x.UId == entity.Sensor.UId)
           .Select().FirstOrDefault();

            if (device == null)
            {
                SaveSensor(sensor);
            }
            else
            {
                sensor.PumpStation = new PumpStation() { Id = entity.PumpStationId };
                sensor.Id = device.Id;
                sensor.AuditField = device.AuditField;
                UpdateSensor(sensor);
                
            }
        }

        public DeviceEntity GetSensor(string uid)
        {
            Device sensor =
            _sensorRepository
                .Query(x => x.UId == uid)
                .Select().FirstOrDefault();

            return mapper.Map<Device, DeviceEntity>(sensor);
        }

        public void AddCamera(PumpStationCameraEntity entity)
        {
            Camera device =
         _cameraRepository
             .Query(x => x.UId == entity.Camera.UId)
             .Select().FirstOrDefault();

            if (device == null)
            {
                var camera = mapper.Map<CameraEntity, Camera>(entity.Camera);
                camera.PumpStation = new PumpStation() { Id = entity.PumpStationId };
                SaveCamera(camera);
            }
            else
            {
                device.Url = entity.Camera.Url;
                device.Name = entity.Camera.Name;
                device.PumpStation = new PumpStation() { Id = entity.PumpStationId };
                UpdateCamera(device);
            }
        }

        private void SaveCamera(Camera camera)
        {
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Camera> cameraRepository = new Repository<Camera>(context, unitOfWork);
                unitOfWork.BeginTransaction();
                try
                {
                    camera.AuditField = new AuditFields(_name, DateTime.Now, _name, DateTime.Now);
                    camera.ObjectState = ObjectState.Added;
                    cameraRepository.Insert(camera);
                    var changes = unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                }
            }
        }

        private void UpdateCamera(Camera camera)
        {
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Camera> cameraRepository = new Repository<Camera>(context, unitOfWork);
                unitOfWork.BeginTransaction();
                try
                {
                    camera.AuditField = new AuditFields(camera.AuditField.InsertedBy, camera.AuditField.InsertedDateTime,
                        _name, DateTime.Now);
                    camera.ObjectState = ObjectState.Modified;
                    cameraRepository.Update(camera);
                    var changes = unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                }
            }
        }

        public void AddRouter(PumpStationRouterEntity entity)
        {
            Router device =
         _routerRepository
             .Query(x => x.MacId == entity.Router.MacId)
             .Select().FirstOrDefault();

            if (device == null)
            {
                var router = mapper.Map<RouterEntity, Router>(entity.Router);
                router.PumpStation = new PumpStation() { Id = entity.PumpStationId };
                SaveRouter(router);
            }
            else
            {
                device.PumpStation = new PumpStation() { Id = entity.PumpStationId };
                device.Ip = entity.Router.Ip;
                device.Name = entity.Router.Name;
                device.Port = entity.Router.Port;
                UpdateRouter(device);
            }
        }
        
        private void SaveRouter(Router router)
        {
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Router> routerRepository = new Repository<Router>(context, unitOfWork);
                unitOfWork.BeginTransaction();
                try
                {
                    router.AuditField = new AuditFields(_name, DateTime.Now, _name, DateTime.Now);
                    router.ObjectState = ObjectState.Added;
                    routerRepository.Insert(router);
                    var changes = unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                }
            }
        }

        private void UpdateRouter(Router router)
        {
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Router> routerRepository = new Repository<Router>(context, unitOfWork);
                unitOfWork.BeginTransaction();
                try
                {
                    router.AuditField = new AuditFields(router.AuditField.InsertedBy, router.AuditField.InsertedDateTime,
                        _name, DateTime.Now);
                    router.ObjectState = ObjectState.Modified;
                    routerRepository.Update(router);
                    var changes = unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                }
            }
        }

        public void AddPump(PumpStationPumpEntity entity)
        {
            Pump device = _pumpRepository
                .Query(x => x.UId == entity.Pump.UId)
                .Select().FirstOrDefault();

            if (device == null)
            {
                var pump = mapper.Map<PumpEntity, Pump>(entity.Pump);
                pump.PumpStation = new PumpStation() { Id = entity.PumpStationId };
                SavePump(pump);
            }
            else
            {
                device.PumpStation = new PumpStation() { Id = entity.PumpStationId };
                device.Name = entity.Pump.Name;
                UpdatePump(device);
            }
        }

        private void SavePump(Pump pump)
        {
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Pump> pumpRepository = new Repository<Pump>(context, unitOfWork);
                unitOfWork.BeginTransaction();
                try
                {
                    pump.AuditField = new AuditFields(_name, DateTime.Now, _name, DateTime.Now);
                    pump.ObjectState = ObjectState.Added;
                    pumpRepository.Insert(pump);
                    var changes = unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                }
            }
            
        }

        private void UpdatePump(Pump pump)
        {
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Pump> pumpRepository = new Repository<Pump>(context, unitOfWork);
                unitOfWork.BeginTransaction();
                try
                {
                    pump.AuditField = new AuditFields(pump.AuditField.InsertedBy, pump.AuditField.InsertedDateTime,
                        _name, DateTime.Now);
                    pump.ObjectState = ObjectState.Modified;
                    pumpRepository.Update(pump);
                    var changes = unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                }
            }
        }

        private void SaveSensor(Sensor sensor)
        {
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Sensor> sensorRepository = new Repository<Sensor>(context, unitOfWork);
                unitOfWork.BeginTransaction();
                try
                {
                    sensor.AuditField = new AuditFields(_name, DateTime.Now, _name, DateTime.Now);
                    sensor.ObjectState = ObjectState.Added;

                    sensorRepository.Insert(sensor);
                    var changes = unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                }
            }
        }

        private void UpdateSensor(Sensor sensor)
        {
            using (IDataContextAsync context = new DWasaDataContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Sensor> sensorRepository = new Repository<Sensor>(context, unitOfWork);
                unitOfWork.BeginTransaction();
                try
                {
                    sensor.AuditField = new AuditFields(sensor.AuditField.InsertedBy, sensor.AuditField.InsertedDateTime,
                        _name, DateTime.Now);
                    sensor.ObjectState = ObjectState.Modified;

                    sensorRepository.Update(sensor);
                    var changes = unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                }
            }
        }

        public void SaveSensorStatus(SensorStatusEntity sensorStatus)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                var sensor = mapper.Map<SensorStatusEntity, SensorStatus>(sensorStatus);
                sensor.AuditField = new AuditFields(_name, DateTime.Now, _name, DateTime.Now);
                sensor.ObjectState = ObjectState.Added;

                _sensorStatusRepository.Insert(sensor);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
            }
        }

        private Sensor GetSensor(PumpStationSensorEntity entity)
        {
            Sensor sensor = null;
            if (entity.SensorType == SensorType.CT)
            {
                sensor = new CholorinationSensor();
                MapSensorProperty(entity, sensor);
            }

            else if (entity.SensorType == SensorType.EM)
            {
                sensor = new EnergySensor();
                MapSensorProperty(entity, sensor);
            }

            if (entity.SensorType == SensorType.LT)
            {
                sensor = new PressureSensor();
                MapSensorProperty(entity, sensor);
            }

            if (entity.SensorType == SensorType.PT)
            {
                sensor = new ProductionSensor();
                MapSensorProperty(entity, sensor);
            }

            if (entity.SensorType == SensorType.FT)
            {
                sensor = new WaterLevelSensor();
                MapSensorProperty(entity, sensor);
            }

            return sensor;
        }

        private void MapSensorProperty(PumpStationSensorEntity entity, Sensor sensor)
        {
            sensor.Name = entity.Sensor.Name;
            sensor.Id = entity.Sensor.Id;
            sensor.Value = entity.Sensor.Value;
            sensor.UId = entity.Sensor.UId;
        }
        
    }
}
