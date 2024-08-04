//модель корзины
public class Cart
{
    public int Id { get; set; }

    public User? User { get; set; } // TODO уточнить необходимость для связи один ко многим

    // если пользован зарегистрирован, то корзину привязываем на уровне пользователя
    public int? UserId { get; set; }

    // корзина у незарегистрированного пользователя привязывается к 
    // сгенерированному токену (который генерится не при регистрации пользователя,
    // а при создании корзины при добавлении первого товара)
    public string TokenId { get; set; } = string.Empty;

    // общая стоимость товаров в корзине
    public int TotalAmount { get; set; }

    public List<CartItem> CartItems { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

// отдельные продукты, хранящиеся только в корзине, не являющиеся Product
public class CartItem
{
    public int Id { get; set; }

    public ProductItem ProductItem { get; set; } // TODO уточнить необходимость для связи один ко многим

    public int ProductItemId { get; set; }

    public Cart Cart { get; set; } // TODO уточнить необходимость для связи один ко многим

    public int CartId { get; set; }

    public List<Ingredient> Ingredients { get; set; }

    public int Quantity { get; set; }

    public int Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}