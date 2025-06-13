using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace PoolTrack;

public partial class MainPage : ContentPage
{
    private int count = 0;

    public MainPage(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();
        //BindingContext = new MainPageViewModel();
    }

    public class MainPageViewModel
    {
        public ICommand NavigateToTestEntry { get; }

        public MainPageViewModel()
        {
            NavigateToTestEntry = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(nameof(TestEntryPage)));
        }

    }
}