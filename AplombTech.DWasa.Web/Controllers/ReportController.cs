using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Service.Interfaces;
using AplombTech.DWasa.Utility.Enums;
using Repository.Pattern.UnitOfWork;

namespace AplombTech.DWasa.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IConfigurationService _configurationService;

        public ReportController(IUnitOfWorkAsync unitOfWorkAsync, IConfigurationService configurationService)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _configurationService = configurationService;
        }
        // GET: Graph
        public ActionResult Show()
        {
            ReportEntity model = new ReportEntity();
            model.PumpStation.PumpStationList = _configurationService.GetAllPumpStation();
            return View(model);
        }

        public JsonResult GetOverViewDataOfPumpStation(int pumpStationId)
        {
            List<SensorStatusEntity> sensorStatusList = _configurationService.GetOverViewDataOfPumpStation(pumpStationId);

            return Json(new { Data = sensorStatusList,IsSuccess=true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleSensorStatus(int sensorId)
        {
            SensorStatusEntity sensorStatus = _configurationService.GetSinleSensorStatus(sensorId);

            return Json(new { Data = sensorStatus, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPumpStationLocation(int pumpStationId)
        {
            PumpStationEntity pumpStation = _configurationService.FindPumpStation(pumpStationId);
            
            return Json(new { Data = pumpStation, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Show(ReportEntity model)
        {
            model = _configurationService.GetReportData(model);
            model.PumpStation.PumpStationList = _configurationService.GetAllPumpStation();
            return View(model);
        }

        private string GeneratetSeriesDataHourly()//list of device value will passed
        {
            string series = @"[{
                    name: 'FT',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }, {
                    name: 'FT2',
                    data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
                }]";


            return series;
        }

        //private string GeneratetSeriesDataDaily(ReportEntity model)//list of device value will passed
        //{

        //    //ToDo move to report service
        //    if(model.SensorType == SensorType.WaterLevel)
        //    {
        //        model.FromDateTime =Convert.ToDateTime("1/1/2016");
        //        string data = "data:[";
        //        string name = "";
        //        List<WaterLevelSensorEntity> sensorList = _configurationService.GetPumpStationWaterLevelSensor(model.PumpStation.PumpStationId);
        //        foreach (var waterLevelSensorEntity in sensorList)
        //        {
        //            name = "\'"+waterLevelSensorEntity.Name+"\',";
        //            for (int i = 0; i < 24; i++)
        //            {
        //                double avgValue = _configurationService.GetWaterSensorDataHourly(model.FromDateTime.AddHours(i),
        //                    model.FromDateTime.AddHours(1), waterLevelSensorEntity.Id);
        //                data += avgValue.ToString() + ',';
        //            }
        //            data = data.Remove(data.Length - 1)+"]";
        //        }
        //    }

        //    string series = @"[{
        //            name: 'FT',
        //            data: [0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6,7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        //        }, {
        //            name: 'FT2',
        //            data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5,-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
        //        }]";


        //    return series;
        //}

        private string GeneratetSeriesDataMonthly()
        {
            string series = @"[{
                    name: 'FT',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6,7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6,25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }, {
                    name: 'FT2',
                    data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5,-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5,25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }]";


            return series;
        }

        private string GeneratetSeriesDataRealTime()
        {
            string series = @"[{
                    name: 'FT',
                    data: [7.0]
                }, {
                    name: 'FT2',
                    data: [-0.2]
                }]";


            return series;
        }
    }
}