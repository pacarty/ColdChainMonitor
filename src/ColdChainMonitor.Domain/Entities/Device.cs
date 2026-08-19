namespace ColdChainMonitor.Domain.Entities;

public class Device
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Location { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private Device() { } // EF Core

    public Device(string name, string location)
    {
        Id = Guid.NewGuid();
        Name = name;
        Location = location;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate() => IsActive = false;
}
