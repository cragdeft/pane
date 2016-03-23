using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Service.Interfaces;
using Repository.Pattern.UnitOfWork;

namespace AplombTech.DWasa.Service
{
    public class ReportService:IReportService
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private  ISensorService _waterSensorService;


        public ReportService(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _waterSensorService = new WaterSensorService(_unitOfWorkAsync);
        }

        public void GeneratetSeriesDataDaily(ReportEntity model)
        {
            
        }
    }
}
