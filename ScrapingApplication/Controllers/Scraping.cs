using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ScrapeController3 : ControllerBase
{
    [HttpGet("nav-links")]
    public async Task<IActionResult> GetSpanNavLinkText([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("لطفاً آدرس سایت را وارد کنید.");

        try
        {
            using var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // XPath: انتخاب تمام span هایی که کلاسشان nav-link-text است
            var spanNodes = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'nav-link-text')]");

            var result = new List<string>();

            if (spanNodes != null)
            {
                foreach (var span in spanNodes)
                {
                    var text = span.InnerText.Trim();
                    if (!string.IsNullOrEmpty(text))
                        result.Add(text);
                }
            }

            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"خطا در دریافت سایت: {ex.Message}");
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"خطای عمومی: {ex.Message}");
        }
    }
}
