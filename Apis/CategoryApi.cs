public class CategoryApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/categories", async (IPizzaRepository repository) =>
            Results.Ok(await repository.GetCategoresAsync()))
                .Produces<List<Category>>(StatusCodes.Status200OK)
                .WithName("GetAllCategories")
                .WithTags("Getters");
    }
}