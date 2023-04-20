namespace TTX.Services;

public interface IServiceProfile
{
    void Initialize();

    void Initialize(IRuntimeConfig runtimeConfig);
}