namespace TTX.Services.TagsIndexer;

public interface ITagsIndexerOptions : IServiceOptions
{
    string TagsIndexerSID { get; set; }
}