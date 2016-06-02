using AplombTech.WMS.QueryModel.Features;
using AplombTech.WMS.QueryModel.UserAccounts;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Repositories
{
    public class LoggedInUserInfoRepository : AbstractFactoryAndRepository
    {
        IList<Feature>  _features = new List<Feature>();

        public IList<Feature> GetFeatureListByLoginUser()
        {
            if (_features.Count > 0)
                return _features;

            LoginUser user = (from f in Container.Instances<LoginUser>()
                              where f.Email == Container.Principal.Identity.Name
                              select f).FirstOrDefault();

            if (user != null)
                _features = user.Role.Features;

            return _features;
        }
    }
}
