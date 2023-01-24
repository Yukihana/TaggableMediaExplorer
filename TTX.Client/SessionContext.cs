using System;
using System.Threading;
using TTX.Client.QueryApi;
using TTX.Client.ViewHandles;

namespace TTX.Client;

public static class SessionContext
{
    // Actions

    public static Action? OnExit { get; set; } = null;
    public static Func<ILoginView>? CreateLogin { get; set; } = null;
    public static Func<IMainView>? CreateMain { get; set; } = null;

    // Windows

    private static IMainView? LoginView { get; set; } = null;
    private static IMainView? MainView { get; set; } = null;

    // Components : Internal

    private static SynchronizationContext? _syncContext = null;
    private static DataLoader? _dataLoader = null;

    // Components : Exposed

    public static SynchronizationContext SyncContext
    {
        get => _syncContext ?? throw new NullReferenceException($"Missing {nameof(SynchronizationContext)} {nameof(SyncContext)}");
        set => _syncContext = value;
    }

    internal static DataLoader DataLoader => _dataLoader ?? throw new NullReferenceException($"Uninitialized DataLoader");

    // Component

    internal static readonly CancellationTokenSource CTS = new();

    // Build

    public static void Build()
    {
        _dataLoader = new(SyncContext);
    }

    // Control

    public static void Start()
    {
        ILoginView login = CreateLogin?.Invoke() ?? throw new NullReferenceException("No Login View");
        if (login.ShowModal() != 1)
        {
            OnExit?.Invoke();
            return;
        }

        // Recieve login data here.

        // if logged in
        MainView = CreateMain?.Invoke() ?? throw new NullReferenceException("No Main View");
        MainView.ShowView();
        // if not logged in, exit (cancelled login)
    }
}