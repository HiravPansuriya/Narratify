using Narratify.Models.Entities;
using Narratify.Repositories.Interfaces;
using Narratify.Services.Interfaces;
using System.Linq;

namespace Narratify.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Article?> GetArticleById(int id)
        {
            return await _unitOfWork.Articles.GetByIdAsync(id);
        }

        public async Task<Article?> GetArticleBySlugAsync(string slug)
        {
            return (await _unitOfWork.Articles.FindAsync(a => a.Slug == slug && a.IsPublished, a => a.Author, a => a.Comments)).FirstOrDefault();
        }

        public async Task<Article?> GetArticleBySlugForAuthorAsync(string slug, string authorId)
        {
            return (await _unitOfWork.Articles.FindAsync(a => a.Slug == slug && a.AuthorId == authorId, a => a.Author, a => a.Comments)).FirstOrDefault();
        }

        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _unitOfWork.Articles.GetAllAsync();
        }

        public async Task<IEnumerable<Article>> GetPublishedArticles()
        {
            return await _unitOfWork.Articles.GetPublishedWithIncludesAsync();
        }

        public async Task<IEnumerable<Article>> GetUserArticles(string userId)
        {
            return await _unitOfWork.Articles.FindAsync(a => a.AuthorId == userId, a => a.Author);
        }

        public async Task CreateArticle(Article article)
        {
            article.GenerateSlug();
            article.UpdateReadingTime();
            article.Summary = article.PreviewText;
            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateArticle(Article article)
        {
            _unitOfWork.Articles.Update(article);
            article.UpdateReadingTime();
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteArticle(int id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article != null)
            {
                _unitOfWork.Articles.Remove(article);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<bool> PublishArticle(int id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article != null)
            {
                article.Publish();
                await UpdateArticle(article);
                return true;
            }
            return false;
        }

        public async Task<bool> UnpublishArticle(int id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article != null)
            {
                article.Unpublish();
                await UpdateArticle(article);
                return true;
            }
            return false;
        }

        public async Task<int> GetTotalArticleCountAsync()
        {
            return await _unitOfWork.Articles.CountAsync();
        }

        public async Task RecalculateArticleStats()
        {
            var allArticles = await _unitOfWork.Articles.GetAllAsync();
            
            foreach (var article in allArticles)
            {
                // Recalculate comment count from actual comments in database
                var actualCommentCount = (await _unitOfWork.Comments.FindAsync(c => c.ArticleId == article.Id)).Count();
                article.CommentCount = actualCommentCount;
                
                _unitOfWork.Articles.Update(article);
            }
            
            await _unitOfWork.CompleteAsync();
        }
    }
}