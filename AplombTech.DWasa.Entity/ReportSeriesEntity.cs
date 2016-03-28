using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class ReportSeriesEntity
    {
        public ReportSeriesEntity()
        {
            data = new List<double>();
        }
        public string name { get; set; }
        public List<double> data { get; set; }
    }
}
