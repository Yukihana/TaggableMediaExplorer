namespace TTX.Services.Integrity;

public class IntegrityOptions : IServiceOptions
{
    public int CrumbsCount { get; set; } = 16;

    public void Initialize()
    { }
}