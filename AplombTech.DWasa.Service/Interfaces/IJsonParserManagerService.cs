using System.Collections.Generic;
using AplombTech.DWasa.Model.Models;

namespace AplombTech.DWasa.Service.Interfaces
{
    public interface IJsonParserManagerService
    {
        IEnumerable<DataLog> AddOrUpdateDataLogGraphRange(IEnumerable<DataLog> model);
    }
}
