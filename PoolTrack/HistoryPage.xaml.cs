using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace PoolTrack;

public partial class HistoryPage : ContentPage
{
    private readonly HistoryViewModel _viewModel;

    public HistoryPage()
    {
        InitializeComponent();
        _viewModel = new HistoryViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Reload();
    }
}

public class HistoryViewModel
{
    public ObservableCollection<TestResult> TestResults { get; } = new();

    public void Reload()
    {
        TestResults.Clear();
        foreach (var result in PoolStore.Current.GetResults(PoolStore.Current.ActivePoolId))
        {
            TestResults.Add(result);
        }
    }
}
