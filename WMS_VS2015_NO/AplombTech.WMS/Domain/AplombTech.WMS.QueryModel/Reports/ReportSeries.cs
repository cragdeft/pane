using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Reports
{
    public class ReportSeries
    {
        public ReportSeries()
        {
            data = new List<double>();
            threshold = 25;
            color = "red";
            negativeColor = "green";
        }
        public string name { get; set; }
        public List<double> data { get; set; }
        public int threshold { get; set; }
        public string negativeColor { get; set; }
        public string color { get; set; }
    }
}
