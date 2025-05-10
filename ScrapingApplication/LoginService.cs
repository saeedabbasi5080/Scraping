using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LoginService
{
    private readonly HttpClient _httpClient;
    private readonly string _loginUrl = "YOUR_LOGIN_URL"; // جایگزین کنید
    private readonly string _username = "YOUR_USERNAME"; // جایگزین کنید
    private readonly string _password = "YOUR_PASSWORD"; // جایگزین کنید

    public LoginService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> LoginAsync()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "username_field_name", _username }, // نام فیلد کاربری در فرم لاگین
            { "password_field_name", _password }, // نام فیلد رمز عبور در فرم لاگین
            // اگر توکن CSRF وجود دارد، باید آن را هم از صفحه لاگین استخراج و اضافه کنید
        });

        var response = await _httpClient.PostAsync(_loginUrl, content);

        if (response.IsSuccessStatusCode)
        {
            // لاگین موفقیت آمیز بود، کوکی ها در HttpClient ذخیره شده اند
            return true;
        }
        else
        {
            // لاگین ناموفق
            return false;
        }
    }

    public async Task<string> GetProtectedContentAsync(string protectedUrl)
    {
        // بعد از لاگین موفقیت آمیز، می توانید درخواست های بعدی را ارسال کنید
        var response = await _httpClient.GetAsync(protectedUrl);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // دسترسی به محتوای محافظت شده امکان پذیر نیست
            return null;
        }
    }
}