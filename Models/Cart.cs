//модель корзины
public class Cart
{
    public int Id { get; set; }

    // если пользован зарегистрирован, то корзину привязываем на уровне пользователя
    public User? User { get; set; }
    public int? UserId { get; set; }

    // корзина у незарегистрированного пользователя привязывается к 
    // сгенерированному токену (который генерится не при регистрации пользователя,
    // а при создании корзины при добавлении первого товара)
    public string TokenId { get; set; } = string.Empty;

    // общая стоимость товаров в корзине
    public double TotalAmount { get; set; }

    public List<CartItem> CartItems { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

// отдельные продукты, хранящиеся только в корзине, не являющиеся Product
public class CartItem
{
    public int Id { get; set; }

    public ProductItem ProductItem { get; set; }

    public int ProductItemId { get; set; }

    public Cart Cart { get; set; }

    public int CartId { get; set; }

    /*  public User User { get; set; }

     public int UserId { get; set; } */

    public List<Ingredient> Ingredients { get; set; }

    public int Quantity { get; set; }

    public int Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}


public class CartDto
{
    public int Id { get; set; }

    public string TokenId { get; set; } = string.Empty;

    public double TotalAmount { get; set; }

    public List<CartItemDto> CartItems { get; set; }
}

public class CartItemDto
{
    public int Id { get; set; }

    public ProductItemDto ProductItem { get; set; }

    public List<IngredientDto> Ingredients { get; set; }

    public int Quantity { get; set; }
}


public class QuantityRequest
{
    public int Quantity { get; set; }
}

public class CreateCartItemValues
{
    public int ProductItemId { get; set; }
    public List<int> Ingredients { get; set; }
}




