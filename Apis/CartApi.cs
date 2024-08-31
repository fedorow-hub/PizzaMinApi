
public class CartApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/cart", async (HttpContext context, IPizzaRepository repository) =>
            {
                string token = context.Request.Cookies["cartToken"] ?? "";
                if (token == "")
                {
                    return Results.Ok(new CartDto { TotalAmount = 0, CartItems = [] });
                }
                return Results.Ok(await repository.FindOrCreateCartAsync(token));
            })
            .Produces<Cart>(StatusCodes.Status200OK)
            .WithName("GetCart")
            .WithTags("Getters");

        app.MapPatch("/cart/{id}", async ([FromBody] QuantityRequest request, int id, HttpContext context, IPizzaRepository repository) =>
            {
                string token = context.Request.Cookies["cartToken"] ?? "";

                if (token == "")
                {
                    //обработать ситуацию, когда не передан токен, обязательный для обновления кол-ва товаров в корзине
                }
                var cart = await repository.PatchCartItemAsync(token, id, request.Quantity);
                await repository.SaveAsync();
                return Results.Ok(cart);
            })
            .Accepts<QuantityRequest>("application/json")
            .Produces<CartDto>(StatusCodes.Status200OK)
            .WithName("PatchCart")
            .WithTags("Updators");

        app.MapDelete("/cart/{id}", async (int id, HttpContext context, IPizzaRepository repository) =>
            {
                string token = context.Request.Cookies["cartToken"] ?? "";

                if (token == "")
                {
                    //обработать ситуацию, когда не передан токен, обязательный для обновления кол-ва товаров в корзине
                }
                var cart = await repository.DeleteCartItemAsync(token, id);
                await repository.SaveAsync();
                return Results.Ok(cart);
            })
            .Produces<CartDto>(StatusCodes.Status200OK)
            .WithName("DeleteCartItem")
            .WithTags("Deletors");

        app.MapPost("/cart", async (CreateCartItemValues cartItemValues, HttpContext context, IPizzaRepository repository) =>
            {
                string token = context.Request.Cookies["cartToken"] ?? "";

                if (token == "")
                {
                    token = Guid.NewGuid().ToString();
                }
                var cart = await repository.FindOrCreateCartAsync(token);

                try
                {
                    cart = await repository.FindOrCreateCartItem(token, cartItemValues, cart.Id);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

                await repository.SaveAsync();
                context.Response.Cookies.Append("cartToken", token, new CookieOptions
                {
                    HttpOnly = true, // Опция для безопасности, чтобы cookie не было доступно через JavaScript
                    //Secure = false, // Убедитесь, что cookie передается только по HTTPS
                    SameSite = SameSiteMode.Unspecified, // Опция для предотвращения CSRF атак
                    Expires = DateTimeOffset.UtcNow.AddDays(30) // Установите срок действия cookie
                });
                return Results.Ok(cart);
            })
            .Produces<CartDto>(StatusCodes.Status200OK)
            .WithName("SetCart")
            .WithTags("Setters");
    }
}