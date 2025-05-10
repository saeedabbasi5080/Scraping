using ScrapingApplication.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// اضافه کردن HttpClient به عنوان یک سرویس
builder.Services.AddHttpClient<ScrapingService>();

// اضافه کردن DbContext به عنوان یک سرویس
builder.Services.AddDbContext<AppDbContext>();

// اضافه کردن ScrapingService به عنوان یک سرویس
builder.Services.AddTransient<ScrapingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// دریافت نمونه ای از ScrapingService و اجرای آن پس از ساخت برنامه
using (var scope = app.Services.CreateScope())
{
    var scrapingService = scope.ServiceProvider.GetRequiredService<ScrapingService>();
    //await scrapingService.ScrapeArticlesAsync();
}

app.Run();