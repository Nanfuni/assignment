using Assignment1.Services;
using Microsoft.Azure.Cosmos;

namespace Assignment1
{
    public class CosmosDbServiceInitializer
    {
        public static async Task<ICosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection.GetValue<string>("DatabaseName") ?? throw new ArgumentNullException("DatabaseName configuration is missing");
            string containerName = configurationSection.GetValue<string>("ContainerName") ?? throw new ArgumentNullException("ContainerName configuration is missing");
            string account = configurationSection.GetValue<string>("Account") ?? throw new ArgumentNullException("Account configuration is missing");
            string key = configurationSection.GetValue<string>("Key") ?? throw new ArgumentNullException("Key configuration is missing");

            CosmosClient client = new CosmosClient(account, key);
            CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }
    }
}
