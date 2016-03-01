using System.Data.Entity;

namespace AplombTech.DWasa.Model.ModelDataContext
{
    public interface IDWasaDataContext
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        void ExecuteCommand(string command, params object[] parameters);
    }
}
