using Narratify.Models.Entities;

namespace Narratify.Models.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public required User CurrentUser { get; set; }
        public int TotalPosts { get; set; }
        public int TotalViews { get; set; }
        public int TotalComments { get; set; }
        public List<Article> UserArticles { get; set; } = new List<Article>();
    }
}