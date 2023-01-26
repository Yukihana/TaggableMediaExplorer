using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using TTX.Client.ViewData;

namespace TTX.Client.ViewLogic;

public partial class LoggerLogic : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<LogEntry> _logEntries = new();

    internal void LogEntry(string message, object? sender, object? data, Exception? exception)
    { }
}