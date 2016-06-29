using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.QueryModel.Repositories;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;
using Newtonsoft.Json;

namespace AplombTech.WMS.Site.Controllers
{
    [Authorize]
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
        public ActionResult ShowDrillDownFromMap()
        {
            DrillDown model = _reportRepository.DrillDown();
            return View("~/Views/DrillDown/DrillDown.cshtml", model);
        }

        public JsonResult GetReportModel(DrillDown model)
        {
            model = _reportRepository.GetReportData(model);

            return Json(new { Data = model, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ExportToExcel(DrillDown m)
        {
            m = _reportRepository.GetReportData(m);
            Summary model = _reportRepository.Summary();
            
            try
            {
                var grid = new System.Web.UI.WebControls.GridView();
                List<ExcelDrillDownReport> report = new List<ExcelDrillDownReport>();
                foreach (var series in m.Series)
                {
                    for (int index = 0; index < series.data.Count; index++)
                    {
                        var data = series.data[index];
                        report.Add(new ExcelDrillDownReport()
                        {
                            Sensor = series.name,
                            Unit = m.Unit,
                            Value = data.ToString(),
                            Date = m.XaxisCategory[index]
                        });
                    }
                }
                grid.DataSource = report;

                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename="+m.ReportType+"_" + DateTime.Now.ToShortDateString() + ".xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();

                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);

            }
            catch
            {

            }

            return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSensorDropdownData(int pumpstationId)
        {
            List<AplombTech.WMS.QueryModel.Sensors.Sensor> sensorList = _reportRepository.GetSensorData(pumpstationId);
            var sendingSensorList = new List<dynamic>();
            foreach (var sensor in sensorList)
            {
                dynamic sendingSensor = new ExpandoObject();
                sendingSensor.SensorID = sensor.SensorID;
                sendingSensor.Name = sensor.Name;
                sendingSensor.Model = sensor.Model;
                sendingSensor.Version = sensor.Version;
                sendingSensor.DisplayName = GetName(sensor);
                sendingSensorList.Add(sendingSensor);

            }

            return Json(new { Data = JsonConvert.SerializeObject(sendingSensorList),OptionGroup=new List<string>() { "AC Presence Detector", "Battery Voltage Detector", "Chlorine Presence Detector", "Energy Transmitter", "Flow Transmitter", "Level Transmitter", "Pressure Transmitter" }, IsSuccess = true }, JsonRequestBehavior.AllowGet);
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
    }
}