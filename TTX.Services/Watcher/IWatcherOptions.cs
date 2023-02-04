namespace TTX.Services.Watcher;

public interface IWatcherOptions : IServiceOptions
{
    string ServerRoot { get; set; }
    string AssetsPath { get; set; }
    string[] Whitelist { get; set; }
    string[] Blacklist { get; set; }
    string[] FinalAdds { get; set; }
}