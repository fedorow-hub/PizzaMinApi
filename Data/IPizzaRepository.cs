public interface IPizzaRepository : IDisposable
{
    Task<List<Product>> GetProductsAsync();
    Task<List<Product>> GetProductsBySearchParamAsync(string searchParam);
    Task<ProductDto> GetProductAsync(int productId);
    Task InsertProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int productId);

    Task<List<StoryDTO>> GetStoriesAsync();

    Task<List<Ingredient>> GetIngredientsAsync();
    Task<List<CategoryDto>> GetCategoresAsync(double minPrice, double maxPrice, int[]? sizes, int[]? pizzaTypes, int[]? ingredientsIdArr);

    Task<CartDto> FindOrCreateCartAsync(string token);
    Task<CartDto> PatchCartItemAsync(string token, int id, int quantity);
    Task<CartDto> DeleteCartItemAsync(string token, int cartItemId);
    Task<CartDto> FindOrCreateCartItem(string token, CreateCartItemValues cartItem, int userCartId);

    Task<string> CreateOrderAndCreatingPaymentURL(string token, OrderDTO order);

    Task PaymentCallbackHandle(PaymentObject paymentObject);

    Task SaveAsync();
}