using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ScrapeController7 : ControllerBase
{
    [HttpGet("exercise-names")]
    public IActionResult GetExerciseNames([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("لطفاً آدرس سایت را وارد کنید.");

        try
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // اجرای Chrome در حالت headless
            using var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);

            // صبر کنید تا عنصر مورد نظر بارگذاری شود (مثال)
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
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