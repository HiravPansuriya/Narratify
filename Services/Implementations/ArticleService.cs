using Narratify.Models.Entities;
using Narratify.Repositories.Interfaces;
using Narratify.Services.Interfaces;

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

        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _unitOfWork.Articles.GetAllAsync();
        }

        public async Task<IEnumerable<Article>> GetPublishedArticles()
        {
            return await _unitOfWork.Articles.FindAsync(a => a.IsPublished);
        }

        public async Task<IEnumerable<Article>> GetUserArticles(string userId)
        {
            return await _unitOfWork.Articles.FindAsync(a => a.AuthorId == userId);
        }

        public async Task CreateArticle(Article article)
        {
            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateArticle(Article article)
        {
            _unitOfWork.Articles.Update(article);
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
                article.IsPublished = true;
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
                article.IsPublished = false;
                await UpdateArticle(article);
                return true;
            }
            return false;
        }
    }
}