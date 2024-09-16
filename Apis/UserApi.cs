public class UserApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/users", async (IPizzaRepository repository) =>
        Results.Ok(await repository.GetAllUsersAsync()))
            .Produces<List<User>>(StatusCodes.Status200OK)
            .WithName("GetAllUsers")
            .WithTags("Getters");

        app.MapPost("/users", async ([FromBody] UserVM user, IPizzaRepository repository) =>
            {
                await repository.InsertUserAsync(user);
                await repository.SaveAsync();
                return Results.Created($"/users/{user.Id}", user);
            })
            .Accepts<UserVM>("application/json")
            .Produces<UserVM>(StatusCodes.Status201Created)
            .WithName("CreateUser")
            .WithTags("Creators");
    }
}