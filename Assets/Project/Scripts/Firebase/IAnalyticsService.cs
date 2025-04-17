namespace Project.Scripts.Firebase
{
    public interface IAnalyticsService
    {
        void LogEnemyDeath(int killsCount);
        void LogEntityDeath(int bulletsFired);
        void LogLevelPassed(int levelCount);
    }
}