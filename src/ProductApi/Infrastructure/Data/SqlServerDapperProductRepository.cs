using System.Data;
using Dapper;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using System.Data.SqlClient;

namespace ProductApi.Infrastructure.Data;

public class SqlServerDapperProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;

    public SqlServerDapperProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var sql = "SELECT * FROM Products";
        return await _dbConnection.QueryAsync<Product>(sql);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var sql = "SELECT * FROM Products WHERE Id = @Id";
        return (await _dbConnection.QueryAsync<Product>(sql, new { Id = id })).FirstOrDefault();
    }

    public async Task<int> CreateProductAsync(Product product)
    {
        var sql = "INSERT INTO Products (Name, Brand, Price) VALUES (@Name, @Brand, @Price); SELECT CAST(SCOPE_IDENTITY() as int)";
        var id = (await _dbConnection.QueryAsync<int>(sql, product)).FirstOrDefault();

        return id;
    }

    public async Task UpdateProductAsync(Product product)
    {
        var sql = "UPDATE Products SET Name = @Name, Brand = @Brand, Price = @Price WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var sql = "DELETE FROM Products WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, new { Id = id });
    }
}