using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Narratify.Models;
using Narratify.Services.Interfaces;
using Narratify.Models.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Narratify.Models.Entities;

namespace Narratify.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IArticleService _articleService;
    private readonly SignInManager<User> _signInManager;

    public HomeController(ILogger<HomeController> logger, IArticleService articleService, SignInManager<User> signInManager)
    {
        _logger = logger;
        _articleService = articleService;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    {
        // Allow both logged-in and anonymous users to view the home page with published articles
        var featuredArticles = (await _articleService.GetPublishedArticles())
            .OrderByDescending(a => a.PublishedAt)
            .Take(6)
            .ToList();

        var viewModel = new HomeViewModel
        {
            FeaturedArticles = featuredArticles
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
