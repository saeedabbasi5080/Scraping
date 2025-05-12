using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[controller]")]
public class ScrapeController8 : ControllerBase
{
    private readonly string _authToken = "**"; // **جایگزین کنید**

    [HttpGet("exercise-names")]
    public IActionResult GetExerciseNames([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("لطفاً آدرس سایت را وارد کنید.");

        try
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            using var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);

            // استخراج دامنه از URL
            Uri uri = new Uri(url);
            string domain = uri.Host;

            // اضافه کردن Cookie حاوی auth-token
            var authTokenCookie = new Cookie("auth-token", _authToken, domain, "/", DateTime.Now.AddDays(1));
            driver.Manage().Cookies.AddCookie(authTokenCookie);

            // بارگذاری مجدد صفحه برای اعمال Cookie (ممکن است همیشه لازم نباشد)
            driver.Navigate().GoToUrl(url);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.XPath("//p[@class='sc-5f9bb2b1-8 hKZjAp']")) != null);

            var html = driver.PageSource;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

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
        catch (Exception ex)
        {
            return StatusCode(500, $"خطا: {ex.Message}");
        }
    }
}