using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Repositories;
using ProductsApplication.Interfaces;
using ProductsApplication.Services;
using Serilog;

// 1. إعداد Serilog الأولي لتسجيل الأخطاء حتى أثناء الإقلاع
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "ProductsApi")
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting web application...");

    var builder = WebApplication.CreateBuilder(args);
    

    // 2. دمج Serilog مع الـ Host الخاص بالتطبيق
    builder.Host.UseSerilog();

    // 3. إضافة الخدمات و الـ Controllers و Swagger
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // 4. إعداد قاعدة البيانات المؤقتة في الذاكرة (In-Memory)
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("ProductsDb"));

    // 5. تسجيل الـ Repositories والخدمات في الـ Dependency Injection
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<ProductService>();

    var app = builder.Build();

    // 6. إعداد الـ Pipeline والـ Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseSerilogRequestLogging();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}