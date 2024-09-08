public class OrderApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapPost("/order", async (OrderDTO order, HttpContext context, IPizzaRepository repository) =>
        {
            string token = context.Request.Cookies["cartToken"] ?? "";

            if (token == "")
            {
                throw new Exception(); //TODO создать кастомное исключение
            }

            string urlForPayment = "";

            try
            {
                urlForPayment = await repository.CreateOrderAndCreatingPaymentURL(token, order);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return Results.Ok(urlForPayment);
        })
        .Produces<string>(StatusCodes.Status200OK)
        .WithName("CreateOrder")
        .WithTags("Setters");
    }
}