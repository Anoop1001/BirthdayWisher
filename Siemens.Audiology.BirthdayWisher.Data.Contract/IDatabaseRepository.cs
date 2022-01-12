using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siemens.Audiology.BirthdayWisher.Data.Contract
{
    public interface IDatabaseRepository
    {
        Task CreateGenericTable<T>();
        Task<List<T>> GetAllData<T>() where T : new();
        Task ClearData<T>();
        Task InsertData<T>(T data);
        Task DeleteOneAsync<T>(object id);
        Task InsertDataList<T>(List<T> data);
        Task UpdateData<T>(T data);
    }
}
