
using System.Collections.Immutable;
using System.Diagnostics;
using System.Security.Authentication;


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

    public async Task<CartDto> FindOrCreateCartAsync(string token)
    {
        var cart = await _context.Carts
                                   .Include(c => c.CartItems).ThenInclude(p => p.Ingredients)
                                   .Include(c => c.CartItems).ThenInclude(p => p.ProductItem).ThenInclude(pi => pi.Product)
                                   .Where(c => c.TokenId == token)
                                   .FirstOrDefaultAsync();

        if (cart == null)
        {
            cart = new Cart
            {
                TokenId = token,
                TotalAmount = 0,
                CartItems = new List<CartItem>(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _context.Carts.AddAsync(cart);

            await _context.SaveChangesAsync();

            var cartFromDB = await _context.Carts
                                   .Where(c => c.TokenId == token)
                                   .FirstOrDefaultAsync();

            var cartVM = new CartDto
            {
                Id = cartFromDB.Id,
                TokenId = token,
                TotalAmount = 0,
                CartItems = new List<CartItemDto>()
            };
            return cartVM;
        }

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
                Quantity = ci.Quantity
            }).ToList()
        };
        return cartDto;
    }

    public async Task<CartDto> PatchCartItemAsync(string token, int cartItemId, int quantity)
    {
        var cartItemDb = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);

        if (cartItemDb == null) return null;

        cartItemDb.Quantity = quantity;

        return await UptateCartTotalAmountAsync(token);
    }

    public async Task<CartDto> DeleteCartItemAsync(string token, int cartItemId)
    {
        var cartItemFromDb = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);

        if (cartItemFromDb == null) return null;

        _context.CartItems.Remove(cartItemFromDb);

        return await UptateCartTotalAmountAsync(token);
    }

    public async Task<CartDto> FindOrCreateCartItem(string token, CreateCartItemValues cartItem, int userCartId)
    {
        var findCartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == userCartId && ci.ProductItemId == cartItem.ProductItemId);

        var isFullTheSame = false;
        var ingreditents = new List<Ingredient>();

        if(cartItem.IngredientsIds != null && cartItem.IngredientsIds.Count > 0) 
        {
            ingreditents = (from ingredient in _context.Ingredients where cartItem.IngredientsIds.Contains(ingredient.Id) select ingredient).ToList();
            isFullTheSame = findCartItem == null ? false : findCartItem.Ingredients.Select(c => c.Id).OrderBy(x => x).SequenceEqual(cartItem.IngredientsIds.OrderBy(x => x));
        }

        if((cartItem.IngredientsIds == null || cartItem.IngredientsIds.Count == 0) && findCartItem?.Ingredients.Count == 0) 
        {
            isFullTheSame = true;
        }

        var productItem = await _context.ProductItems.Include(pi => pi.Product).FirstOrDefaultAsync(pi => pi.Id == cartItem.ProductItemId);
        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == userCartId);

        if (productItem == null || cart == null)
        {
            throw new Exception("ProductItem or Cart not found");
        }

        if (isFullTheSame)
        {
            //если найден аналогичный cartItem, то просто прибавляем еще один
            if (findCartItem != null)
            {
                findCartItem.Quantity++;
                _context.CartItems.Update(findCartItem);
            }
            return await UptateCartTotalAmountAsync(token);
        }
        else
        {
            //если не найден аналогичный cartItem, то создаем новый и записываем в БД
            var item = new CartItem
            {
                ProductItem = productItem,
                Cart = cart,
                Quantity = 1,
                Ingredients = ingreditents,
                CreatedAt = DateTime.Now
            };
            await _context.CartItems.AddAsync(item);

            await _context.SaveChangesAsync();

            return await UptateCartTotalAmountAsync(token);
        };
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

            totalAmount = totalAmount + (itemPrice + ingredientsTotalPrice) * item.Quantity;
        }

        cart.TotalAmount = totalAmount;
        cart.UpdatedAt = DateTime.Now;

        var cartDto = new CartDto
        {
            Id = cart.Id,
            TotalAmount = totalAmount,
            TokenId = token,
            CartItems = cart.CartItems.OrderByDescending(ci => ci.CreatedAt).Select(ci => new CartItemDto
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
                Quantity = ci.Quantity
            }).ToList()
        };
        return cartDto;
    }
}
