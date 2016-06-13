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
using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using AplombTech.WMS.Site.Models;
using AplombTech.WMS.Site.MQTT;
using Newtonsoft.Json;

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
        public JsonResult PublishMessage(string state)
        {
            try
            {
                state = "\"" + state+ "\"";
                string commandTime = "\"" + DateTime.Now.ToString()+ "\"";
                string message = @"{
                                  ""PumpStation_Id"": ""1"",
                                  ""Pump_Motor"": [
                                    {
                                      ""Command"": "+state+ @",
                                      ""Command_Time"": "+commandTime+ @" 
                                    }
                                  ]
                            }" ;
                m2mMessageViewModel model = new m2mMessageViewModel();
                model.MessgeTopic = "wasa / command / PumpStation_Id";
                model.PublishMessage = state;
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
