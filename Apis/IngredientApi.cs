public class IngredientApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/ingredients", async (IPizzaRepository repository) =>
            Results.Ok(await repository.GetIngredientsAsync()))
                .Produces<List<Ingredient>>(StatusCodes.Status200OK)
                .WithName("GetAllIngredients")
                .WithTags("Getters");
    }
}