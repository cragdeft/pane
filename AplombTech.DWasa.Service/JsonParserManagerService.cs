using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Model.Models;
using AplombTech.DWasa.Service.Interfaces;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;

namespace AplombTech.DWasa.Service
{
    public class JsonParserManagerService : IJsonParserManagerService
    {


        //#region PrivateProperty
        //private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        //private readonly IRepositoryAsync<DataLog> _dataLogRepository;
        //#endregion

        //public JsonParserManagerService(IUnitOfWorkAsync unitOfWorkAsync)
        //{
        //    _unitOfWorkAsync = unitOfWorkAsync;
        //    _dataLogRepository = _unitOfWorkAsync.RepositoryAsync<DataLog>();
        //}


        //#region Data-Log AddOrUpdate  GraphRange
        //public IEnumerable<DataLog> AddOrUpdateDataLogGraphRange(IEnumerable<DataLog> model)
        //{
        //    List<DataLog> dataLogModel = new List<DataLog>();
        //    _unitOfWorkAsync.BeginTransaction();
        //    try
        //    {

        //        dataLogModel = FillDataLogInformations(model, dataLogModel);
        //        _dataLogRepository.InsertOrUpdateGraphRange(dataLogModel);
        //        var changes = _unitOfWorkAsync.SaveChanges();
        //        _unitOfWorkAsync.Commit();

        //    }
        //    catch (Exception ex)
        //    {
        //        _unitOfWorkAsync.Rollback();
        //    }

        //    return dataLogModel;
        //}



        //public List<DataLog> FillDataLogInformations(IEnumerable<DataLog> model, List<DataLog> dataLogModel)
        //{
        //    foreach (var item in model)
        //    {

        //        //check already exist or not.
        //        IEnumerable<DataLog> temp = IsDataLogExists(item.DataLogId);
        //        if (temp.Count() == 0)
        //        {
        //            item.AuditField = new AuditFields();
        //            item.ObjectState = ObjectState.Added;
        //            //new item
        //            dataLogModel.Add(item);

        //            continue;
        //        }
        //        else
        //        {
        //            //existing item               
        //            // dataLogModel = temp;
        //            foreach (var existingItem in temp.ToList())
        //            {
        //                //modify data log              
        //                existingItem.DataLogId = item.DataLogId;
        //                existingItem.Production = item.Production;
        //                existingItem.Energy = item.Energy;
        //                existingItem.Pressure = item.Pressure;
        //                existingItem.WaterLevel = item.WaterLevel;
        //                existingItem.Clorination = item.Clorination;
        //                existingItem.LogDateTime = item.LogDateTime;
        //                existingItem.AuditField = new AuditFields();
        //                existingItem.ObjectState = ObjectState.Modified;

        //                dataLogModel.Add(existingItem);
        //            }
        //        }
        //    }

        //    return dataLogModel;
        //}
        //private IEnumerable<DataLog> IsDataLogExists(int key)
        //{
        //    return _dataLogRepository.Query(e => e.DataLogId == key).Select();
        //}

        //#endregion
    }
}
