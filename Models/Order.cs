public enum OrderStatus
{
    PENDING,
    SUCCEEDED,
    CANCELLED,
}

public class Order
{
    public int Id { get; set; }
    public User? User { get; set; } // TODO уточнить необходимость для связи один ко многим
    public int? UserId { get; set; }
    public OrderStatus Status { get; set; }
    public int TotalAmountCount { get; set; }
    public string? PaymentId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    //список товаров в корзине
    public string ProductList { get; set; }
}