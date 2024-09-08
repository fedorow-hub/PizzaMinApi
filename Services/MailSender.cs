using Resend;

public class MailSender
{
    private readonly IResend _resend;
    public MailSender(IResend resend)
    {
        _resend = resend;
    }

    public async Task SendMailAsync(string emailTo, string orderId, double amount, string url)
    {
        var message = new EmailMessage();
        message.From = "onboarding@resend.dev";
        message.To.Add(emailTo);
        message.Subject = $"Next Pizza / Оплатите заказ #{orderId}";
        message.HtmlBody = $"<h1>Заказ #{orderId}</h1><p>Оплатите заказ на сумму {amount}. Перейдите <a href=\"{url}\">по ссылке</a> для оплаты заказа.</p>";

        try
        {
            await _resend.EmailSendAsync(message);
        }
        catch (HttpRequestException ex)
        {
            //TODO реализовать логгирование
            Console.WriteLine($"Ошибка при отправке запроса: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}