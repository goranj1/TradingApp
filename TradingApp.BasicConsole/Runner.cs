using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NLog;

namespace TradingApp.BasicConsole
{
    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public async Task DoAction(string name)
        {
            try
            {
                TelemetryConfiguration.Active.InstrumentationKey = "5094790a-8cfd-4134-851d-a9b75c314031";

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    "DefaultEndpointsProtocol=https;AccountName=tradingappstorage;AccountKey=PuF4OONFBfBBOTbBlITwIsxfXYUCZCRRN1rid1RNuCTA6wLYyFayqV88+u1KL5Nnvl8WTP+e1Bz+UU0hktVg0g==;EndpointSuffix=core.windows.net");

                DependencyTrackingTelemetryModule module = new DependencyTrackingTelemetryModule();

                // You can prevent correlation header injection to some domains by adding it to the excluded list.
                // Make sure you add a Storage endpoint. Otherwise, you might experience request signature validation issues on the Storage service side.
                module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.windows.net");
                module.Initialize(TelemetryConfiguration.Active);
                var telemetryClient = new TelemetryClient();
         //       var operation = telemetryClient.StartOperation<DependencyTelemetry>("enqueue " + "myqueue");
          //      operation.Telemetry.Type = "Queue";
          //      operation.Telemetry.Data = "Enqueue " + "myqueue";



                // Create the queue client.
                var queueClient = storageAccount.CreateCloudQueueClient();

                // Retrieve a reference to a container.
                var queue = queueClient.GetQueueReference("myqueue");

                // Create the queue if it doesn't already exist
                await queue.CreateIfNotExistsAsync();
                await queue.AddMessageAsync(new CloudQueueMessage("test"));
          //      telemetryClient.StopOperation(operation);

                telemetryClient.TrackEvent("MyEvent");
                //severity level in application insights search will be verbose
                _logger.LogDebug("Doing hard work(!!) {name}",name);
                

                //severity level in application insights search will be Informational
                //      _logger.LogInformation($"Doing hard work!! {name}");
                LogEventInfo logEventinfo = new LogEventInfo();

                using (_logger.BeginScope(new[] { new KeyValuePair<string, object>("username","goran") }))
                {
                    _logger.LogDebug("Logon from me");
                }

                try
                {
                    throw new ArgumentException("Bad Argument");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error");
            }

        }


    }
}