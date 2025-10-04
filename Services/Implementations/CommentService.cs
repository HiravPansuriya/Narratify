using Narratify.Models.Entities;
using Narratify.Repositories.Interfaces;
using Narratify.Services.Interfaces;

namespace Narratify.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Comment?> GetCommentById(int id)
        {
            return await _unitOfWork.Comments.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByArticleId(int articleId)
        {
            var comments = await _unitOfWork.Comments.FindAsync(c => c.ArticleId == articleId, c => c.User!);
            return comments ?? Enumerable.Empty<Comment>();
        }

        public async Task AddComment(Comment comment)
        {
            await _unitOfWork.Comments.AddAsync(comment);
            
            // Update the article's comment count
            var article = await _unitOfWork.Articles.GetByIdAsync(comment.ArticleId);
            if (article != null)
            {
                article.CommentCount++;
                _unitOfWork.Articles.Update(article);
            }
            
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateComment(Comment comment)
        {
            _unitOfWork.Comments.Update(comment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteComment(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (comment != null)
            {
                // Update the article's comment count
                var article = await _unitOfWork.Articles.GetByIdAsync(comment.ArticleId);
                if (article != null && article.CommentCount > 0)
                {
                    article.CommentCount--;
                    _unitOfWork.Articles.Update(article);
                }
                
                _unitOfWork.Comments.Remove(comment);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<int> GetCommentsCountForLastWeekAsync()
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            return await _unitOfWork.Comments.CountAsync(c => c.CreatedAt >= oneWeekAgo);
        }
    }
}