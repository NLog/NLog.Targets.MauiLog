using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace MauiApp2.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly IConnectivity connectivity;
    private readonly ILogger<MainViewModel> _logger;

    public MainViewModel(IConnectivity connectivity, ILogger<MainViewModel> logger)
    {
        Items = new ObservableCollection<string>();
        this.connectivity = connectivity;
        this._logger = logger;
    }

    [ObservableProperty]
    public partial ObservableCollection<string> Items { get; set; }

    [ObservableProperty]
    public partial string Text { get; set; }

    [RelayCommand]
    async Task Add()
    {
        if (string.IsNullOrWhiteSpace(Text))
            return;

        if(connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlertAsync("Uh Oh!", "No Internet", "OK");
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
        try
        {
            _logger.LogTrace("Trace - GoTo {Tap}", s);
            _logger.LogDebug("Debug - GoTo {Tap}", s);
            _logger.LogInformation("Info - GoTo {Tap}", s);
            _logger.LogWarning("Warn - GoTo {Tap}", s);
            _logger.LogError("Error - GoTo {Tap}", s);
            await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Critical - GoTo {Tap}", s);
            throw;
        }
    }

}
