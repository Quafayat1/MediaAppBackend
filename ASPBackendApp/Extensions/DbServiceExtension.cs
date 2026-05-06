using Microsoft.Azure.Cosmos;
using ASPBackendApp.Services;

namespace ASPBackendApp.Extensions
{
    public static class DbServiceExtension
    {
        public static Task<DbService> InitializeCosmosClientInstanceAsync(this IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];

            var client = new CosmosClient(account, key);

            // No CreateIfNotExists calls — just connect to existing
            return Task.FromResult(new DbService(client, databaseName, containerName));
        }
    }




}

