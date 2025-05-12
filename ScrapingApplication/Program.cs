using ScrapingApplication.Db;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });

    // 🔐 Add JWT Authorization
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "توکن JWT خود را وارد کنید. فقط مقدار بدون Bearer بنویس.",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



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