using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Client.Services.ApiConnector;
using TTX.Client.Services.SessionClient;
using TTX.Client.ViewHandles;
using TTX.Client.ViewLogic;

namespace TTX.Client;

public static class SessionContext
{
    // Component : Cancellation Token

    private static readonly CancellationTokenSource CTS = new();

    public static CancellationToken CancellationToken => CTS.Token;

    // Action : LoginFactory

    private static Func<ILoginView>? _loginFactoryMethod = null;

    private static Func<ILoginView>? LoginFactoryMethod
        => _loginFactoryMethod ?? throw new NullReferenceException($"{nameof(LoginFactoryMethod)} should not be null when building the client.");

    public static void SetLoginViewActivator(Func<ILoginView> func)
        => _loginFactoryMethod = func;

    // Action : MainFactory

    private static Func<IMainView>? _mainFactoryMethod = null;

    private static Func<IMainView> MainFactoryMethod
        => _mainFactoryMethod ?? throw new NullReferenceException($"{nameof(MainFactoryMethod)} should not be null when building the client.");

    public static void SetMainViewActivator(Func<IMainView> func)
        => _mainFactoryMethod = func;

    // Action : OnExit

    private static Action? _onExit = null;

    internal static Action OnExit
        => _onExit ?? throw new NullReferenceException($"{nameof(OnExit)} should not be null when building the client.");

    public static void SetExitAction(Action exitAction)
        => _onExit = exitAction;

    // Component : SyncContext

    private static SynchronizationContext? _syncContext = null;

    internal static SynchronizationContext SyncContext
        => _syncContext ?? throw new NullReferenceException($"{nameof(SyncContext)} should not be null when building the client.");

    public static void SetSyncContext(SynchronizationContext syncContext)
        => _syncContext = syncContext;

    // Component : Session Client

    private static ApiConnectionService? _apiConnectionService = null;

    internal static ApiConnectionService ApiConnectionService
        => _apiConnectionService ?? throw new NullReferenceException($"Uninitialized {nameof(ApiConnectionService)}");

    // Components : Api Connection

    private static SessionClientService? _sessionClientService = null;

    internal static SessionClientService SessionClientService
        => _sessionClientService ?? throw new NullReferenceException($"Uninitialized {nameof(SessionClientService)}");

    // Component : Logger Logic

    private static readonly LoggerLogic _loggerLogic = new();

    public static LoggerLogic LoggerLogic => _loggerLogic;

    // Windows

    private static IMainView? MainView { get; set; } = null;

    // Internal

    private static void DoLogin()
    {
        ILoginView login = LoginFactoryMethod?.Invoke() ?? throw new NullReferenceException("No Login View");
        if (login.ShowModal() != 1)
        {
            _onExit?.Invoke();
            return;
        }

        // Recieve login data here.
    }

    private static void ShowMain()
    {
        if (MainView != null)
            return;

        MainView = MainFactoryMethod.Invoke();
        MainView.ShowView();
    }

    // Public API

    public static void Build()
    {
        _sessionClientService = new();
        _apiConnectionService = new(SyncContext, SessionClientService);
    }

    public static void Start()
    {
        DoLogin();

        ShowMain();
    }

    internal static void QueryClose(object? sender)
        => OnExit.Invoke();
}