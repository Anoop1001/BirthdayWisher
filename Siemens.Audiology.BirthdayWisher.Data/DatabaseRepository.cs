using Siemens.Audiology.BirthdayWisher.Data.Contract;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siemens.Audiology.BirthdayWisher.Data
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly SQLiteConnection _connection;
        public DatabaseRepository(SQLiteConnection connection)
        {
            _connection = connection;
            CreateGenericTable<BirthdayInformation>().GetAwaiter().GetResult();
        }

        public async Task CreateGenericTable<T>()
        {
            await Task.FromResult(_connection.CreateTable<T>(CreateFlags.None));
        }

        public async Task<List<T>> GetAllData<T>() where T : new()
        {
            return await Task.FromResult(_connection.Table<T>().ToList());
        }

        public async Task<List<T>> GetQueryableData<T>(string query) where T : new()
        {
            return await Task.FromResult(_connection.Query<T>(query, null));
        }

        public async Task ClearData<T>()
        {
            await Task.FromResult(_connection.DeleteAll<T>());
        }

        public async Task DeleteOneAsync<T>(object id)
        {
            await Task.FromResult(_connection.Delete<T>(id));
        }

        public async Task DeleteDataAsync<T>(T data)
        {
            await Task.FromResult(_connection.Delete(data));
        }

        public async Task InsertData<T>(T data)
        {
            await Task.FromResult(_connection.Insert(data));
        }

        public async Task InsertDataList<T>(List<T> data)
        {
            await Task.FromResult(_connection.InsertAll(data));
        }

        public async Task UpdateData<T>(T data)
        {
            await Task.FromResult(_connection.Update(data));
        }
    }
}
