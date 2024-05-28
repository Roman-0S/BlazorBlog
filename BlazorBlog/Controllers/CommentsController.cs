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


        [HttpGet("selected/{commentId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CommentDTO>> GetCommentById([FromRoute] int commentId)
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


        [HttpPost]
        public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CommentDTO comment)
        {
            comment.AuthorId = _userId;

            CommentDTO createdComment = await _commentService.CreateCommentAsync(comment);

            return Ok(createdComment);
        }


        [HttpPut("{commentId:int}")]
        public async Task<ActionResult<CommentDTO>> UpdateComment([FromRoute] int commentId, [FromBody] CommentDTO commentDTO)
        {
            bool inAuthorRole = User.IsInRole("Author");
            bool inModeratorRole = User.IsInRole("Moderator");

            CommentDTO? commentToUpdate = await _commentService.GetCommentByIdAsync(commentId);

            if (commentToUpdate is not null)
            {
                if (inAuthorRole || inModeratorRole || commentToUpdate?.AuthorId == _userId)
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


        [HttpDelete("{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            bool inAuthorRole = User.IsInRole("Author");
            bool inModeratorRole = User.IsInRole("Moderator");

            CommentDTO? comment = await _commentService.GetCommentByIdAsync(commentId);

            if (comment is not null)
            {
                if (inAuthorRole || inModeratorRole || comment?.AuthorId == _userId)
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

    }
}
