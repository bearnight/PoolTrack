using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace PoolTrack;

public partial class TestEntryPage : ContentPage
{
    public TestEntryPage()
    {
        InitializeComponent();
        BindingContext = new TestEntryViewModel(pHEntry, chlorineEntry, chlorineFreeEntry, alkalinityEntry, calciumEntry);
    }
}

public class TestEntryViewModel
{
    private readonly Entry _ph;
    private readonly Entry _chlorine;
    private readonly Entry _chlorineFree;
    private readonly Entry _alkalinity;
    private readonly Entry _calcium;

    public ICommand SaveTestCommand { get; }

    public TestEntryViewModel(Entry ph, Entry chlorine, Entry chlorineFree, Entry alkalinity, Entry calcium)
    {
        _ph = ph;
        _chlorine = chlorine;
        _chlorineFree = chlorineFree;
        _alkalinity = alkalinity;
        _calcium = calcium;

        SaveTestCommand = new AsyncRelayCommand(SaveAsync);
    }

    private async Task SaveAsync()
    {
        var pool = PoolStore.Current.ActivePool;
        if (pool == null)
        {
            await Shell.Current.DisplayAlert("No Pool", "Add a pool before saving a test.", "OK");
            return;
        }

        var details = string.Join(", ",
            new[]
            {
                $"pH: {_ph.Text}",
                $"Chlorine: {_chlorine.Text} ppm",
                $"Free Chlorine: {_chlorineFree.Text} ppm",
                $"Alkalinity: {_alkalinity.Text} ppm",
                $"Calcium: {_calcium.Text} ppm"
            });

        PoolStore.Current.AddResult(pool.Id, new TestResult { Details = details });

        await Shell.Current.DisplayAlert("Success", $"Test saved to {pool.Name}!", "OK");
        await Shell.Current.GoToAsync("..");
    }
}
