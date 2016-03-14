using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplombTech.DWasa.Model.Models;

namespace AplombTech.DWasa.Web.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LeafLetMap()
        {
            return View();
        }
    }
}