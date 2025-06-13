using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace PoolTrack;

public partial class TestEntryPage : ContentPage
{
    public TestEntryPage()
    {
        InitializeComponent();
        BindingContext = new TestEntryViewModel();
    }
}

public class TestEntryViewModel
{
    public ICommand SaveTestCommand { get; }

    public TestEntryViewModel()
    {
        SaveTestCommand = new AsyncRelayCommand(async () =>
        {
            // Save logic (to database or API)
            await Shell.Current.DisplayAlert("Success", "Test saved!", "OK");
            await Shell.Current.GoToAsync(".."); // Navigate back
        });
    }
}
