using System.Net.Http.Headers;
using Newtonsoft.Json;

public class PaymentService
{
  public async Task<PaymentData> CreatePayment(string descriptionValue, string orderId, double amountValue)
  {
    var paymentData = new
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

    var json = JsonConvert.SerializeObject(paymentData);

    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.yookassa.ru/v3/payments")
    {
      Content = content
    };

    var username = "454507";
    var password = "test_CP532nflqaaOsKs41awJBi2WKVmE8svSGbKWzEWOmxg";

    var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);
    request.Headers.Add("Idempotence-Key", Guid.NewGuid().ToString());

    using (HttpClient client = new HttpClient())
    {
      var response = await client.SendAsync(request);

      if (response.IsSuccessStatusCode)
      {
        var responseData = await response.Content.ReadAsStringAsync();
        var payData = JsonConvert.DeserializeObject<PaymentData>(responseData);
        return payData;
      }
      else
      {
        var errorData = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error: {response.StatusCode}");
        Console.WriteLine($"Error details: {errorData}");
        return null;
      }
    }
  }
}
