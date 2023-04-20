namespace TTX.Services;

public interface IRuntimeConfig
{
    string ServerPath { get; }
    string ProfileRoot { get; }
    string ProfileFilename { get; }
}