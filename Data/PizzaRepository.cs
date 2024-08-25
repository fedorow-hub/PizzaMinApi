
using System.Collections.Immutable;
using System.Diagnostics;


public class PizzaRepository : IPizzaRepository
{
    private readonly PizzaDb _context;
    public PizzaRepository(PizzaDb context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<List<Product>> GetProductsBySearchParamAsync(string searchParam)
    {
        var products = await _context.Products.ToListAsync();
        return products.Where(p => p.Name.Contains(searchParam, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<ProductDto> GetProductAsync(int productId)
    {
        var product = await _context.Products
                                   .Include(c => c.Ingredients)
                                   .Include(c => c.Items)
                                   .Where(c => c.Id == productId)
                                   .FirstOrDefaultAsync();

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Ingredients = product.Ingredients.Select(i => new IngredientDto
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                ImageUrl = i.ImageUrl
            }).ToList(),
            ProductItems = product.Items.Select(i => new ProductItemDto
            {
                Id = i.Id,
                Price = i.Price,
                Size = i.Size,
                PizzaType = i.PizzaType
            }).ToList()
        };

        return productDto;
    }

    public async Task InsertProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { product.Id });

        if (productFromDb == null) return;

        productFromDb.Category = product.Category;
        productFromDb.CategoryId = product.CategoryId;
        productFromDb.UpdatedAt = DateTime.Now;
        productFromDb.ImageUrl = product.ImageUrl;
        productFromDb.Name = product.Name;
        productFromDb.Ingredients = product.Ingredients;
        productFromDb.Items = product.Items;
    }

    public async Task DeleteProductAsync(int productId)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { productId });

        if (productFromDb == null) return;

        _context.Products.Remove(productFromDb);
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        return await _context.Ingredients.ToListAsync();
    }

    public async Task<List<CategoryDto>> GetCategoresAsync()
    {
        var categories = await _context.Categories
                                   .Include(c => c.Products).ThenInclude(p => p.Ingredients)
                                   .Include(c => c.Products).ThenInclude(p => p.Items)
                                   .Where(c => c.Products.Any())
                                   .ToListAsync();

        var categoryDtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Products = c.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                ProductItems = p.Items.Select(i => new ProductItemDto
                {
                    Id = i.Id,
                    Price = i.Price,
                    Size = i.Size,
                    PizzaType = i.PizzaType
                }).ToList(),
                Ingredients = p.Ingredients.Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    ImageUrl = i.ImageUrl
                }).ToList()
            }).ToList()
        }).ToList();

        return categoryDtos;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<CartDto> GetCartAsync(string token)
    {
        var cart = await _context.Carts
                                   .Include(c => c.CartItems).ThenInclude(p => p.Ingredients)
                                   .Include(c => c.CartItems).ThenInclude(p => p.ProductItem).ThenInclude(pi => pi.Product)
                                   .Where(c => c.TokenId == token)
                                   .FirstOrDefaultAsync();

        if (cart != null)
        {
            var cartDto = new CartDto
            {
                Id = cart.Id,
                TotalAmount = cart.TotalAmount,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductItem = new ProductItemDto
                    {
                        Id = ci.ProductItem.Id,
                        Price = ci.ProductItem.Price,
                        Size = ci.ProductItem.Size,
                        PizzaType = ci.ProductItem.PizzaType,
                        Product = new ProductDto
                        {
                            Id = ci.ProductItem.Product.Id,
                            Name = ci.ProductItem.Product.Name,
                            ImageUrl = ci.ProductItem.Product.ImageUrl
                        }
                    },
                    Ingredients = ci.Ingredients.Select(i => new IngredientDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Price = i.Price,
                        ImageUrl = i.ImageUrl
                    }).ToList(),
                    Quantity = ci.Quantity,
                    CreatedAt = ci.CreatedAt
                }).ToList()
            };
            return cartDto;
        }

        return new CartDto
        {
            Id = 0,
            TotalAmount = 0,
            CartItems = []
        };
    }

    public async Task<CartDto> PatchCartAsync(string token, int cartItemId, int quantity)
    {
        var cartItemDb = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);

        if (cartItemDb == null) return null;

        cartItemDb.Quantity = quantity;
        await _context.SaveChangesAsync();// TODO возможно надо будет сделать только раз в конце

        return await UptateCartTotalAmountAsync(token);
    }

    //функция обновления всей корзины
    private async Task<CartDto> UptateCartTotalAmountAsync(string token)
    {
        var cart = await _context.Carts
                                   .Include(c => c.CartItems).ThenInclude(p => p.Ingredients)
                                   .Include(c => c.CartItems).ThenInclude(p => p.ProductItem).ThenInclude(pi => pi.Product)
                                   .Where(c => c.TokenId == token)
                                   .FirstOrDefaultAsync();

        if (cart == null) return null;

        var totalAmount = 0.0;

        foreach (var item in cart.CartItems)
        {
            var itemPrice = item.ProductItem.Price;

            var ingredientsTotalPrice = 0;

            foreach (var ingredient in item.Ingredients)
            {
                ingredientsTotalPrice = ingredientsTotalPrice + ingredient.Price;
            }

            totalAmount = (itemPrice + ingredientsTotalPrice) * item.Quantity;
        }

        cart.TotalAmount = totalAmount;
        cart.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        var cartDto = new CartDto
        {
            Id = cart.Id,
            TotalAmount = totalAmount,
            CartItems = cart.CartItems.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                ProductItem = new ProductItemDto
                {
                    Id = ci.ProductItem.Id,
                    Price = ci.ProductItem.Price,
                    Size = ci.ProductItem.Size,
                    PizzaType = ci.ProductItem.PizzaType,
                    Product = new ProductDto
                    {
                        Id = ci.ProductItem.Product.Id,
                        Name = ci.ProductItem.Product.Name,
                        ImageUrl = ci.ProductItem.Product.ImageUrl
                    }
                },
                Ingredients = ci.Ingredients.Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    ImageUrl = i.ImageUrl
                }).ToList(),
                Quantity = ci.Quantity,
                CreatedAt = ci.CreatedAt
            }).ToList()
        };
        return cartDto;
    }
}
