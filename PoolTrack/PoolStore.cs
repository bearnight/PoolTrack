using System.Text.Json;

namespace PoolTrack;

public class PoolStore
{
    private static readonly Lazy<PoolStore> _current = new(() => new PoolStore());
    public static PoolStore Current => _current.Value;

    private readonly string _filePath;
    private readonly List<Pool> _pools = new();
    private readonly List<TestResult> _results = new();
    private string _activePoolId = "";

    private PoolStore()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, "pooltrack.json");
        Load();
    }

    public IReadOnlyList<Pool> Pools => _pools;
    public string ActivePoolId => _activePoolId;

    public Pool? ActivePool => _pools.FirstOrDefault(p => p.Id == _activePoolId);

    public void SetActivePool(string poolId)
    {
        if (_pools.Any(p => p.Id == poolId))
        {
            _activePoolId = poolId;
            Save();
        }
    }

    public Pool AddPool(string name)
    {
        var pool = new Pool { Name = name.Trim() };
        _pools.Add(pool);
        if (string.IsNullOrEmpty(_activePoolId))
        {
            _activePoolId = pool.Id;
        }
        Save();
        return pool;
    }

    public void RenamePool(string poolId, string name)
    {
        var pool = _pools.FirstOrDefault(p => p.Id == poolId);
        if (pool != null)
        {
            pool.Name = name.Trim();
            Save();
        }
    }

    public void DeletePool(string poolId)
    {
        _pools.RemoveAll(p => p.Id == poolId);
        _results.RemoveAll(r => r.PoolId == poolId);
        if (_activePoolId == poolId)
        {
            _activePoolId = _pools.FirstOrDefault()?.Id ?? "";
        }
        if (_pools.Count == 0)
        {
            SeedDefault();
        }
        Save();
    }

    public IReadOnlyList<TestResult> GetResults(string poolId) =>
        _results.Where(r => r.PoolId == poolId).OrderByDescending(r => r.Timestamp).ToList();

    public void AddResult(string poolId, TestResult result)
    {
        result.PoolId = poolId;
        _results.Add(result);
        Save();
    }

    private void Load()
    {
        if (!File.Exists(_filePath))
        {
            SeedDefault();
            Save();
            return;
        }

        try
        {
            var data = JsonSerializer.Deserialize<StoreData>(File.ReadAllText(_filePath));
            if (data == null)
            {
                SeedDefault();
                Save();
                return;
            }

            _pools.Clear();
            _pools.AddRange(data.Pools ?? new List<Pool>());
            _results.Clear();
            _results.AddRange(data.Results ?? new List<TestResult>());
            _activePoolId = data.ActivePoolId ?? "";

            if (_pools.Count == 0)
            {
                SeedDefault();
                Save();
            }
        }
        catch (JsonException)
        {
            SeedDefault();
            Save();
        }
    }

    private void SeedDefault()
    {
        var pool = new Pool { Name = "My Pool" };
        _pools.Add(pool);
        _activePoolId = pool.Id;
        _results.Add(new TestResult
        {
            PoolId = pool.Id,
            Timestamp = new DateTime(2025, 3, 30),
            Details = "pH: 7.5, Chlorine: 2.2 ppm",
            Status = "OK"
        });
        _results.Add(new TestResult
        {
            PoolId = pool.Id,
            Timestamp = new DateTime(2025, 3, 25),
            Details = "pH: 7.3, Chlorine: 1.8 ppm",
            Status = "OK"
        });
    }

    private void Save()
    {
        var data = new StoreData
        {
            ActivePoolId = _activePoolId,
            Pools = _pools,
            Results = _results
        };

        var tempPath = _filePath + ".tmp";
        File.WriteAllText(tempPath, JsonSerializer.Serialize(data));
        File.Move(tempPath, _filePath, overwrite: true);
    }

    private class StoreData
    {
        public string? ActivePoolId { get; set; }
        public List<Pool>? Pools { get; set; }
        public List<TestResult>? Results { get; set; }
    }
}
