using Firebase.Analytics;

namespace Project.Scripts.Firebase
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        public void LogEnemyDeath(int killsCount)
        {
            FirebaseAnalytics.LogEvent("enemy_death", new Parameter("kills_count", killsCount));
        }

        public void LogEntityDeath(int bulletsFired)
        {
            FirebaseAnalytics.LogEvent("entity_death", new Parameter("bullets_fired", bulletsFired));
        }

        public void LogLevelPassed(int levelCount)
        {
            FirebaseAnalytics.LogEvent("level_passed", new Parameter("levels_number", levelCount));
        }
    }
}
