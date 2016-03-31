using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.QueryModel.Repositories;
using NakedObjects;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplombTech.WMS.Site.Controllers
{
    public class ZoneMapController : SystemControllerImpl
    {
        #region Injected Services
        public ReportRepository _reportRepository { set; protected get; }
        #endregion

        public ZoneMapController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) { }

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        public ZoneMapController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
            : base(facade, idHelper)
        {
            nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        }

        // GET: ZoneMap
        public ActionResult Index()
        {
            ZoneMap zones = _reportRepository.GoogleMap();
            int totalZone = zones.Zones.Count();
            return View();
        }

        // GET: ZoneMap/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ZoneMap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZoneMap/Create
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

        // GET: ZoneMap/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZoneMap/Edit/5
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

        // GET: ZoneMap/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ZoneMap/Delete/5
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
    }
}
