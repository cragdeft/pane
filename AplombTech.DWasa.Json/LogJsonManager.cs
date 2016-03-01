using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Model.ModelDataContext;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service;
using AplombTech.DWasa.Service.Interfaces;
using Newtonsoft.Json;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.UnitOfWork;

namespace AplombTech.DWasa.Json
{
    public class LogJsonManager
    {
        private IUnitOfWorkAsync _unitOfWorkAsync;
        private IJsonParserManagerService _jsonParserManagerService;
        public DataLog DataLog { get; }

        private T JsonDesrialized<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public LogJsonManager(string jsonString)
        {
            DataLog = JsonDesrialized<DataLog>(jsonString);
            InitializeParameters();
        }

        public LogJsonManager(string jsonString, IJsonParserManagerService jsonParserManagerService)
        {
            _jsonParserManagerService = jsonParserManagerService;
            DataLog = JsonDesrialized<DataLog>(jsonString);
        }

        private void InitializeParameters()
        {
            IDataContextAsync context = new DWasaDataContext();
            _unitOfWorkAsync = new UnitOfWork(context);
            _jsonParserManagerService = new JsonParserManagerService(_unitOfWorkAsync);
        }

        public void Parse()
        {
            IEnumerable<DataLog> list = new List<DataLog>() { DataLog };
            _jsonParserManagerService.AddOrUpdateDataLogGraphRange(list);
        }
    }
}
