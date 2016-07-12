using AplombTech.WMS.QueryModel.Repositories;
using NakedObjects;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.QueryModel.Motors;
using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using AplombTech.WMS.Site.Models;
using AplombTech.WMS.Site.MQTT;
using Newtonsoft.Json;
using Motor = AplombTech.WMS.QueryModel.Motors.Motor;
using MotorData = AplombTech.WMS.QueryModel.Motors.MotorData;

namespace AplombTech.WMS.Site.Controllers
{
    [Authorize]
    public class ScadaMapController : SystemControllerImpl
    {
        #region Injected Services
        public ReportRepository _reportRepository { set; protected get; }
        #endregion

        public ScadaMapController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) { }

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        public ScadaMapController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
            : base(facade, idHelper)
        {
            nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        }

        // GET: ScadaMap
        public ActionResult Index()
        {
            //ZoneMap zones = _reportRepository.GoogleMap();
            //int totalZone = zones.Zones.Count();
            return View();
        }

        // GET: ScadaMap/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ScadaMap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScadaMap/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ScadaMap/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ScadaMap/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ScadaMap/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ScadaMap/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetDmaDropdownData(int zoneId)
        {
            List<AplombTech.WMS.QueryModel.Areas.DMA> dmaList = _reportRepository.GetDmaList(zoneId);
            var dictornaty = new Dictionary<int, string>();
            foreach (var dma in dmaList)
            {
                dictornaty.Add(dma.AreaId, dma.Name);
            }

            return Json(new { Data = JsonConvert.SerializeObject(dictornaty), IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPumpStationDropdownData(int dmaId)
        {
            List<AplombTech.WMS.QueryModel.Areas.PumpStation> pumpStationList = _reportRepository.GetPumpStationList(dmaId);
            var dictornaty = new Dictionary<int, string>();
            foreach (var pumpStation in pumpStationList)
            {
                dictornaty.Add(pumpStation.AreaId, pumpStation.Name);
            }

            return Json(new { Data = JsonConvert.SerializeObject(dictornaty), IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowScada(string pumpStationId)
        {
            List<Sensor> sensorList = _reportRepository.GetSensorData(Convert.ToInt32(pumpStationId));
            List<QueryModel.Motors.MotorData> motorDataList = new List<QueryModel.Motors.MotorData>();
            motorDataList.Add(_reportRepository.GetPumpMotorData(Convert.ToInt32(pumpStationId)));
            motorDataList.Add(_reportRepository.GetCholorineMotorData(Convert.ToInt32(pumpStationId)));
            ViewBag.MotorDataList = motorDataList;
            //ScadaViewModel model = new ScadaViewModel() {SensorList = sensorList,MotorDataList = motorDataList };
            return PartialView("~/Views/ScadaMap/ScadaMap.cshtml", sensorList);
        }

        public JsonResult GetScadaData(string pumpStationId)
        {
            try
            {
                List<Sensor> sensorList = _reportRepository.GetSensorData(Convert.ToInt32(pumpStationId));
                var dictornary = new Dictionary<string, string>();
                dictornary = ConvertSensorData(sensorList);
                List<QueryModel.Motors.MotorData> motorDataList = new List<QueryModel.Motors.MotorData>();
                motorDataList.Add(_reportRepository.GetPumpMotorData(Convert.ToInt32(pumpStationId)));
                motorDataList.Add(_reportRepository.GetCholorineMotorData(Convert.ToInt32(pumpStationId)));
                motorDataList = GetMotorDataList(motorDataList);
                //ScadaViewModel model = new ScadaViewModel() {SensorList = sensorList,MotorDataList = motorDataList };
                return Json(new { SensorList = JsonConvert.SerializeObject(dictornary), MotorList = motorDataList, LastDataRecived = sensorList[1].LastDataReceived.ToString(), IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetSensorData(string sensorId)
        {
            try
            {
                if (!string.IsNullOrEmpty(sensorId))
                {
                    var sensor = _reportRepository.GetPumpSingleSensorByUid(sensorId);
                    if (sensor is FlowSensor)
                        sensor.UnitName = "Litre";
                    if (sensor is EnergySensor)
                        sensor.UnitName = "KW";
                    if (sensor != null)
                        return Json(new { IsSuccess = true, Unit = sensor.UnitName, Name=GetName(sensor), Value = sensor.CurrentValue }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

            }

            return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);

        }

        private string GetName(Sensor sensor)
        {
            if (sensor is PressureSensor)
                return "PT-" + sensor.UUID;
            if (sensor is FlowSensor)
                return "FT-" + sensor.UUID;
            if (sensor is EnergySensor)
                return "ET-" + sensor.UUID;
            if (sensor is LevelSensor)
                return "LT-" + sensor.UUID;
            if (sensor is ACPresenceDetector)
                return "ACP-" + sensor.UUID;
            if (sensor is BatteryVoltageDetector)
                return "BV-" + sensor.UUID;
            if (sensor is ChlorinePresenceDetector)
                return "CPD-" + sensor.UUID;

            return string.Empty;
        }

        private List<MotorData> GetMotorDataList(List<MotorData> motorDataList)
        {
            List<MotorData> convertedMotorDataList = new List<MotorData>();
            foreach (var motorData in motorDataList)
            {
                var mdata = new MotorData();
                mdata.Auto = motorData.Auto;
                mdata.LastCommand = motorData.LastCommand;
                mdata.LastCommandTime = motorData.LastCommandTime;
                mdata.MotorStatus = motorData.MotorStatus;
                mdata.Motor = new Motor() { MotorID = motorData.Motor.MotorID, UUID = motorData.Motor.UUID };
                mdata.Motor.MotorID = motorData.Motor.MotorID;

                convertedMotorDataList.Add(mdata);
            }

            return convertedMotorDataList;
        }

        private Dictionary<string, string> ConvertSensorData(List<Sensor> sensorList)
        {
            var dictornary = new Dictionary<string, string>();
            foreach (var sensor in sensorList)
            {
                if (sensor is FlowSensor) { 
                    dictornary.Add("FTC_"+sensor.UUID, ((FlowSensor)sensor).CurrentValue.ToString()+" Litre");
                    dictornary.Add("FTQ_" + sensor.UUID, ((FlowSensor)sensor).CumulativeValue.ToString() + " Litre");
                }

                if (sensor is EnergySensor) { 
                    dictornary.Add("ETC_" + sensor.UUID, ((EnergySensor)sensor).CurrentValue.ToString()+ " kw-hr");
                    dictornary.Add("ETQ_" + sensor.UUID, ((EnergySensor)sensor).CumulativeValue.ToString() + " kw-hr");
                }

                if (sensor is PressureSensor)
                    dictornary.Add("PT_" + sensor.UUID, sensor.CurrentValue.ToString()+" "+sensor.UnitName);

                if (sensor is LevelSensor)
                    dictornary.Add("LT_" + sensor.UUID, sensor.CurrentValue.ToString()+" "+sensor.UnitName);

                if (sensor is BatteryVoltageDetector)
                    dictornary.Add("BV_" + sensor.UUID, sensor.CurrentValue.ToString() + " " + sensor.UnitName);

                if (sensor is ACPresenceDetector)
                    dictornary.Add("ACP_" + sensor.UUID, sensor.CurrentValue > 0 ? "On" : "Off");
                if (sensor is ChlorinePresenceDetector)
                    dictornary.Add("CPD_" + sensor.UUID, sensor.CurrentValue > 0 ? "On" : "Off");
            }

            return dictornary;
        }

        public ActionResult ShowScadaForMap(string pumpStationId)
        {
            List<Sensor> sensorList = _reportRepository.GetSensorData(Convert.ToInt32(pumpStationId));
            return PartialView("~/Views/ScadaMap/PlainScada.cshtml", sensorList.ToList());
        }

        public ActionResult DemoScada(int pumpStationId)
        {
            //List<Sensor> sensorList = _reportRepository.GetSensorData(Convert.ToInt32(3));
            var station = _reportRepository.GetPumpStationById(pumpStationId);
            ScadaMap model = _reportRepository.ScadaMap();
            model.SelectedZoneId = station.Parent.Parent.AreaId;
            model.SelectedDmaId = station.Parent.AreaId;
            model.SelectedZoneId = station.AreaId;
            return View("~/Views/ScadaMap/PlainScada.cshtml", model);
        }

        [HttpPost]
        public JsonResult PublishMessage(string state, string pumpStationId)
        {
            try
            {
                state = "\"" + state + "\"";
                string commandTime = "\"" + DateTime.Now.ToString() + "\"";
                string message = @"{""PumpStation_Id"": """+pumpStationId+@""",""Pump_Motor"": [{""Command"": " + state + @",""Command_Time"": " + commandTime + @"}]}";
                message.Replace(" ", string.Empty);
                m2mMessageViewModel model = new m2mMessageViewModel();
                model.MessgeTopic = "wasa/command/" + pumpStationId;
                model.PublishMessage = message;
                model.PublishMessageStatus = MQTTService.MQTTClientInstance(true).Publish(model.MessgeTopic, model.PublishMessage);
                return Json(new { Data = model.PublishMessageStatus, IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Data = "", IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }



        }

    }
}
