using System.Collections.Generic;
using Firebase.Analytics;

namespace Project.Scripts.Firebase
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventName, params (string key, object value)[] parameters)
        {
            List<Parameter> firebaseParameters = new();

            foreach (var (key, value) in parameters)
            {
                if (value is string strValue)
                    firebaseParameters.Add(new Parameter(key, strValue));
                else if (value is int intValue)
                    firebaseParameters.Add(new Parameter(key, intValue));
                else if (value is float floatValue)
                    firebaseParameters.Add(new Parameter(key, floatValue));
            }

            FirebaseAnalytics.LogEvent(eventName, firebaseParameters.ToArray());
        }
    }
}
