
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
                return Results.Ok(await repository.GetCartAsync(token));
            })
            .Produces<Cart>(StatusCodes.Status200OK)
            .WithName("GetCart")
            .WithTags("Getters");

        app.MapPatch("/cart/{id}", async ([FromBody] QuantityRequest request, int id, HttpContext context, IPizzaRepository repository) =>
            {
                //здесь нужно получить следующие данные:
                string token = context.Request.Cookies["cartToken"] ?? "1111";

                if (token == "")
                {
                    //обработать ситуацию, когда не передан токен, обязательный для обновления кол-ва товаров в корзине
                }
                var cart = await repository.PatchCartAsync(token, id, request.Quantity);
                await repository.SaveAsync();
                return Results.Ok(cart);
            })
            .Accepts<Cart>("application/json")
            .Produces<Cart>(StatusCodes.Status204NoContent)
            .WithName("PatchCart")
            .WithTags("Updators");
    }
}