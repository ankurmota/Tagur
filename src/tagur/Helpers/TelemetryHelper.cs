using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Helpers
{
    public class TelemetryHelper
    {
        public static void AddEvent(string eventName)
        {
           TelemetryClient telemetry = new TelemetryClient();
            telemetry.TrackEvent(eventName);
        }

        public static void AddMetric(string metricName, int value)
        {
            TelemetryClient telemetry = new TelemetryClient();
            telemetry.TrackMetric(metricName, value);
        }

       
    }
}
