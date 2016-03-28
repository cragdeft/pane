using System.Collections.Generic;
using AplombTech.DWasa.Entity.JsonCommandEntity;
using AplombTech.DWasa.Model.Models;

namespace AplombTech.DWasa.Service.Interfaces
{
    public interface ICommandJsonService
    {
        //IEnumerable<DataLog> AddOrUpdateDataLogGraphRange(IEnumerable<DataLog> model);
        void Add(CommandJsonEntity entity);
    }
}
