public class Story
{

    public int Id { get; set; }
    public string PreviewImageUrl { get; set; } = string.Empty;

    public List<StoryItem> StoryItems { get; set; }

    public DateTime CreatedAt { get; set; }

}

public class StoryItem
{
    public int Id { get; set; }

    public int StoryId { get; set; }

    public string SourceUrl { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

}