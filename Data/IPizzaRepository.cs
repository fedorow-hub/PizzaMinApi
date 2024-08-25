public interface IPizzaRepository : IDisposable
{
    Task<List<Product>> GetProductsAsync();
    Task<List<Product>> GetProductsBySearchParamAsync(string searchParam);
    Task<ProductDto> GetProductAsync(int productId);
    Task InsertProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int productId);

    Task<List<Ingredient>> GetIngredientsAsync();
    Task<List<CategoryDto>> GetCategoresAsync();

    Task<CartDto> GetCartAsync(string token);
    Task<CartDto> PatchCartAsync(string token, int id, int quantity);

    Task SaveAsync();
}