using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Narratify.Services.Interfaces;
using System.Security.Claims;

namespace Narratify.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string returnUrl = "")
        {
            try
            {
                var comment = await _commentService.GetCommentById(id);
                
                if (comment == null)
                {
                    TempData["ErrorMessage"] = "Comment not found.";
                    return RedirectToReturnUrl(returnUrl);
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var isAdmin = User.IsInRole("Admin");

                // Check if the current user is the comment author or an admin
                if (comment.UserId != currentUserId && !isAdmin)
                {
                    TempData["ErrorMessage"] = "You don't have permission to delete this comment.";
                    return RedirectToReturnUrl(returnUrl);
                }

                await _commentService.DeleteComment(id);
                TempData["SuccessMessage"] = "Comment deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the comment. Please try again.";
            }

            return RedirectToReturnUrl(returnUrl);
        }

        private IActionResult RedirectToReturnUrl(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
