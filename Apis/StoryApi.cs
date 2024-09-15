public class StoryApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/stories", async (IPizzaRepository repository) =>
        Results.Ok(await repository.GetStoriesAsync()))
            .Produces<List<Story>>(StatusCodes.Status200OK)
            .WithName("GetAllStories")
            .WithTags("Getters");
    }
}