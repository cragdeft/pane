﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.QueryModel.Areas;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;

namespace AplombTech.WMS.QueryModel.Reports
{
    [NotMapped]
    public class DrillDown: IViewModel
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Drill Down";

            t.Append(title);

            return t.ToString();
        }

        public DrillDown()
        {
            Series = new List<ReportSeries>();
            SelectedSensor = new Sensor();
        }
        public IDomainObjectContainer Container { set; protected get; }  //Injected service
        public virtual IList<PumpStation> PumpStations { get; set; }
        [Required(ErrorMessage = "Pump Station is required")]
        public int SelectedPumpStationId { get; set; }
        public string[] DeriveKeys()
        {
            string[] ids = PumpStations.Select(s => s.AreaId.ToString()).ToArray();
            return ids;
        }
        [Required(ErrorMessage = "Sensor Type is required")]
        public Sensor.TransmitterType TransmeType { get; set; }

        public Sensor SelectedSensor { get; set; }
        [Required(ErrorMessage = "Report Type is required")]
        public ReportType ReportType { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public string GraphTitle { get; set; }
        public string GraphSubTitle { get; set; }
        public string[] XaxisCategory { get; set; }
        public DateTime ToDateTime { get; set; }
        public DateTime FromDateTime { get; set; }
        public List<ReportSeries> Series { get; set; }
        public void PopulateUsingKeys(string[] keys)
        {
            IList<string> ids = keys.ToList();
            PumpStations = Container.Instances<PumpStation>().Where(w => ids.Contains(w.AreaId.ToString())).ToList();
        }
        public string Unit { get; set; }
    }
}
