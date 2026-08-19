namespace ColdChainMonitor.DeviceSimulator;

public class SimulatorOptions
{
    public const string SectionName = "Simulator";

    public string ApiBaseUrl { get; set; } = "http://localhost:5116";
    public int DeviceCount { get; set; } = 3;
    public int IntervalSeconds { get; set; } = 5;

    // Chance (0.0-1.0) that any given reading is an out-of-range excursion rather than normal.
    public double ExcursionProbability { get; set; } = 0.1;
}
