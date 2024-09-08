public class PaymentService
{
    public async Task<string> CreatePayment(string descriptionValue, string orderId, double amountValue)
    {
        //string responce = "";

        var content = new
        {
            amount = new
            {
                value = amountValue,
                currency = "RUB"
            },
            capture = true,
            description = descriptionValue,
            metadata = new
            {
                order_id = orderId
            },
            confirmation = new
            {
                type = "redirect",
                return_url = "http://localhost:3000/"
            }
        };
        // создаем JsonContent
        JsonContent jsonContent = JsonContent.Create(content);

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());
            client.

            var responce = await client.PostAsync("https://api.yookassa.ru/v3/payments", jsonContent) as HttpResponseMessage;
            return responce.Content;
        }
        return responce;
    }
}

/* const { data } = await axios.post<PaymentData>(
    'https://api.yookassa.ru/v3/payments',
    {
      amount: {
        value: details.amount,
        currency: 'RUB',
      },
      capture: true,
      description: details.description,
      metadata: {
        order_id: details.orderId,s
      },
      confirmation: {
        type: 'redirect',
        return_url: 'http://localhost:3000/?paid',
      },
    },
    {
      auth: {
        username: process.env.YOOKASSA_API_KEY,
        password: '',
      },
      headers: {
        'Idempotence-Key': Math.random().toString(36).substring(7),
      },
    },
  ); */