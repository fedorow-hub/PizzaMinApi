public class ProfileApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/profile", [AllowAnonymous] async (HttpContext context, IUserRepository userRepository) =>
        {
            /* UserModel userModel = new()
            {
                Login = context.Request.Query["username"],
                Password = context.Request.Query["password"]
            };
            var userDto = userRepository.GetUser(userModel);
            if (userDto == null) return Results.Unauthorized();
            var token = tokenService.BuildToken(app.Configuration["Jwt:Key"],
                app.Configuration["Jwt:Issuer"], userDto);

            return Results.Ok(token); */

        });
    }
}