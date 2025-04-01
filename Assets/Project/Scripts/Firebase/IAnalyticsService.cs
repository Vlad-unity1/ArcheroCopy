public interface IAnalyticsService
{
    void LogEvent(string eventName, params (string key, object value)[] parameters);
}