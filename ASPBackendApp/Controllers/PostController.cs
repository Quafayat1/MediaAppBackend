using Microsoft.AspNetCore.Mvc;
using ASPBackendApp.Contracts;

namespace ASPBackendApp.Controllers
{

    //waqas-media-web-app-gbekc0a6fpeggkam.uksouth-01.azurewebsites.net/api/posts

    [ApiController]
    [Route("api/")]
    public class PostController : ControllerBase
    {
        private readonly IDBService _dbService;

        public PostController(IDBService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost("posts")]
        public async Task<IActionResult> AddPostAsync([FromBody] PostDto post)
        {
            var response = await _dbService.AddPostAsync(post);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("posts")]
        public async Task<IActionResult> GetPostsAsync()
        {
            var posts = await _dbService.GetPostsAsync("SELECT * FROM c");
            return Ok(posts);
        }

        [HttpDelete("post/{id}")]
        public async Task<IActionResult> DeletePostAsync(string id)
        {
            var response = await _dbService.DeletePostAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
