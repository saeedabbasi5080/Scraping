using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class ScrapingService
{
    public async Task<string> GetWebsiteContentAsync(string url)
    {
        using var httpClient = new HttpClient();
        return await httpClient.GetStringAsync(url);
    }

    public async Task<List<ArticleDto>> ScrapeArticlesForPreviewAsync(string htmlContent)
    {
        var articles = new List<ArticleDto>();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // پیدا کردن همه لینک‌ها
        var nodes = htmlDoc.DocumentNode.SelectNodes("//a");

        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                var href = node.GetAttributeValue("href", string.Empty);
                var text = node.InnerText.Trim();

                if (!string.IsNullOrWhiteSpace(href))
                {
                    articles.Add(new ArticleDto
                    {
                        Title = text,
                    });
                }
            }
        }

        return articles;
    }
}
