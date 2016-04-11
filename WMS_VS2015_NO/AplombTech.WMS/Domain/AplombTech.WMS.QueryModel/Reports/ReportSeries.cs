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
        }
        public string name { get; set; }
        public List<double> data { get; set; }
    }
}
