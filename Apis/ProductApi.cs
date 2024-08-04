public class ProductApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/products", async (IPizzaRepository repository) =>
            Results.Ok(await repository.GetProductsAsync()))
                .Produces<List<Product>>(StatusCodes.Status200OK)
                .WithName("GetAllProducts")
                .WithTags("Getters");

        app.MapGet("/products/{id}", async (int id, IPizzaRepository repository) =>
            await repository.GetProductAsync(id) is Product product
            ? Results.Ok(product)
            : Results.NotFound())
            .Produces<Product>(StatusCodes.Status200OK)
            .WithName("GetProduct")
            .WithTags("Getters");

        app.MapPost("/products", [Authorize] async ([FromBody] Product product, IPizzaRepository repository) =>
            {
                await repository.InsertProductAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/products/{product.Id}", product);
            })
            .Accepts<Product>("application/json")
            .Produces<Product>(StatusCodes.Status201Created)
            .WithName("CreateProduct")
            .WithTags("Creators");

        app.MapPut("/products", [Authorize] async ([FromBody] Product product, IPizzaRepository repository) =>
            {
                await repository.UpdateProductAsync(product);
                await repository.SaveAsync();
                return Results.NoContent();
            })
            .Accepts<Product>("application/json")
            .Produces<Product>(StatusCodes.Status204NoContent)
            .WithName("UpdateProduct")
            .WithTags("Updators");

        app.MapDelete("/products/{id}", [Authorize] async (int id, IPizzaRepository repository) =>
            {
                await repository.DeleteProductAsync(id);
                await repository.SaveAsync();
                return Results.NoContent();
            })
            .WithName("DeleteProduct")
            .WithTags("Deletors");
    }
}