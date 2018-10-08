using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace CaleresPOCFunctionApp1
{
    public static class TimerTriggerFunction3
    {
        private static string key = TelemetryConfiguration.Active.InstrumentationKey
            = System.Environment.GetEnvironmentVariable(
                "APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
        private static TelemetryClient telemetry = new TelemetryClient()
        {
            InstrumentationKey = key
        };

        [FunctionName("TimerTriggerFunction3")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            telemetry.Context.Operation.Id = context.InvocationId.ToString();
            telemetry.Context.Operation.Name = context.FunctionName;

            log.LogInformation($"C# Timer trigger function 3 executed at: {DateTime.Now}. Log lvl Info");

            // Track a Metric
            var metric = new MetricTelemetry("Test Metric", DateTime.Now.Millisecond);
            telemetry.TrackMetric(metric);
        }
    }
}
