using Narratify.Models.Entities;
using System.Collections.Generic;

namespace Narratify.Models.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<Article> FeaturedArticles { get; set; } = new List<Article>();
    }
}