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
            return await _unitOfWork.Comments.FindAsync(c => c.ArticleId == articleId);
        }

        public async Task AddComment(Comment comment)
        {
            await _unitOfWork.Comments.AddAsync(comment);
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
                _unitOfWork.Comments.Remove(comment);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}