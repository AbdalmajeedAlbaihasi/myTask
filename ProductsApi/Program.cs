using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Repositories;
using ProductsApplication.Interfaces;
using ProductsApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. إضافة الـ Controllers والـ Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. إعداد قاعدة البيانات المؤقتة في الذاكرة (In-Memory)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

// 3. تسجيل الخدمات في الـ Dependency Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// 4. إعداد الـ Pipeline والـ Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();