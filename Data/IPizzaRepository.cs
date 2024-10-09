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
    Task<List<Category>> GetCategoresAsync();
    Task<List<Category>> DeleteCategoryAsync(int categoryId);
    Task InsertCategoryAsync(CategoryDto category);
    Task<List<Category>> UpdateCategoryAsync(int categoryId, CategoryDto category);

    Task<CartDto> FindOrCreateCartAsync(string token);
    Task<CartDto> PatchCartItemAsync(string token, int id, int quantity);
    Task<CartDto> DeleteCartItemAsync(string token, int cartItemId);
    Task<CartDto> FindOrCreateCartItem(string token, CreateCartItemValues cartItem, int userCartId);

    Task<string> CreateOrderAndCreatingPaymentURL(string token, OrderDTO order);

    Task PaymentCallbackHandle(PaymentObject paymentObject);

    Task<List<User>> GetAllUsersAsync();

    Task InsertUserAsync(UserVM user);

    Task<List<User>> DeleteUserAsync(int userId);

    Task<List<User>> UpdateUserAsync(int userId, UserVM user);

    Task SaveAsync();
}