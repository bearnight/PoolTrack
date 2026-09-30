using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace PoolTrack;

public partial class PoolsPage : ContentPage
{
    public PoolsPage()
    {
        InitializeComponent();
        BindingContext = new PoolsViewModel();
    }
}

public class PoolsViewModel
{
    public ObservableCollection<PoolItemViewModel> Pools { get; } = new();

    public IAsyncRelayCommand AddPoolCommand { get; }

    public PoolsViewModel()
    {
        AddPoolCommand = new AsyncRelayCommand(AddPoolAsync);
        Reload();
    }

    private void Reload()
    {
        Pools.Clear();
        var activeId = PoolStore.Current.ActivePoolId;
        foreach (var pool in PoolStore.Current.Pools)
        {
            Pools.Add(new PoolItemViewModel(pool.Id, pool.Name, pool.Id == activeId, this));
        }
    }

    private async Task AddPoolAsync()
    {
        var name = await Shell.Current.DisplayPromptAsync("Add Pool", "Pool name", "Add", "Cancel");
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        PoolStore.Current.AddPool(name);
        Reload();
    }

    internal async Task ActivateAsync(string poolId)
    {
        PoolStore.Current.SetActivePool(poolId);
        Reload();
    }

    internal async Task RenameAsync(string poolId, string currentName)
    {
        var name = await Shell.Current.DisplayPromptAsync("Rename Pool", "New name", "Save", "Cancel", initialValue: currentName);
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        PoolStore.Current.RenamePool(poolId, name);
        Reload();
    }

    internal async Task DeleteAsync(string poolId, string name)
    {
        var confirmed = await Shell.Current.DisplayAlert(
            "Delete Pool",
            $"Delete {name}? Its test history will be removed.",
            "Delete",
            "Cancel");
        if (!confirmed)
        {
            return;
        }

        PoolStore.Current.DeletePool(poolId);
        Reload();
    }
}

public class PoolItemViewModel
{
    private readonly PoolsViewModel _owner;

    public string Id { get; }
    public string Name { get; }
    public bool IsActive { get; }

    public IAsyncRelayCommand ActivateCommand { get; }
    public IAsyncRelayCommand RenameCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }

    public PoolItemViewModel(string id, string name, bool isActive, PoolsViewModel owner)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        _owner = owner;

        ActivateCommand = new AsyncRelayCommand(() => _owner.ActivateAsync(Id));
        RenameCommand = new AsyncRelayCommand(() => _owner.RenameAsync(Id, Name));
        DeleteCommand = new AsyncRelayCommand(() => _owner.DeleteAsync(Id, Name));
    }
}
