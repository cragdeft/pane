using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplombTech.DWasa.Entity;
using AplombTech.DWasa.Service.Interfaces;
using AplombTech.DWasa.Utility.Enums;
using Repository.Pattern.UnitOfWork;

namespace AplombTech.DWasa.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IConfigurationService _configurationService;

        public ReportController(IUnitOfWorkAsync unitOfWorkAsync, IConfigurationService configurationService)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _configurationService = configurationService;
        }
        // GET: Graph
        public ActionResult Show()
        {
            ReportEntity model = new ReportEntity();
            model.XaxisCategory = new string[] { "00.00", "01.00", "02.00", "03.00", "04.00", "05.00", "06.00", "07.00", "08.00", "09.00" };
            return View(model);
        }

        [HttpPost]
        public ActionResult Show(ReportEntity model)
        {
            //assign model data
            if (model.ReportType == ReportType.Hourly)
            {
                model.GraphTitle = "Hourly Data Review";
                model.GraphSubTitle = "Data for " + model.ToDateTime.Hour+"th Hour";
                model.XaxisCategory = new string[] { "00.00", "00.05", "00.10", "00.15", "00.20", "00.25", "00.30", "00.35", "00.40", "00.45","00.50","00.55","01.00" };
                model.Series = GeneratetSeriesDataHourly();
            }
            else if (model.ReportType == ReportType.Daily)
            {
                model.GraphTitle = "Daily Data Review";
                model.GraphSubTitle = "Data for " + model.ToDateTime.DayOfWeek ;
                model.XaxisCategory = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
                model.Series = GeneratetSeriesDataDaily();
            }

            else if (model.ReportType == ReportType.Monthly)
            {
                model.GraphTitle = "Monthly Data Review";
                model.GraphSubTitle = "Data for month of " + model.ToDateTime.ToString("MMM");
                model.XaxisCategory = new string[] {  "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23","24","25","26","27","28","29","30" };
                model.Series = GeneratetSeriesDataMonthly();
            }

            else if (model.ReportType == ReportType.Realtime)
            {
                model.GraphTitle = "Real time Data Review";
                model.GraphSubTitle = "Data for Real time";
                model.XaxisCategory = new string[] { "10.50"};
                model.Series = GeneratetSeriesDataRealTime();
            }

            
            return View(model);
        }

        private string GeneratetSeriesDataHourly()//list of device value will passed
        {
            string series = @"[{
                    name: 'FT',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }, {
                    name: 'FT2',
                    data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
                }]";


            return series;
        }

        private string GeneratetSeriesDataDaily()//list of device value will passed
        {
            string series = @"[{
                    name: 'FT',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6,7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }, {
                    name: 'FT2',
                    data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5,-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
                }]";


            return series;
        }

        private string GeneratetSeriesDataMonthly()
        {
            string series = @"[{
                    name: 'FT',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6,7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6,25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }, {
                    name: 'FT2',
                    data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5,-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5,25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
                }]";


            return series;
        }

        private string GeneratetSeriesDataRealTime()
        {
            string series = @"[{
                    name: 'FT',
                    data: [7.0]
                }, {
                    name: 'FT2',
                    data: [-0.2]
                }]";


            return series;
        }
    }
}