using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace PoolTrack;

public partial class HistoryPage : ContentPage
{
    public HistoryPage()
    {
        InitializeComponent();
        BindingContext = new HistoryViewModel();
    }
}

public class HistoryViewModel
{
    public ObservableCollection<TestResult> TestResults { get; set; }

    public HistoryViewModel()
    {
        TestResults = new ObservableCollection<TestResult>
        {
            new TestResult { Date = "March 30, 2025", Details = "pH: 7.5, Chlorine: 2.2 ppm" },
            new TestResult { Date = "March 25, 2025", Details = "pH: 7.3, Chlorine: 1.8 ppm" }
        };
    }
}

public class TestResult
{
    public string Date { get; set; }
    public string Details { get; set; }
}