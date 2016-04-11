using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.QueryModel.Repositories;
using NakedObjects;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;

namespace AplombTech.WMS.Site.Controllers
{
    public class DrillDownController : SystemControllerImpl
    {
        #region Injected Services
        public ReportRepository _reportRepository { set; protected get; }
        #endregion

        public DrillDownController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) { }

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        public DrillDownController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
            : base(facade, idHelper)
        {
            nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        }

        // GET: Report
        public ActionResult Index()
        {
            DrillDown model = _reportRepository.DrillDown();
            return View("~/Views/DrillDown/ObjectView.cshtml", model);
        }

        public JsonResult GetReportModel(DrillDown model)
        {
            model = _reportRepository.GetReportData(model);

            return Json(new { Data = model, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
    }
}