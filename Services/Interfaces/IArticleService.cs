using Narratify.Models.Entities;

namespace Narratify.Services.Interfaces;

public interface IArticleService
{
    Task<Article?> GetArticleById(int id);
    Task<IEnumerable<Article>> GetAllArticles();
    Task<IEnumerable<Article>> GetPublishedArticles();
    Task<IEnumerable<Article>> GetUserArticles(string userId);
    Task CreateArticle(Article article);
    Task UpdateArticle(Article article);
    Task DeleteArticle(int id);
    Task<bool> PublishArticle(int id);
    Task<bool> UnpublishArticle(int id);
}