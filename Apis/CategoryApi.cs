public class CategoryApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/categories", async (IPizzaRepository repository,
                                    [FromQuery] double minPrice,
                                    [FromQuery] double maxPrice,
                                    [FromQuery] int[]? sizes,
                                    [FromQuery] int[]? pizzaTypes,
                                    [FromQuery] int[]? ingredients
                                    ) =>
            Results.Ok(await repository.GetCategoresAsync(minPrice, maxPrice, sizes, pizzaTypes, ingredients)))
                .Produces<List<Category>>(StatusCodes.Status200OK)
                .WithName("GetAllCategories")
                .WithTags("Getters");
    }
}