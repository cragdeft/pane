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
    public class MapController : Controller
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IConfigurationService _configurationService;

        public MapController(IUnitOfWorkAsync unitOfWorkAsync, IConfigurationService configurationService)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _configurationService = configurationService;
        }
        // GET: Map
        public ActionResult Index()
        {
            List<ZoneEntity> model = new List<ZoneEntity>();
            model = _configurationService.GetAll();

            return View(model);
        }

        public ActionResult LeafLetMap()
        {
            return View();
        }
    }
}