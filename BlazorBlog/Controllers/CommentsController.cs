using BlazorBlog.Client.Models;
using BlazorBlog.Client.Services.Interfaces;
using BlazorBlog.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CommentsController : ControllerBase
    {
        private string _userId => User.GetUserId()!;

        private readonly ICommentDTOService _commentService;

        public CommentsController(ICommentDTOService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{blogId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsByBlogPostId([FromRoute] int blogId)
        {
            try
            {

                if (blogId is not 0)
                {
                    IEnumerable<CommentDTO> comments = await _commentService.GetCommentsByBlogPostIdAsync(blogId);

                    return Ok(comments);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpGet("selected/{commentId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CommentDTO>> GetCommentById([FromRoute] int commentId)
        {

            try
            {

                CommentDTO? comment = await _commentService.GetCommentByIdAsync(commentId);

                if (comment is not null)
                {
                    return Ok(comment);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPost]
        public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CommentDTO comment)
        {

            try
            {

                comment.AuthorId = _userId;

                CommentDTO createdComment = await _commentService.CreateCommentAsync(comment);

                return Ok(createdComment);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPut("{commentId:int}")]
        public async Task<ActionResult<CommentDTO>> UpdateComment([FromRoute] int commentId, [FromBody] CommentDTO commentDTO)
        {

            try
            {

                bool inAuthorRole = User.IsInRole("Author");
                bool inModeratorRole = User.IsInRole("Moderator");

                CommentDTO? commentToUpdate = await _commentService.GetCommentByIdAsync(commentId);

                if (commentToUpdate is not null)
                {
                    if (inAuthorRole || inModeratorRole || commentToUpdate.AuthorId == _userId)
                    {
                        await _commentService.UpdateCommentAsync(commentDTO);

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Problem();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpDelete("{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {

            try
            {

                bool inAuthorRole = User.IsInRole("Author");
                bool inModeratorRole = User.IsInRole("Moderator");

                CommentDTO? comment = await _commentService.GetCommentByIdAsync(commentId);

                if (comment is not null)
                {
                    if (inAuthorRole || inModeratorRole || comment.AuthorId == _userId)
                    {
                        await _commentService.DeleteCommentByIdAsync(commentId);

                        return NoContent();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}
