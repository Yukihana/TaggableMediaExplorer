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
        SessionContext.SetSyncContext(
            SynchronizationContext.Current ??
            throw new NullReferenceException(
                $"Require a {nameof(SynchronizationContext)}."));
        SessionContext.SetLoginViewActivator(() => new LoginWindow());
        SessionContext.SetMainViewActivator(() => new MainWindow());
        SessionContext.SetExitAction(Shutdown);

        SessionContext.Build();

        // Start
        SessionContext.Start();
    }
}