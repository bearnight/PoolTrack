namespace PoolTrack;

public class Pool
{
    public string Id { get; set; } = Guid.NewGuid().ToString("n");
    public string Name { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TestResult
{
    public string PoolId { get; set; } = "";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Details { get; set; } = "";
    public string Status { get; set; } = "";

    public string Date => Timestamp.ToString("MMMM d, yyyy");
}
