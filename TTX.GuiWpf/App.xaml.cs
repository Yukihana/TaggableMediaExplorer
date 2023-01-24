using System;
using System.Threading;
using System.Windows;
using TTX.Client;
using TTX.GuiWpf.Views;

namespace TTX.GuiWpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App() : base()
    {
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Build
        SessionContext.SyncContext = SynchronizationContext.Current
            ?? throw new NullReferenceException($"Require a {nameof(SynchronizationContext)}.");
        SessionContext.CreateLogin = () => new LoginWindow();
        SessionContext.CreateMain = () => new MainWindow();

        // Start
        SessionContext.Start();
        SessionContext.OnExit = Shutdown;
    }
}