using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace CaleresPOCFunctionApp1
{
    public static class TimerTriggerFunction2
    {
        private static string key = TelemetryConfiguration.Active.InstrumentationKey 
            = System.Environment.GetEnvironmentVariable(
                "APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
        private static TelemetryClient telemetry = new TelemetryClient()
        {
            InstrumentationKey = key
        };

        [FunctionName("TimerTriggerFunction2")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            telemetry.Context.Operation.Id = context.InvocationId.ToString();
            telemetry.Context.Operation.Name = context.FunctionName;

            //generate some dummy errors on certian day of the week.
            if (DateTime.Now.DayOfWeek == DateTime.Parse("10/09/2018").DayOfWeek)
            {
                log.LogCritical($"C# Timer trigger function 2 executed at: {DateTime.Now}. Log lvl Critical");
            }
            log.LogInformation($"C# Timer trigger function 2 executed at: {DateTime.Now}. Log lvl Information");

            // Track a Metric something like records process but fake it here with an int.
            var metric = new MetricTelemetry("Test Metric", DateTime.Now.Day);
            telemetry.TrackMetric(metric);
        }
    }
}
