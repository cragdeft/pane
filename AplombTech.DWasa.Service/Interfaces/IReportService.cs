using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity;

namespace AplombTech.DWasa.Service.Interfaces
{
    public interface IReportService
    {
        void GeneratetSeriesDataDaily(ReportEntity model);
    }
}
