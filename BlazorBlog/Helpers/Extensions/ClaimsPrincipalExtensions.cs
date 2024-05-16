using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace BlazorBlog.Helpers.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
