using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service.Interfaces;
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
        private readonly IRepositoryAsync<PumpStation> _pumpRepository;
        #endregion

        public ConfigurationService(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _zoneRepository = _unitOfWorkAsync.RepositoryAsync<Zone>();
            _dmaRepository = _unitOfWorkAsync.RepositoryAsync<DMA>();
            _pumpRepository = _unitOfWorkAsync.RepositoryAsync<PumpStation>();
        }

        public Zone AddZone(ZoneEntity entity)
        {
            Mapper.CreateMap<ZoneEntity, Zone>();
            Mapper.CreateMap<AddressEntity, Address>();
            Zone zone = Mapper.Map<ZoneEntity, Zone>(entity);
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                zone.AuditField = new AuditFields(null, DateTime.Now, null, null);
                zone.ObjectState = ObjectState.Added;
                _zoneRepository.Insert(zone);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return zone;
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
                return zone;
            }
        }

        public void EditZone(ZoneEntity entity)
        {
            Mapper.CreateMap<Zone, ZoneEntity>();
            Zone zone = Mapper.Map<ZoneEntity, Zone>(entity);
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

        public DMA AddDMA( DMA dma)
        {
            _unitOfWorkAsync.BeginTransaction();
            try
            {
                dma.AuditField = new AuditFields(null, DateTime.Now, null, null);
                dma.ObjectState = ObjectState.Added;
                _dmaRepository.Insert(dma);
                var changes = _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return dma;
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
                return dma;
            }
        }

        //public Zone FindZone(int zoneId)
        //{
        //    return _zoneRepository
        //        .Query(u => u.ZoneId ==zoneId)
        //        .Select()
        //        .FirstOrDefault();
        //}

        public List<ZoneEntity> GetAllZone()
        {
            Mapper.CreateMap<Zone, ZoneEntity>();


            IEnumerable<Zone> zoneList = _zoneRepository
                .Query(u => u.Id > 0)
                .Select();

            return  Mapper.Map<IEnumerable<Zone>, IEnumerable<ZoneEntity>>(zoneList).ToList();
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

        //public PumpStation AddPumpStation( PumpStation pump)
        //{
        //    _unitOfWorkAsync.BeginTransaction();
        //    try
        //    {
        //        pump.AuditField = new AuditFields(null, DateTime.Now, null, null);
        //        pump.ObjectState = ObjectState.Added;
        //        _pumpRepository.Insert(pump);
        //        var changes = _unitOfWorkAsync.SaveChanges();
        //        _unitOfWorkAsync.Commit();
        //        return pump;
        //    }
        //    catch (Exception ex)
        //    {
        //        _unitOfWorkAsync.Rollback();
        //        return pump;
        //    }
        //}

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
    }
}
