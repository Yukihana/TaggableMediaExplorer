namespace TTX.Services.Integrity;

public class IntegrityOptions : IServiceOptions
{
    public int CrumbsCount { get; set; } = 16;
    public int CrumbsConcurrency { get; set; } = 4;
    public int ReadBufferSize { get; set; } = 10485760;

    public void Initialize()
    { }
}