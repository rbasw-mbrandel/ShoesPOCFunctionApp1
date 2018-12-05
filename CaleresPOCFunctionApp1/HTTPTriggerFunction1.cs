
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace CaleresPOCFunctionApp1
{
    public static class HTTPTriggerFunction1
    {
        private static string key = TelemetryConfiguration.Active.InstrumentationKey
            = System.Environment.GetEnvironmentVariable(
                "APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
        private static TelemetryClient telemetry = new TelemetryClient()
        {
            InstrumentationKey = key
        };

        [FunctionName("HTTPSTriggerFunction1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function 1 processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            telemetry.Context.Operation.Id = context.InvocationId.ToString();
            telemetry.Context.Operation.Name = context.FunctionName;

            //generate some dummy errors on certian day of the week.
            if (DateTime.Now.DayOfWeek == DateTime.Parse("10/09/2018").DayOfWeek)
            {
                log.LogCritical($"C# HTTP trigger function 1 executed at: {DateTime.Now}. Log lvl Critical");
            }
            log.LogInformation($"C# HTTP trigger function 1 executed at: {DateTime.Now}. Log lvl Information");

            // Track a Metric something like records process but fake it here with an int.
            var metric = new MetricTelemetry("Test Metric", DateTime.Now.Day);
            telemetry.TrackMetric(metric);

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
