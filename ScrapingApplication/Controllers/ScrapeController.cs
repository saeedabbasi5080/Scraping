// Controllers/ScrapeController.cs
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using ScrapingApplication;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ScrapeController : ControllerBase
{
    private readonly ScrapingService _scrapingService;

    public ScrapeController(ScrapingService scrapingService)
    {
        _scrapingService = scrapingService;
    }

    [HttpGet("preview")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> PreviewScrapedData()
    {
        try
        {
            var htmlContent = await _scrapingService.GetWebsiteContentAsync();
            var scrapedArticles = await _scrapingService.ScrapeArticlesForPreviewAsync(htmlContent);

            if (scrapedArticles != null && scrapedArticles.Any())
            {
                return Ok(scrapedArticles);
            }
            else
            {
                return NotFound("هیچ مقاله ای برای پیش نمایش پیدا نشد.");
            }
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"خطا در درخواست وب‌سایت: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"خطا در هنگام اسکرپ کردن: {ex.Message}");
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveScrapedDataToDatabase()
    {
        await _scrapingService.ScrapeAndSaveArticlesAsync(); // نیاز به ایجاد این متد در ScrapingService
        return Ok("داده ها با موفقیت ذخیره شدند.");
    }
}