﻿using System;
using System.IO;
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

        // TODO
        // Splash wrap the following code
        // Task.Run the whole thing
        // Use dispatcher to control the splash

        // Prepare

        string baseDirectory = Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().Location) ??
            throw new NullReferenceException("Cannot acquire base path.");

        // VLC

        // Options

        ClientOptions options = new(SynchronizationContext.Current ?? new())
        {
            ShutdownAction = Shutdown,
            LoginViewFactoryMethod = () => new LoginWindow(),
            MainViewFactoryMethod = () => new MainWindow(),
            TagSelectorViewFactoryMethod = () => new TagSelectorWindow(),
            BaseDirectory = baseDirectory,
        };

        // Start

        ClientContextHost.BuildServices(options);
        ClientContextHost.Start();
    }
}