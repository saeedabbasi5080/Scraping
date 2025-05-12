using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ScrapeController4 : ControllerBase
{
    [HttpGet("exercise-names")]
    public async Task<IActionResult> GetExerciseNames([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("لطفاً آدرس سایت را وارد کنید.");

        try
        {
            using var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//p[contains(@class, 'sc-5f9bb2b1-8') and contains(@class, 'hKZjAp')]");


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
            return StatusCode(500, $"خطای ارتباطی: {ex.Message}");
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"خطای عمومی: {ex.Message}");
        }
    }
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
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", ""));

            var html = await httpClient.GetStringAsync(url);

            // 👇 این خط رو برای تست محتوای HTML اضافه کن
            return Content(html, "text/html");

            // بقیه کدها بعداً اینجا میان (برای پارس کردن HTML)
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

