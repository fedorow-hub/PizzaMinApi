using Resend;

public class MailSender
{
    private readonly IResend _resend;
    public MailSender(IResend resend)
    {
        _resend = resend;
    }

    /// <summary>
    /// Метод отправки сообщения на почту о необходимости оплаты со ссылкой на страницу оплаты
    /// </summary>
    /// <param name="emailTo">email</param>
    /// <param name="orderId">номер заказа</param>
    /// <param name="amount">сумма заказа</param>
    /// <param name="url">ссылка на страницу оплаты</param>
    /// <returns></returns>
    public async Task SendMailPaymentAsync(string emailTo, string orderId, double amount, string url)
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

    /// <summary>
    /// Метод отправки сообщения на почту об успешности оплаты с деталями заказа
    /// </summary>
    /// <param name="emailTo">email</param>
    /// <param name="items">заказанные продукты</param>
    /// <returns></returns>
    public async Task SendMailSuccessAsync(string emailTo, List<ProductItemForLetter> items, int orderId)
    {

        var message = new EmailMessage();
        message.From = "onboarding@resend.dev";
        message.To.Add(emailTo);
        message.Subject = $"Next Pizza / Заказ успешно оплачен";
        message.HtmlBody = @$"
            <div>
                <h1>Спасибо за покупку!</h1>
                <p>Ваш заказ #{orderId} оплачен. Список товаров:</p>
                <ul>
                    {items} 

                </ul>
            </div>

        "; //TODO Для каждого item вывести элемент списка с информацией о наименовании, стоимость умноженная на количество равно общая сумма

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