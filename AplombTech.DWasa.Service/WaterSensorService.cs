using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service.Interfaces;
using AutoMapper;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;

namespace AplombTech.DWasa.Service
{
    public class WaterSensorService:ISensorService
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IRepositoryAsync<WaterLevelSensor> _waterLevelSensorRepository;
        private IMapper mapper;

        public WaterSensorService(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _waterLevelSensorRepository = _unitOfWorkAsync.RepositoryAsync<WaterLevelSensor>();
            ConfigMapper();
        }

        private void ConfigMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WaterLevelSensor, WaterLevelSensorEntity>();

            });

            mapper = config.CreateMapper();
        }

        public IEnumerable<SensorEntity> GetSensorData(int pumpStationId)
        {
            IEnumerable<WaterLevelSensor> list = _waterLevelSensorRepository
                .Query(x=>x.PumpStation.Id == pumpStationId)
                .Select();

            return mapper.Map<IEnumerable<WaterLevelSensor>, IEnumerable<WaterLevelSensorEntity>>(list).ToList();
        }
    }
}
