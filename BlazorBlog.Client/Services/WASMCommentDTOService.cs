using BlazorBlog.Client.Models;
using BlazorBlog.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace BlazorBlog.Client.Services
{
    public class WASMCommentDTOService : ICommentDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMCommentDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CommentDTO> CreateCommentAsync(CommentDTO commentDTO)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<CommentDTO>("api/comments", commentDTO);
            response.EnsureSuccessStatusCode();

            CommentDTO? createdComment = await response.Content.ReadFromJsonAsync<CommentDTO>();
            return createdComment!;
        }

        public async Task DeleteCommentByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/comments/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<CommentDTO?> GetCommentByIdAsync(int commentId)
        {
            CommentDTO? comment = await _httpClient.GetFromJsonAsync<CommentDTO>($"api/comments/selected/{commentId}");

            return comment;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId)
        {
            IEnumerable<CommentDTO> comments = await _httpClient.GetFromJsonAsync<IEnumerable<CommentDTO>>($"api/comments/{blogPostId}") ?? [];

            return comments;
        }

        public async Task UpdateCommentAsync(CommentDTO comment)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<CommentDTO>($"api/comments/{comment.Id}", comment);

            response.EnsureSuccessStatusCode();
        }
    }
}
