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
                _configurationService.AddZone(entity);
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

        public ActionResult CreateDMA()
        {
            ZoneDMAEntity model = new ZoneDMAEntity();
            model.ZoneList = _configurationService.GetAllZone();
            return View(new ZoneDMAEntity());
        }

        [HttpPost]
        public ActionResult CreateDMA(ZoneDMAEntity entity)
        {
            if (ModelState.IsValid)
            {
                //_configurationService.AddZone(entity);
            }
            return View(entity);
        }

        //public ActionResult CreateZone()
        //{
        //    return View(new Zone());
        //}

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