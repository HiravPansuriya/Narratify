using Narratify.Models.Entities;

namespace Narratify.Services.Interfaces;

public interface ICommentService
{
    Task<Comment?> GetCommentById(int id);
    Task<IEnumerable<Comment>> GetCommentsByArticleId(int articleId);
    Task AddComment(Comment comment);
    Task UpdateComment(Comment comment);
    Task DeleteComment(int id);
}