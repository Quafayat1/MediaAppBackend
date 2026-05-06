using Microsoft.Azure.Cosmos;
using ASPBackendApp.Contracts;
using ASPBackendApp.Models;

namespace ASPBackendApp.Services
{
    /// <summary>
    /// Service for interacting with Cosmos DB "Posts" container.
    /// Handles create, read (query), and delete operations.
    /// </summary>
    public class DbService : IDBService
    {
        private readonly Container _container;

        /// <summary>
        /// Initializes the Cosmos DB container instance.
        /// </summary>
        /// <param name="dbClient">The CosmosClient injected via DI.</param>
        /// <param name="databaseName">The database name.</param>
        /// <param name="containerName">The container name.</param>
        public DbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            // Get reference to the container in Cosmos DB
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        /// <summary>
        /// Adds a new post to the Cosmos DB container.
        /// </summary>
        /// <param name="post">DTO containing post data from client.</param>
        /// <returns>ApiResponse with status and created entity.</returns>
        public async Task<ApiResponse> AddPostAsync(PostDto post)
        {
            // Create a Post entity mapped to Cosmos schema
            var postEntity = new Post
            {
                // Id is required in Cosmos DB and must match the partition key "/id"
                Id = Guid.NewGuid().ToString(),
                Title = post.Title,
                Content = post.Content,
                Author = post.Author
            };

            // Insert item into Cosmos DB.
            // PartitionKey must match the container partition key ("/id")
            var response = await _container.CreateItemAsync(postEntity, new PartitionKey(postEntity.Id));

            return new ApiResponse
            {
                // Cosmos DB returns 201 Created on successful insert
                IsSuccess = response.StatusCode == System.Net.HttpStatusCode.Created,
                Message = response.StatusCode.ToString(),
                Result = postEntity
            };
        }

        /// <summary>
        /// Executes a query against the Cosmos DB container to retrieve posts.
        /// </summary>
        /// <param name="query">SQL-like query string for Cosmos DB (e.g. "SELECT * FROM c").</param>
        /// <returns>ApiResponse with list of posts.</returns>
        public async Task<ApiResponse> GetPostsAsync(string query)
        {
            // Build the query iterator for paged results
            var queryIterator = _container.GetItemQueryIterator<Post>(new QueryDefinition(query));
            var posts = new List<Post>();

            // Read pages until no more results
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                posts.AddRange(response.ToList());
            }

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Posts fetched successfully!",
                Result = posts
            };
        }

        /// <summary>
        /// Deletes a post from Cosmos DB by id.
        /// </summary>
        /// <param name="id">The id of the post (also the partition key value).</param>
        /// <returns>ApiResponse with status of delete operation.</returns>
        public async Task<ApiResponse> DeletePostAsync(string id)
        {
            try
            {
                // Delete the item by id and partition key.
                var response = await _container.DeleteItemAsync<Post>(id, new PartitionKey(id));

                return new ApiResponse
                {
                    // Cosmos DB returns 204 NoContent when delete is successful
                    IsSuccess = response.StatusCode == System.Net.HttpStatusCode.NoContent,
                    Message = response.StatusCode.ToString(),
                    Result = null
                };
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Handle case when item is not found
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Post with id '{id}' not found. {ex.Message}",
                    Result = null
                };
            }
        }
    }
}
