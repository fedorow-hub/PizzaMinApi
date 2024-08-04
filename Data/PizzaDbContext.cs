public class PizzaDb : DbContext
{
    public PizzaDb(DbContextOptions<PizzaDb> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Ingredient> Ingredients => Set<Ingredient>();

    public DbSet<User> Users => Set<User>();
}