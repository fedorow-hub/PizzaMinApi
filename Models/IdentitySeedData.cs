public static class IdentitySeedData
{
    private const string adminUser = "Admin";
    private const string adminPassword = "Secret123$";
    public static async void EnsurePopulated(IApplicationBuilder app)
    {
        AppIdentityDbContext context = app.ApplicationServices
        .CreateScope().ServiceProvider
        .GetRequiredService<AppIdentityDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
        UserManager<User> userManager = app.ApplicationServices
        .CreateScope().ServiceProvider
        .GetRequiredService<UserManager<User>>();
        User user = await userManager.FindByNameAsync(adminUser);
        if (user == null)
        {
            user = new User("Admin");
            user.Email = "admin@example.com";
            user.PhoneNumber = "555-1234";
            await userManager.CreateAsync(user, adminPassword);
        }
    }
}