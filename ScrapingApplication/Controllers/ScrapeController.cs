using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ScrapeController : ControllerBase
{
    [HttpGet("title")]
    public async Task<IActionResult> GetPageTitle([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("لطفاً آدرس سایت را وارد کنید.");

        try
        {
            using var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var titleNode = doc.DocumentNode.SelectSingleNode("//noscript");

            if (titleNode == null)
                return NotFound("تگ <title> در صفحه پیدا نشد.");

            var title = titleNode.InnerText.Trim();
            return Ok(new { Title = title });
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"خطا در ارتباط با سایت: {ex.Message}");
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"خطای غیرمنتظره: {ex.Message}");
        }
    }
}
