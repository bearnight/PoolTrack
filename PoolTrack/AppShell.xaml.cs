namespace PoolTrack;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
   
        // Register routes for navigation`
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(TestEntryPage), typeof(TestEntryPage));
        Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
    }
}