using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ScrapeController5 : ControllerBase
{
    [HttpGet("secure-extract")]
    public async Task<IActionResult> ExtractProtectedContent(
        [FromQuery] string url,
        [FromHeader(Name = "Authorization")] string authHeader)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("آدرس سایت الزامی است.");

        if (string.IsNullOrWhiteSpace(authHeader))
            return BadRequest("توکن احراز هویت در هدر Authorization ارسال نشده است.");

        try
        {
            using var httpClient = new HttpClient();

            // تنظیم توکن در هدر Authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", ""));

            // ارسال درخواست به سایت
            var html = await httpClient.GetStringAsync(url);

            // پردازش HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // استخراج داده‌ها (مثلاً همه تگ‌های p خاص)
            var nodes = doc.DocumentNode.SelectNodes("//p[@class='sc-5f9bb2b1-8 hKZjAp']");

            var results = new List<string>();

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var text = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(text))
                        results.Add(text);
                }
            }

            return Ok(results);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"خطا در دریافت محتوا: {ex.Message}");
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"خطای دیگر: {ex.Message}");
        }
    }
}
