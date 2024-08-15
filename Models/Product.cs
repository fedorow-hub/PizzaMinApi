public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
    public List<ProductItem> Items { get; set; }
    public List<Ingredient> Ingredients { get; set; }
}

// вариации продуктов
public class ProductItem
{
    public int Id { get; set; }
    public double Price { get; set; }
    public int? Size { get; set; }
    public int? PizzaType { get; set; }
    public List<CartItem> CartItems { get; set; }
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public List<CartItem> CartItems { get; set; }
    public List<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Product> Products { get; set; }
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProductDto> Products { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public List<IngredientDto> Ingredients { get; set; }
    public List<ProductItemDto> ProductItems { get; set; }
}

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    //public List<CartItem> CartItems { get; set; }
}

public class ProductItemDto
{
    public int Id { get; set; }
    public double Price { get; set; }
    public int? Size { get; set; }
    public int? PizzaType { get; set; }
}



