using Microsoft.AspNetCore.Mvc;
using Narratify.Services.Interfaces;
using Narratify.Models.ViewModels;
using Narratify.Models.ViewModels.Articles;
using Narratify.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Markdig;

namespace Narratify.Controllers
{
    [Route("Articles")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public ArticlesController(
            IArticleService articleService,
            ICommentService commentService,
            UserManager<User> userManager)
        {
            _articleService = articleService;
            _commentService = commentService;
            _userManager = userManager;
        }

        [HttpGet("")]
        [HttpGet("Browse")]
        public async Task<IActionResult> Browse(string? search, string? sort, int page = 1, int pageSize = 12)
        {
            var allArticles = await _articleService.GetPublishedArticles();

            // 1️⃣ Filter by search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                allArticles = allArticles.Where(a =>
                    (a.Title != null && a.Title.ToLower().Contains(lowerSearch)) ||
                    (a.Summary != null && a.Summary.ToLower().Contains(lowerSearch)) ||
                    (a.Author != null && a.Author.DisplayName.ToLower().Contains(lowerSearch))
                ).ToList();
            }

            // 2️⃣ Sort
            allArticles = sort switch
            {
                "Popular" => allArticles.OrderByDescending(a => a.ViewCount).ToList(),
                "Most Commented" => allArticles.OrderByDescending(a => a.Comments?.Count ?? 0).ToList(),
                "Oldest" => allArticles.OrderBy(a => a.PublishedAt).ToList(),
                _ => allArticles.OrderByDescending(a => a.PublishedAt).ToList(), // Latest default
            };

            // 3️⃣ Pagination
            var totalArticles = allArticles.Count();
            var totalPages = (int)Math.Ceiling((double)totalArticles / pageSize);
            var pagedArticles = allArticles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new BrowseArticlesViewModel
            {
                Articles = pagedArticles,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalArticles = totalArticles,
            };

            return View(viewModel);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            ViewBag.ProfilePictureUrl = currentUser.GetProfilePictureOrDefault();
            
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateArticleViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Title = model.Title,
                    Content = model.Content,
                    HtmlContent = Markdown.ToHtml(model.Content),
                    AuthorId = currentUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = model.IsPublished ? ArticleStatus.Published : ArticleStatus.Draft,
                    IsPublished = model.IsPublished,
                    PublishedAt = model.IsPublished ? DateTime.UtcNow : null
                };
                
                article.HtmlContent = Markdown.ToHtml(article.Content);

                await _articleService.CreateArticle(article);
                
                if (model.IsPublished)
                {
                    // For published articles, redirect to the article page
                    return Redirect($"/Articles/{article.Slug}");
                }
                else
                {
                    // For drafts, redirect to dashboard with success message
                    TempData["SuccessMessage"] = "Article saved as draft successfully!";
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            
            ViewBag.ProfilePictureUrl = currentUser.GetProfilePictureOrDefault();
            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var article = await _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            // Check if user owns the article
            if (article.AuthorId != currentUser.Id)
            {
                return Forbid();
            }

            var viewModel = new EditArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Summary = article.PreviewText,
                IsPublished = article.IsPublished,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                PublishedAt = article.PublishedAt,
                AuthorName = article.AuthorName,
                HtmlContent = article.HtmlContent
            };

            ViewBag.ProfilePictureUrl = currentUser.GetProfilePictureOrDefault();
            return View(viewModel);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, EditArticleViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            var article = await _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            // Check if user owns the article
            if (article.AuthorId != currentUser.Id)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                // Update article properties
                article.Title = model.Title;
                article.Content = model.Content;
                article.Summary = model.Summary;
                article.UpdatedAt = DateTime.UtcNow;

                // Handle publish/draft status change
                var wasPublished = article.IsPublished;
                article.IsPublished = model.IsPublished;
                
                // IMPORTANT: Keep Status and IsPublished in sync
                if (model.IsPublished && !wasPublished)
                {
                    article.Publish();
                    TempData["SuccessMessage"] = "Article published successfully!";
                }
                else if (!model.IsPublished && wasPublished)
                {
                    article.Unpublish();
                    TempData["SuccessMessage"] = "Article saved as draft!";
                }
                else
                {
                    article.Status = model.IsPublished ? ArticleStatus.Published : ArticleStatus.Draft;
                    TempData["SuccessMessage"] = "Article updated successfully!";
                }

                article.HtmlContent = Markdown.ToHtml(article.Content);

                await _articleService.UpdateArticle(article);

                // Redirect based on current status
                if (article.IsPublished)
                {
                    return Redirect($"/Articles/{article.Slug}");
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ViewBag.ProfilePictureUrl = currentUser.GetProfilePictureOrDefault();
            return View(model);
        }

        // Delete article - must come before {slug} route
        [HttpPost("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Check if user is authenticated
            if (!User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var article = await _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            // Check if the current user is the author of the article
            if (article.AuthorId != currentUser.Id)
            {
                return Forbid(); // Returns 403 Forbidden
            }

            await _articleService.DeleteArticle(id);
            return RedirectToAction("Index", "Dashboard");
        }

        // Post comment - must come before {slug} route
        [HttpPost("PostComment")]
        public async Task<IActionResult> PostComment(PostCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }

                var comment = new Comment
                {
                    Content = model.NewCommentContent,
                    ArticleId = model.ArticleId,
                    UserId = currentUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _commentService.AddComment(comment);
                return Redirect($"/Articles/{model.ArticleSlug}");
            }
            
            return Redirect($"/Articles/{model.ArticleSlug}");
        }

        // Individual article by slug
        [HttpGet("{slug}")]
        public async Task<IActionResult> Blog(string slug)
        {
            Article? article = null;
            
            // First try to get published article (for public viewing)
            article = await _articleService.GetArticleBySlugAsync(slug);
            
            // If not found and user is authenticated, check if it's the author's draft
            if (article == null && User.Identity?.IsAuthenticated == true)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    article = await _articleService.GetArticleBySlugForAuthorAsync(slug, currentUser.Id);
                }
            }
            
            if (article == null)
            {
                return NotFound();
            }

            // Only increment view count for published articles and if it's not the author viewing their own article
            if (article.IsPublished)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.Id != article.AuthorId)
                {
                    // Check if this user/session has already viewed this article
                    string viewKey = $"viewed_article_{article.Id}";
                    string userIdentifier = currentUser?.Id ?? HttpContext.Session.Id;
                    string sessionViewKey = $"{viewKey}_{userIdentifier}";
                    
                    // Only increment if not viewed in this session
                    if (HttpContext.Session.GetString(sessionViewKey) == null)
                    {
                        article.ViewCount++;
                        await _articleService.UpdateArticle(article);
                        
                        // Mark as viewed in this session (expires when session ends)
                        HttpContext.Session.SetString(sessionViewKey, "viewed");
                    }
                }
            }

            var comments = (await _commentService.GetCommentsByArticleId(article.Id)).ToList();

            var viewModel = new BlogViewModel
            {
                Article = article,
                Comments = comments
            };

            return View(viewModel);
        }
    }
}
