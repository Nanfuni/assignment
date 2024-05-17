using Assignment1.Services;
using Microsoft.Azure.Cosmos;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(
        CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        _container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task<IEnumerable<T>> GetItemsAsync<T>(QueryDefinition queryDefinition)
    {
        try
        {
            var queryIterator = _container.GetItemQueryIterator<T>(queryDefinition);
            var results = new List<T>();
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task<T> AddItemAsync<T>(T item)
    {
        try
        {
            ItemResponse<T> response = await _container.CreateItemAsync(item);
            return response;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task<T> UpdateItemAsync<T>(string id, T item)
    {
        try
        {
            ItemResponse<T> response = await _container.ReplaceItemAsync(item, id);
            return response;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }

    }

    public async Task<T> GetItemAsync<T>(string id)
    {
        try
        {
            ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
            return response;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task<T> DeleteItemAsync<T>(string id)
    {
        try
        {
            ItemResponse<T> response = await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
            return response;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

}
