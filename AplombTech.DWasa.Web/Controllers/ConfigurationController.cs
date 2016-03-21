using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service.Interfaces;
using Repository.Pattern.UnitOfWork;

namespace AplombTech.DWasa.Web.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IUnitOfWorkAsync unitOfWorkAsync, IConfigurationService configurationService)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _configurationService = configurationService;
        }

        public ActionResult AddZone()
        {
            ZoneEntity zone = new ZoneEntity();
            return View(zone);
        }

        [HttpPost]
        public ActionResult AddZone(ZoneEntity entity)
        {
            if (ModelState.IsValid)
            {
                if (!_configurationService.IsZoneExists(entity.Name))
                {
                    _configurationService.AddZone(entity);
                    return View("Success");
                }
                else
                    ModelState.AddModelError("Name", "Zone name already exists");
            }
            return View(entity);
        }

        //public ActionResult EditZone()
        //{
        //    return View(new Zone());
        //}

        //[HttpPost]
        //public ActionResult EditZone(Zone entity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _configurationService.EditZone(entity);
        //    }
        //    return View(entity);
        //}

        public ActionResult AddDMA()
        {
            ZoneDMAEntity model = new ZoneDMAEntity { ZoneList = _configurationService.GetAllZone() };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddDMA(ZoneDMAEntity entity)
        {
            if (ModelState.IsValid)
            {
                DMAEntity model = entity.DmaEntity;
                model.Zone = new ZoneEntity() { Id = entity.ZoneId };//_configurationService.FindZone(entity.ZoneId);
                _configurationService.AddDMA(model);
                return View("Success");
            }
            entity.ZoneList = _configurationService.GetAllZone();
            return View(entity);
        }

        public ActionResult AddPumpStation()
        {
            DMAPumpEntity model = new DMAPumpEntity { ZoneList = _configurationService.GetAllZone(), DMAList = _configurationService.GetAllDMA() };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddPumpStation(DMAPumpEntity entity)
        {
            if (ModelState.IsValid)
            {
                PumpStationEntity model = entity.PumpStationEntity;
                model.DMA = new DMAEntity() { Id = entity.DMAId };//_configurationService.FindZone(entity.ZoneId);
                _configurationService.AddPumpStation(model);
                return View("Success");
            }
            entity.DMAList = _configurationService.GetAllDMA();
            return View(entity);
        }

        public ActionResult AddSensor()
        {
            PumpStationSensorEntity model = new PumpStationSensorEntity { PumpStationList = _configurationService.GetAllPumpStation() };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddSensor(PumpStationSensorEntity entity)
        {
            if (ModelState.IsValid)
            {
                _configurationService.AddSensor(entity);
                return View("Success");//Show dynamically
            }
            entity.PumpStationList = _configurationService.GetAllPumpStation();
            return View(entity);
        }

        public ActionResult AddCamera()
        {
            PumpStationCameraEntity model = new PumpStationCameraEntity { PumpStationList = _configurationService.GetAllPumpStation() };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddCamera(PumpStationCameraEntity entity)
        {
            if (ModelState.IsValid)
            {
                _configurationService.AddCamera(entity);
                return View("Success");//Show dynamically
            }
            entity.PumpStationList = _configurationService.GetAllPumpStation();
            return View(entity);
        }

        public ActionResult AddRouter()
        {
            PumpStationRouterEntity model = new PumpStationRouterEntity { PumpStationList = _configurationService.GetAllPumpStation() };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddRouter(PumpStationRouterEntity entity)
        {
            if (ModelState.IsValid)
            {
                _configurationService.AddRouter(entity);
                return View("Success");//Show dynamically
            }
            entity.PumpStationList = _configurationService.GetAllPumpStation();
            return View(entity);
        }

        public ActionResult AddPump()
        {
            PumpStationPumpEntity model = new PumpStationPumpEntity { PumpStationList = _configurationService.GetAllPumpStation() };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddPump(PumpStationPumpEntity entity)
        {
            if (ModelState.IsValid)
            {
                _configurationService.AddPump(entity);
                return View("Success");//Show dynamically
            }
            entity.PumpStationList = _configurationService.GetAllPumpStation();
            return View(entity);
        }

        //[HttpPost]
        //public ActionResult Edit(Zone entity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _configurationService.EditZone(entity);
        //    }
        //    return View(entity);
        //}

    }
}