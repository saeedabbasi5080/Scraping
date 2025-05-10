//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System.Threading.Tasks;

//public class SeleniumScraper
//{
//    private readonly string _loginUrl = "YOUR_LOGIN_URL"; // جایگزین کنید
//    private readonly string _username = "YOUR_USERNAME"; // جایگزین کنید
//    private readonly string _password = "YOUR_PASSWORD"; // جایگزین کنید
//    private readonly string _protectedUrl = "YOUR_PROTECTED_URL"; // جایگزین کنید

//    public async Task<string> ScrapeProtectedContentWithSeleniumAsync()
//    {
//        using (var driver = new ChromeDriver()) // یا هر درایور مرورگر دیگری
//        {
//            driver.Navigate().GoToUrl(_loginUrl);

//            // پیدا کردن عناصر فرم لاگین و وارد کردن اطلاعات
//            var usernameField = driver.FindElement(By.Id("username_field_id")); // شناسه فیلد کاربری
//            usernameField.SendKeys(_username);

//            var passwordField = driver.FindElement(By.Id("password_field_id")); // شناسه فیلد رمز عبور
//            passwordField.SendKeys(_password);

//            var loginButton = driver.FindElement(By.Id("login_button_id")); // شناسه دکمه لاگین
//            loginButton.Click();

//            // صبر کنید تا لاگین انجام شود (ممکن است نیاز به Wait داشته باشید)
//            await Task.Delay(3000); // مثال: 3 ثانیه صبر

//            // رفتن به صفحه محافظت شده
//            driver.Navigate().GoToUrl(_protectedUrl);

//            // دریافت محتوای صفحه
//            return driver.PageSource;
//        }
//    }
//}