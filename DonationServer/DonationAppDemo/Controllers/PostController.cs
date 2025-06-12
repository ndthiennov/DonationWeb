using Microsoft.AspNetCore.Mvc;
using DonationAppDemo.Services.Interfaces;
using DonationAppDemo.DTOs;
using System.Threading.Tasks;

namespace DonationAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddPost([FromBody] PostDto postDto)
        {
            try
            {
                await _postService.AddPostAsync(postDto);
                return Ok("Post added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] PostDto postDto)
        {
            try
            {
                await _postService.UpdatePostAsync(id, postDto);
                return Ok("Post updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                return Ok("Post deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetPostDetail(int id)
        {
            try
            {
                var result = await _postService.GetPostDetailAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetPostsForAdmin([FromQuery] string postDate)
        {
            var result = await _postService.GetPostsForAdminAsync(postDate);
            return Ok(result);
        }

        [HttpGet("User/List")]
        public async Task<IActionResult> GetPostsForUser([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await _postService.GetPostsForUserAsync(page, pageSize);
            return Ok(result);
        }

        [HttpPost("Comment/Add")]
        public async Task<IActionResult> AddComment([FromBody] CommentDto commentDto)
        {
            try
            {
                await _postService.AddCommentAsync(commentDto);
                return Ok("Comment added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Comment/Update/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto commentDto)
        {
            try
            {
                await _postService.UpdateCommentAsync(id, commentDto);
                return Ok("Comment updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Comment/Delete/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                await _postService.DeleteCommentAsync(id);
                return Ok("Comment deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Comment/List/{postId}")]
        public async Task<IActionResult> GetCommentsByPost(int postId)
        {
            try
            {
                var result = await _postService.GetCommentsByPostAsync(postId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}