namespace TTX.Services.TagsIndexer;

public class TagsIndexerOptions : ITagsIndexerOptions
{
    public string TagsIndexerSID { get; set; } = "tix";

    public void Initialize()
    { }
}