using System;
using System.Diagnostics;

public class PerformanceMeasurement : IDisposable
{
    private Stopwatch _stopwatch;
    private TimeSpan _startCpuTime;
    private TimeSpan _cpuTimeUsed;

    public PerformanceMeasurement()
    {
        _stopwatch = new Stopwatch();
    }

    public void Start()
    {
        _stopwatch.Restart(); 
        _startCpuTime = Process.GetCurrentProcess().TotalProcessorTime;
    }

    public void Stop()
    {
        _stopwatch.Stop();
        _cpuTimeUsed = Process.GetCurrentProcess().TotalProcessorTime - _startCpuTime;
        
    }

    public TimeSpan ElapsedWallClockTime => _stopwatch.Elapsed;
    public TimeSpan ElapsedCpuTime => _cpuTimeUsed;

    public void Dispose()
    {
        _stopwatch?.Stop();
    }
}
