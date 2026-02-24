namespace Arachne.Abstractions.Interfaces.Crawler;

public interface ICrawlerMetrics
{
    long AverageRunTimeMs { get; }
    long MaxRunTimeMs { get; }
    long MinRunTimeMs { get; }
    long SuccessCount { get; }
    long FailureCount { get; }
    int QueueLength { get; }
    void IncrementSuccess();
    void IncrementFailure();
    void SetQueueLength(int length);
    void AddTaskRunTime(long ms);
    IDisposable MeasureTaskTime();
}