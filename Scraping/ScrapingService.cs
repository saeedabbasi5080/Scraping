// Services/ScrapingService.cs
using HtmlAgilityPack;
using Scraping;
using Scraping.Entities;
using System.Net.Http;
using System.Threading.Tasks;

public class ScrapingService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _dbContext;
    private readonly string _urlToScrape = "https://example.com/blog"; // جایگزین کردن با URL واقعی

    public ScrapingService(HttpClient httpClient, AppDbContext dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task ScrapeArticlesAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_urlToScrape);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            // مثال: پیدا کردن تمام لینک‌های داخل تگ‌های <a> با کلاس "article-link"
            var articleNodes = htmlDocument.DocumentNode.SelectNodes("//a[@class='article-link']");

            if (articleNodes != null)
            {
                foreach (var node in articleNodes)
                {
                    var title = node.InnerText?.Trim();
                    var url = node.GetAttributeValue("href", "")?.Trim();

                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(url))
                    {
                        //var newArticle = new Exercises { Title = title, Url = url };
                        //_dbContext.Exercises.Add(newArticle);
                    }
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