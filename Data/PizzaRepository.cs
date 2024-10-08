
using System.Collections.Immutable;
using System.Diagnostics;
using System.Security.Authentication;
using Newtonsoft.Json;
using Resend;


public class PizzaRepository : IPizzaRepository
{
    private readonly PizzaDb _context;
    private readonly IResend _resend;
    public PizzaRepository(PizzaDb context, ResendClient resend)
    {
        _context = context;
        _resend = resend;
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

    public async Task<List<StoryDTO>> GetStoriesAsync()
    {
        var stories = await _context.Stores
            .Include(s => s.StoryItems)
            .ToListAsync();

        var storyDto = stories.Select(s => new StoryDTO
        {
            Id = s.Id,
            PreviewImageUrl = s.PreviewImageUrl,
            StoryItems = s.StoryItems.Select(i => new StoryItemDTO
            {
                Id = i.Id,
                SourceUrl = i.SourceUrl
            }).ToList()
        }).ToList();
        return storyDto;
    }

    public async Task<List<CategoryDto>> GetCategoresAsync(double minPrice, double maxPrice, int[]? sizes, int[]? pizzaTypes, int[]? ingredientsIdArr)
    {
        //TODO Доработать фильтрацию
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
                ProductItems = p.Items.Where(item => item.Price >= minPrice && item.Price <= maxPrice).OrderBy(item => item.Price).Select(i => new ProductItemDto
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

    public async Task<List<Category>> GetCategoresAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<List<Category>> DeleteCategoryAsync(int categoryId)
    {
        var categoryFromDb = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

        if (categoryFromDb == null) return null;

        _context.Categories.Remove(categoryFromDb);
        await _context.SaveChangesAsync();
        return await _context.Categories.ToListAsync();
    }

    public async Task<List<Category>> UpdateCategoryAsync(int categoryId, CategoryDto category)
    {
        Category categroryFromDB = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        categroryFromDB.Name = category.Name;

        _context.Categories.Update(categroryFromDB);
        await _context.SaveChangesAsync();
        return await _context.Categories.ToListAsync();
    }

    //(ingredientsIdArr == null ? p.Ingredients : p.Ingredients.Where(i => ingredientsIdArr.Contains(i.Id)))

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

        if (cartItem.IngredientsIds != null && cartItem.IngredientsIds.Count > 0)
        {
            ingreditents = (from ingredient in _context.Ingredients where cartItem.IngredientsIds.Contains(ingredient.Id) select ingredient).ToList();
            isFullTheSame = findCartItem == null ? false : findCartItem.Ingredients.Select(c => c.Id).OrderBy(x => x).SequenceEqual(cartItem.IngredientsIds.OrderBy(x => x));
        }

        if ((cartItem.IngredientsIds == null || cartItem.IngredientsIds.Count == 0) && findCartItem?.Ingredients.Count == 0)
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

    public async Task<string> CreateOrderAndCreatingPaymentURL(string token, OrderDTO orderDTO)
    {
        //находим корзину по userId или по токену
        var cart = await _context.Carts
            .Include(c => c.CartItems).ThenInclude(p => p.Ingredients)
            .Include(c => c.CartItems).ThenInclude(p => p.ProductItem).ThenInclude(pi => pi.Product)
            .Where(c => c.TokenId == token)
            .FirstOrDefaultAsync();

        //Если totalAmount 0 или если корзина не найдена возвращаем ошибку
        if (cart == null || cart.TotalAmount == 0) throw new Exception(); //TODO создать кастомную ошибку

        var cartItemDtos = cart.CartItems.Select(ci => new CartItemDto
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
                    Name = ci.ProductItem.Product.Name
                }
            },
            Ingredients = ci.Ingredients.Select(i => new IngredientDto
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price
            }).ToList(),
            Quantity = ci.Quantity
        }).ToList();

        // создаем заказ
        var order = new Order
        {
            Status = OrderStatus.PENDING,
            TotalAmountCount = cart.TotalAmount,
            FullName = orderDTO.firstName + " " + orderDTO.lastName,
            Address = orderDTO.address,
            Email = orderDTO.email,
            Phone = orderDTO.phone,
            Comment = orderDTO.comment,
            CreatedAt = DateTime.Now,
            ProductList = JsonConvert.SerializeObject(cartItemDtos)
        };

        //удаляем все cartItem из корзины и обнуляем ее стоимость
        cart.CartItems.RemoveAll(i => true);
        cart.TotalAmount = 0;
        await _context.Orders.AddAsync(order);

        await _context.SaveChangesAsync();

        //создаем url для оплаты, используя Юмани        
        PaymentData paymentData = await new PaymentService().CreatePayment($"Заказ №{cart.Id}", order.Id.ToString(), order.TotalAmountCount);

        if (paymentData == null) throw new Exception();

        order.PaymentId = paymentData.id;

        await _context.SaveChangesAsync();

        string paymentUrl = paymentData.confirmation.confirmation_url;

        //отправляем письмо на почту о необходимости оплаты
        var mailSender = new MailSender(_resend);
        await mailSender.SendMailPaymentAsync(orderDTO.email, cart.Id.ToString(), order.TotalAmountCount, paymentUrl);

        return paymentUrl;
    }

    public async Task PaymentCallbackHandle(PaymentObject paymentObject)
    {
        //найти заказ по orderId, который прийдет в metadata
        var order = await _context.Orders
            .Where(c => c.Id == Convert.ToInt32(paymentObject.metadata.order_id))
            .FirstOrDefaultAsync();

        //и обновить его статус
        var isCucceeded = paymentObject.status == "succeeded";
        order.Status = isCucceeded ? OrderStatus.SUCCEEDED : OrderStatus.CANCELLED;
        await _context.SaveChangesAsync();

        //достать из order список товаров и email заказчика для отправки сообщения на почту
        var email = order.Email;
        var items = JsonConvert.DeserializeObject<List<ProductItemForLetter>>(order.ProductList);
        var mailSender = new MailSender(_resend);
        // отправляем письмо
        if (isCucceeded)
        {
            await mailSender.SendMailSuccessAsync(email, items, order.Id);
        }
        else
        {
            //TODO письмо о неуспешной оплате
        }
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

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task InsertUserAsync(UserVM user)
    {
        User userDB = new User
        {
            FullName = user.fullName,
            Email = user.email,
            Login = user.login,
            Password = user.password,
            Role = (UserRole)user.role
        };
        await _context.Users.AddAsync(userDB);
    }

    public async Task<List<User>> DeleteUserAsync(int userId)
    {
        var userFromDb = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);
        _context.Users.Remove(userFromDb);
        await _context.SaveChangesAsync();
        return await _context.Users.ToListAsync();
    }

    public async Task<List<User>> UpdateUserAsync(int userId, UserVM user)
    {
        User userFromDb = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);
        userFromDb.FullName = user.fullName;
        userFromDb.Email = user.email;
        userFromDb.Login = user.login;
        userFromDb.Password = user.password;
        userFromDb.Role = (UserRole)user.role;

        _context.Users.Update(userFromDb);
        await _context.SaveChangesAsync();
        return await _context.Users.ToListAsync();
    }

    public async Task InsertCategoryAsync(CategoryDto category)
    {
        Category categoryDB = new Category
        {
            Name = category.Name
        };
        await _context.Categories.AddAsync(categoryDB);
    }


}
