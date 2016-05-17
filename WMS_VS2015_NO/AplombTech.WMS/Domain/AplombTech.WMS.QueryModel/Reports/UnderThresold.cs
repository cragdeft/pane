using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.QueryModel.Areas;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;

namespace AplombTech.WMS.QueryModel.Reports
{
    public class UnderThresold : IViewModel
    {
        public IDomainObjectContainer Container { set; protected get; }  //Injected service
        public virtual IList<PumpStation> PumpStations { get; set; }
        public int SelectedPumpStationId { get; set; }
        public Sensor.TransmitterType TransmeType { get; set; }
        public ReportType ReportType { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public DateTime ToDateTime { get; set; }
        public DateTime FromDateTime { get; set; }
        public List<SensorData> SensorDatas { get; set; }
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Under Thresold";

            t.Append(title);

            return t.ToString();
        }

        public string[] DeriveKeys()
        {
            string[] ids = PumpStations.Select(s => s.AreaID.ToString()).ToArray();
            return ids;
        }

        public void PopulateUsingKeys(string[] keys)
        {
            IList<string> ids = keys.ToList();
            PumpStations = Container.Instances<PumpStation>().Where(w => ids.Contains(w.AreaID.ToString())).ToList();
        }

        public string Unit { get; set; }
    }
}
