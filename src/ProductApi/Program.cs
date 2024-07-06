using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<IProductRepository, SqlServerDapperProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/products", async (IProductService productService) => 
    Results.Ok(await productService.GetAllProductsAsync()));

app.MapGet("/products/{id}", async (int id, IProductService productService) =>
{
    try
    {
        return Results.Ok(await productService.GetProductByIdAsync(id));
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapPut("/products/{id}", async (int id, Product product, IProductService productService) =>
{
    product.Id = id;
    await productService.UpdateProductAsync(product);
    return Results.NoContent();
});

app.MapPost("/products", async (Product product, IProductService productService) =>
{
    var id = await productService.CreateProductAsync(product);
    return Results.Created($"/products/{id}", await productService.GetProductByIdAsync(id));
});

app.MapDelete("/products/{id}", async (int id, IProductService productService) =>
{
    await productService.DeleteProductAsync(id);
    return Results.NoContent();
});

app.Run();