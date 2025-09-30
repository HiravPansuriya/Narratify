using Narratify.Models.Entities;

namespace Narratify.Models.ViewModels.Articles
{
    public class BrowseArticlesViewModel
    {
        public List<Article> Articles { get; set; } = new List<Article>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalArticles { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
