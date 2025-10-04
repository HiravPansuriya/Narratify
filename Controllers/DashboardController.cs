using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Narratify.Models.Entities;
using Narratify.Models.ViewModels.Dashboard;
using Narratify.Services.Interfaces;

namespace Narratify.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;

        public DashboardController(
            UserManager<User> userManager,
            IArticleService articleService,
            ICommentService commentService)
        {
            _userManager = userManager;
            _articleService = articleService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Recalculate article stats to fix existing data that shows 0
            await _articleService.RecalculateArticleStats();

            var userArticles = (await _articleService.GetUserArticles(currentUser.Id)).ToList();
            var totalPosts = userArticles.Count;
            var totalViews = userArticles.Sum(a => a.ViewCount);
            var totalComments = userArticles.Sum(a => a.CommentCount);

            var viewModel = new DashboardViewModel
            {
                CurrentUser = currentUser,
                TotalPosts = totalPosts,
                TotalViews = totalViews,
                TotalComments = totalComments,
                UserArticles = userArticles
            };

            return View(viewModel);
        }
    }
}
