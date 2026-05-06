using ASPBackendApp.Models;

namespace ASPBackendApp.Contracts
{
    public interface IDBService
    {
        Task<ApiResponse> AddPostAsync(PostDto post);
        Task<ApiResponse> GetPostsAsync(string query);
        Task<ApiResponse> DeletePostAsync(string id);
    }
}
