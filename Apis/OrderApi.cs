using Newtonsoft.Json;

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


        app.MapPost("/paymentcallback", async (HttpRequest request, IPizzaRepository repository) =>
        {
            try
            {
                PaymentCallbackData payData = new PaymentCallbackData();

                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    string body = await reader.ReadToEndAsync();
                    // Process the request body as needed
                    payData = JsonConvert.DeserializeObject<PaymentCallbackData>(body);
                    await repository.PaymentCallbackHandle(payData.Object);
                }

                return Results.Ok("ok");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Results.Ok("ko");
            }

        })
        .Produces<string>(StatusCodes.Status200OK)
        .WithName("PaymentCallback")
        .WithTags("Setters");
    }



}