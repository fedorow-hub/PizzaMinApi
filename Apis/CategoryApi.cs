public class CategoryApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/categories/filter", async (IPizzaRepository repository,
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

        app.MapPost("/categories", async ([FromBody] CategoryDto category, IPizzaRepository repository) =>
            {
                await repository.InsertCategoryAsync(category);
                await repository.SaveAsync();
                return Results.Created($"/categories/{category.Id}", category);
            })
            .Accepts<CategoryDto>("application/json")
            .Produces<CategoryDto>(StatusCodes.Status201Created)
            .WithName("CreateCategory")
            .WithTags("Creators");

        app.MapGet("/categories", async (IPizzaRepository repository) =>
            {
                return Results.Ok(await repository.GetCategoresAsync());
            })
            .Produces<List<Category>>(StatusCodes.Status200OK)
            .WithName("GetCategories")
            .WithTags("Getters");

        app.MapDelete("/categories/{id}", async (int id, IPizzaRepository repository) =>
            {
                var category = await repository.DeleteCategoryAsync(id);
                await repository.SaveAsync();
                return Results.Ok(category);
            })
            .Produces<List<Category>>(StatusCodes.Status200OK)
            .WithName("DeleteCategory")
            .WithTags("Deletors");

        app.MapPut("/categories/{id}", async (int id, [FromBody] CategoryDto category, IPizzaRepository repository) =>
            {
                var categories = await repository.UpdateCategoryAsync(id, category);
                return Results.Ok(categories);
            })
            .Accepts<CategoryDto>("application/json")
            .Produces<List<User>>(StatusCodes.Status200OK)
            .WithName("UpdateCategory")
            .WithTags("Updators");
    }
}