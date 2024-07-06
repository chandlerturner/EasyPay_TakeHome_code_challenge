using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;

    public ProductService(IProductRepository productRepository, ICacheService cacheService)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllProductsAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _cacheService.GetOrAddAsync($"Product_{id}", async () =>
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product is null)
                {
                    throw new KeyNotFoundException();
                }
                return product;
            }
            , slidingExpiration: TimeSpan.FromMinutes(30));
    }

    public async Task<int> CreateProductAsync(Product product)
    {
        var id = await _productRepository.CreateProductAsync(product);
        await _cacheService.RemoveAsync($"Product_{id}");

        return id;
    }

    public async Task UpdateProductAsync(Product product)
    {
        await _productRepository.UpdateProductAsync(product);
        await _cacheService.RemoveAsync($"Product_{product.Id}");
    }

    public async Task DeleteProductAsync(int id)
    {
        await _productRepository.DeleteProductAsync(id);
        await _cacheService.RemoveAsync($"Product_{id}");
    }
}