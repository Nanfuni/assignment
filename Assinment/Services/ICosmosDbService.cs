using Microsoft.Azure.Cosmos;

namespace Assignment1.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<T>> GetItemsAsync<T>(QueryDefinition queryDefinition);
        Task<T> AddItemAsync<T>(T item);
        Task<T> UpdateItemAsync<T>(string id, T item);
        Task<T> GetItemAsync<T>(string id);
        Task<T> DeleteItemAsync<T>(string id);
    }

}
