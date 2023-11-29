using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace MauiApp2.ViewModel;

public partial class MainViewModel : ObservableObject
{
    IConnectivity connectivity;
    private readonly ILogger<MainViewModel> _logger;

    public MainViewModel(IConnectivity connectivity, ILogger<MainViewModel> logger)
    {
        Items = new ObservableCollection<string>();
        this.connectivity = connectivity;
        _logger = logger;
    }

    [ObservableProperty]
    ObservableCollection<string> items;

    [ObservableProperty]
    string text;

    [RelayCommand]
    async Task Add()
    {
        if (string.IsNullOrWhiteSpace(Text))
            return;

        if(connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
            return;
        }

        Items.Add(Text);
        // add our item
        Text = string.Empty;
    }

    [RelayCommand]
    void Delete(string s)
    {
        if(Items.Contains(s))
        {
            Items.Remove(s);
        }
    }

    [RelayCommand]
    async Task Tap(string s)
    {
        _logger.LogTrace("Trace");
        _logger.LogDebug("Debug");
        _logger.LogInformation("Info");
        _logger.LogWarning("Warn");
        _logger.LogError("Error");
        _logger.LogCritical("Critical");
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}");
    }

}
