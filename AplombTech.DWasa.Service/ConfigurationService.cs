using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service.Interfaces;
using AplombTech.DWasa.Utility.Enums;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using AutoMapper;

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
            ConfigMapper();
        }

        private void ConfigMapper()
        {
            var config = new MapperConfiguration(cfg => {
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
                cfg.CreateMap<RouterEntity, Router>();

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
                zone.AuditField = new AuditFields(null, DateTime.Now, null, null);
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
                zone.AuditField = new AuditFields(zone.AuditField.InsertedBy, zone.AuditField.InsertedDateTime, null, DateTime.Now);
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

        public DMAEntity AddDMA( DMAEntity entity)
        {
            DMA dma = mapper.Map<DMAEntity, DMA>(entity);

            Zone zone = new Zone() { Id = entity.Zone.Id};
            dma.Zone = zone;//Mapper.Map<ZoneEntity, Zone>(entity.Zone);

            SaveDMA(dma);
            return entity;
        }

        private void SaveDMA( DMA dma)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                dma.AuditField = new AuditFields(null, DateTime.Now, null, null);
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

        public List<ZoneEntity> GetAllZone()
        {
            IEnumerable<Zone> zoneList = _zoneRepository
                .Query()
                .Select();

            return mapper.Map<IEnumerable<Zone>, IEnumerable<ZoneEntity>>(zoneList).ToList();
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

        public bool IsZoneExists(string name)
        {
            return _zoneRepository
                .Queryable()
                .Where(u => u.Name == name)
                .AsEnumerable().Count() == 0 ? false : true;
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
            
            SavePumpStation(pump);
            return entity;
        }

        private void SavePumpStation( PumpStation pump)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                pump.AuditField = new AuditFields(null, DateTime.Now, null, null);
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

        //public void EditPump(PumpStation pump)
        //{
        //    _unitOfWorkAsync.BeginTransaction();
        //    try
        //    {
        //        pump.AuditField = new AuditFields(pump.AuditField.InsertedBy, pump.AuditField.InsertedDateTime, null, DateTime.Now);
        //        pump.ObjectState = ObjectState.Modified;
        //        _pumpRepository.Update(pump);
        //        var changes = _unitOfWorkAsync.SaveChanges();
        //        _unitOfWorkAsync.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        _unitOfWorkAsync.Rollback();
        //    }
        //}

        //public void DeletePump(int pumpId)
        //{
        //    throw new NotImplementedException();
        //}

        //public DMAEntity AddCamera(CameraEntity entity)
        //{
        //    DMA dma = Mapper.Map<DMAEntity, DMA>(entity);

        //    Zone zone = new Zone() { Id = entity.Zone.Id };
        //    dma.Zone = zone;//Mapper.Map<ZoneEntity, Zone>(entity.Zone);

        //    SaveDMA(dma);
        //    return entity;
        //}

        //public DMAEntity AddRouter(DMAEntity entity)
        //{

        //    Mapper.CreateMap<DMAEntity, DMA>();
        //    Mapper.CreateMap<AddressEntity, Address>();

        //    Mapper.CreateMap<ZoneEntity, Zone>();
        //    DMA dma = Mapper.Map<DMAEntity, DMA>(entity);

        //    Zone zone = new Zone() { Id = entity.Zone.Id };
        //    dma.Zone = zone;//Mapper.Map<ZoneEntity, Zone>(entity.Zone);

        //    SaveDMA(dma);
        //    return entity;
        //}

        public void AddSensor(PumpStationSensorEntity entity)
        {
            Sensor sensor = GetSensor(entity);
            sensor.PumpStation = new PumpStation() { Id = entity.PumpStationId};

            SaveSensor(sensor);
        }

        public void AddCamera(PumpStationCameraEntity entity)
        {
            Camera camera = mapper.Map<CameraEntity, Camera>(entity.Camera);

            camera.PumpStation = new PumpStation() { Id = entity.PumpStationId };
            SaveCamera(camera);
        }

        private void SaveCamera(Camera camera)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                camera.AuditField = new AuditFields(null, DateTime.Now, null, null);
                camera.ObjectState = ObjectState.Added;
                _cameraRepository.Insert(camera);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
            }
        }

        public void AddRouter(PumpStationRouterEntity entity)
        {
            Router router = mapper.Map<RouterEntity, Router>(entity.Router);

            router.PumpStation = new PumpStation() { Id = entity.PumpStationId };
            SaveRouter(router);
        }

        private void SaveRouter(Router router)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                router.AuditField = new AuditFields(null, DateTime.Now, null, null);
                router.ObjectState = ObjectState.Added;
                _routerRepository.Insert(router);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
            }
        }

        public void AddPump(PumpStationPumpEntity entity)
        {
            Pump pump = mapper.Map<PumpEntity, Pump>(entity.Pump);

            pump.PumpStation = new PumpStation() { Id = entity.PumpStationId };
            SavePump(pump);
        }

        private void SavePump(Pump pump)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                pump.AuditField = new AuditFields(null, DateTime.Now, null, null);
                pump.ObjectState = ObjectState.Added;
                _pumpRepository.Insert(pump);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
            }
        }

        private void SaveSensor(Sensor sensor)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                sensor.AuditField = new AuditFields(null, DateTime.Now, null, null);
                sensor.ObjectState = ObjectState.Added;

                _sensorRepository.Insert(sensor);
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
            if (entity.SensorType == SensorType.Cholorination)
            {
                sensor = new CholorinationSensor();
                MapSensorProperty(entity, sensor);
            }

            else if (entity.SensorType == SensorType.Energy)
            {
                sensor = new EnergySensor();
                MapSensorProperty(entity, sensor);
            }

            if (entity.SensorType == SensorType.Pressure)
            {
                sensor = new PressureSensor();
                MapSensorProperty(entity, sensor);
            }

            if (entity.SensorType == SensorType.Production)
            {
                sensor = new ProductionSensor();
                MapSensorProperty(entity, sensor);
            }

            if (entity.SensorType == SensorType.WaterLevel)
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
        }
    }
}
