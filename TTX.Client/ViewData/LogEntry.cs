using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace TTX.Client.ViewData;

public partial class LogEntry : ObservableObject
{
    [ObservableProperty]
    private TimeSpan _timeStamp = TimeSpan.Zero;

    [ObservableProperty]
    private Type? _senderType = null;

    [ObservableProperty]
    private string _message = string.Empty;

    [ObservableProperty]
    private object? _associatedData = null;

    [ObservableProperty]
    private Exception? _exception = null;
}