using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplombTech.DWasa.MQTT.Client;
using AplombTech.DWasa.Web.Models;
using AplombTech.DWasa.Web.MqTTAdapter;

namespace AplombTech.DWasa.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult PublishMessage(M2MMessageViewModel model)
        {
            ViewBag.Message = "Your contact page.";

            model.PublishMessageStatus = MqttClientWrapperAdapter.WrapperInstance.Publish(model.MessgeTopic, model.PublishMessage);

            return View("Index", model);

        }


        [HttpPost]
        public ActionResult SubscribeMessage(M2MMessageViewModel model)
        {
            ViewBag.Message = "Your contact page.";
            model.PublishMessageStatus = MqttClientWrapperAdapter.WrapperInstance.Subscribe(model.MessgeTopic);
            return View("Index", model);
        }
        
    }
}