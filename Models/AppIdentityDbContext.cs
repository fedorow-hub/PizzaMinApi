public class AppIdentityDbContext : IdentityDbContext<User>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
    : base(options) { }
}