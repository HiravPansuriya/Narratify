
using Markdig;
using Narratify.Services.Interfaces;

namespace Narratify.Services.Implementations
{
    public class MarkdownService : IMarkdownService
    {
        public string ConvertToHtml(string markdown)
        {
            return Markdown.ToHtml(markdown);
        }
    }
}
