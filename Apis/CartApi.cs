
public class CartApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/cart", async (HttpContext context, IPizzaRepository repository) =>
            {
                string token = context.Request.Cookies["cartToken"] ?? "1111";
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
                string token = context.Request.Cookies["cartToken"] ?? "1111";

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
                string token = context.Request.Cookies["cartToken"] ?? "1111";

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
                string token = context.Request.Cookies["cartToken"] ?? "1111";

                if (token == "")
                {
                    token = Guid.NewGuid().ToString();
                }
                var cart = await repository.FindOrCreateCartAsync(token);

                var findCartItem = await repository.FindCartItem(token, cartItemValues, cart.Id);
                await repository.SaveAsync();
                return Results.Ok(cart); //TODO вшить в ответ cookie cartToken={token}
            })
            .Produces<CartDto>(StatusCodes.Status200OK)
            .WithName("SetCart")
            .WithTags("Setters");
    }
}