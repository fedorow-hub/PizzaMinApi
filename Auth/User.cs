public record UserDTO(string UserName, string Password);

public record UserModel
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}


public class User : IdentityUser
{
    public User(string name)
        : base(name)
    {

    }
    //public int Id { get; set; }
    public string FullName { get; set; }
    //public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public List<Order> Orders { get; set; }
    public List<CartItem> CartItems { get; set; }
    public Cart Cart { get; set; }
    public VerificationCode? VerificationCode { get; set; }
    public string? Provider { get; set; }
    public string? ProviderId { get; set; }
    public DateTime Verified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum UserRole
{
    USER,
    ADMIN
}

public class VerificationCode
{
    public int Id { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }

    public string Code { get; set; }

    public DateTime CreatedAt { get; set; }
}