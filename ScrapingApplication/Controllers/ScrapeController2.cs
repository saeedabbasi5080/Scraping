using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ScrapeController2 : ControllerBase
{
    [HttpGet("links")]
    public async Task<IActionResult> GetAllLinks([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("آدرس سایت الزامی است.");

        try
        {
            using var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var aTags = htmlDoc.DocumentNode.SelectNodes("//a");

            var result = new List<object>();

            if (aTags != null)
            {
                foreach (var tag in aTags)
                {
                    var href = tag.GetAttributeValue("href", "");
                    var text = tag.InnerText.Trim();
                    result.Add(new { Text = text, Href = href });
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
