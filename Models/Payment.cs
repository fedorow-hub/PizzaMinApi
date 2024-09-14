using Newtonsoft.Json;

public class PaymentData
{
    public string id { get; set; }
    public string status { get; set; }
    public string desctiption { get; set; }
    public Recipient recipient { get; set; }
    public string created_at { get; set; }
    public Confirmation confirmation { get; set; }
    public bool test { get; set; }
    public bool paid { get; set; }
    public bool refundable { get; set; }
    public Metadata metadata { get; set; }
}

public class Recipient
{
    public string account_id { get; set; }
    public string getaway_id { get; set; }
}

public class Confirmation
{
    public string type { get; set; }
    public string confirmation_url { get; set; }
}

public class Metadata
{
    public string order_id { get; set; }
}

public class PaymentCallbackData
{
    public string type { get; set; }
    public string Event { get; set; }
    public PaymentObject Object { get; set; }

}

public class PaymentObject
{
    public string Id { get; set; }
    public string status { get; set; }
    public Amount amount { get; set; }
    public Amount income_amount { get; set; }
    public string description { get; set; }
    public Recipient recipient { get; set; }
    public PaymentMethod payment_method { get; set; }
    public string captured_at { get; set; }
    public string created_at { get; set; }
    public bool test { get; set; }
    public Amount refunded_amount { get; set; }
    public bool paid { get; set; }
    public bool refundable { get; set; }
    public Metadata metadata { get; set; }
    public AuthorizationDetails authorization_details { get; set; }
}

public class Amount
{
    public string value { get; set; }
    public string currency { get; set; }
}

public class PaymentMethod
{
    public string type { get; set; }
    public string id { get; set; }
    public bool saved { get; set; }
    public string title { get; set; }
}

public class AuthorizationDetails
{
    public string rrn { get; set; }
    public string auth_code { get; set; }
}