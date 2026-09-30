using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PoolTrack;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();
        _viewModel = new MainPageViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Reload();
    }

    private async void OnManagePoolsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PoolsPage));
    }
}

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _activePoolName = "";

    [ObservableProperty]
    private string _latestTest = "No tests yet";

    public ICommand NavigateToTestEntry { get; }

    public MainPageViewModel()
    {
        NavigateToTestEntry = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(nameof(TestEntryPage)));
    }

    public void Reload()
    {
        var pool = PoolStore.Current.ActivePool;
        ActivePoolName = pool?.Name ?? "No pool";
        var latest = pool != null
            ? PoolStore.Current.GetResults(pool.Id).FirstOrDefault()
            : null;
        LatestTest = latest?.Details ?? "No tests yet";
    }
}
