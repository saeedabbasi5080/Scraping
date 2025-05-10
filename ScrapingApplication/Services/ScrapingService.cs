//// Services/ScrapingService.cs
//using HtmlAgilityPack;
//using ScrapingApplication;
//using ScrapingApplication.Entities;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace ScrapingApplication;

//public class ScrapingService
//{
//    private readonly HttpClient _httpClient;
//    private readonly AppDbContext _dbContext;
//    private readonly string _urlToScrape = "https://example.com/blog"; // جایگزین کردن با URL واقعی

//    public ScrapingService(HttpClient httpClient, AppDbContext dbContext)
//    {
//        _httpClient = httpClient;
//        _dbContext = dbContext;
//    }

//    public async Task ScrapeArticlesAsync()
//    {
//        try
//        {
//            var response = await _httpClient.GetStringAsync(_urlToScrape);
//            var htmlDocument = new HtmlDocument();
//            htmlDocument.LoadHtml(response);

//            // مثال: پیدا کردن تمام لینک‌های داخل تگ‌های <a> با کلاس "article-link"
//            var articleNodes = htmlDocument.DocumentNode.SelectNodes("//a[@class='article-link']");

//            if (articleNodes != null)
//            {
//                foreach (var node in articleNodes)
//                {
//                    var title = node.InnerText?.Trim();
//                    var url = node.GetAttributeValue("href", "")?.Trim();

//                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(url))
//                    {
//                        //var newArticle = new Exercises { Title = title, Url = url };
//                        //_dbContext.Exercises.Add(newArticle);
//                    }
//                }

//                await _dbContext.SaveChangesAsync();
//                Console.WriteLine("مقالات با موفقیت ذخیره شدند.");
//            }
//            else
//            {
//                Console.WriteLine("هیچ مقاله ای پیدا نشد.");
//            }
//        }
//        catch (HttpRequestException ex)
//        {
//            Console.WriteLine($"خطا در درخواست وب‌سایت: {ex.Message}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"خطا در هنگام اسکرپ کردن یا ذخیره سازی: {ex.Message}");
//        }
//    }
//}



// Services/ScrapingService.cs
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScrapingApplication.Entities;
using ScrapingApplication.Db;

public class ScrapingService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _dbContext;
    //private readonly string _urlToScrape = "https://app.hevycoach.com/exercises"; // جایگزین کردن با URL واقعی
    private readonly string _urlToScrape = "https://digikala.com"; // جایگزین کردن با URL واقعی

    public ScrapingService(HttpClient httpClient, AppDbContext dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task<string> GetWebsiteContentAsync()
    {
        return await _httpClient.GetStringAsync(_urlToScrape);
    }



    public async Task<List<ArticleDto>> ScrapeArticlesForPreviewAsync(string htmlContent)
    {
        var scrapedArticles = new List<ArticleDto>();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);

        var articleNodes = htmlDocument.DocumentNode.SelectNodes("//body");

        //var articleNodes2 = htmlDocument.querySelectorAll(".fmdjmi");
        //articleNodes2[0].children[0].children.find(baby => baby.outerText === "Dumble") undefinded : node
        //articleNodes2[0].children[0].children.outerText
        if (articleNodes == null)
        {
            return [];
        }

        var firstList = articleNodes.Nodes().ToList();
        List<HtmlNode> detectedCollection = [];
        foreach (var node in firstList)
        {
            DetectA(node);
        }

        void DetectA(HtmlNode nc)
        {
            Console.WriteLine("Saeed* -> " + nc.Name ?? "Null");
            if (nc.Name == "a")
            {
                detectedCollection.Add(nc);
            }
            if (nc.ChildNodes != null && nc.ChildNodes.Count > 0)
            {
                foreach (var node in nc.ChildNodes)
                {
                    DetectA(node);
                }
            }
        }

        Console.WriteLine(detectedCollection);

        if (articleNodes != null)
        {
            foreach (var node in articleNodes)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    Console.WriteLine(childNode);
                }
                /*var title = node.InnerText?.Trim();
                var url = node.GetAttributeValue("href", "")?.Trim();

                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(url))
                {
                    scrapedArticles.Add(new ArticleDto { Title = title, Url = url });
                }*/
            }
        }

        return scrapedArticles;
    }

    public async Task ScrapeAndSaveArticlesAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_urlToScrape);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            var articleNodes = htmlDocument.DocumentNode.SelectNodes("//a[@class='article-link']");

            if (articleNodes != null)
            {
                foreach (var node in articleNodes)
                {
                    var title = node.InnerText?.Trim();
                    var url = node.GetAttributeValue("href", "")?.Trim();

                    //if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(url))
                    //{
                    //    var existingArticle = await _dbContext.Exercises.FirstOrDefaultAsync(a => a.Url == url);
                    //    if (existingArticle == null)
                    //    {
                    //        var newArticle = new Exercises { Title = title, Url = url };
                    //        _dbContext.Exercises.Add(newArticle);
                    //    }
                    //}
                }

                await _dbContext.SaveChangesAsync();
                Console.WriteLine("مقالات با موفقیت ذخیره شدند.");
            }
            else
            {
                Console.WriteLine("هیچ مقاله ای پیدا نشد.");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"خطا در درخواست وب‌سایت: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"خطا در هنگام اسکرپ کردن یا ذخیره سازی: {ex.Message}");
        }
    }
}