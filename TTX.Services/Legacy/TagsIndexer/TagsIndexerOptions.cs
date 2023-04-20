using System;

namespace TTX.Services.Legacy.TagsIndexer;

public class TagsIndexerOptions : ITagsIndexerOptions
{
    public void Initialize()
    { }

    public void Initialize(IRuntimeConfig runtimeConfig)
        => throw new NotImplementedException();
}