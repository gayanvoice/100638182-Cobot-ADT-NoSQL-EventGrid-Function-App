// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using CobotADTNoSQLEventGridFunctionApp.Record;
using Microsoft.Azure.Cosmos;
using Azure.Identity;
using Azure.DigitalTwins.Core;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CobotADTNoSQLEventGridFunctionApp.Model;

namespace CobotADTNoSQLEventGridFunctionApp
{
    public static class EventGridFunction
    {
        private static readonly string adtInstanceUrl = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");
        private static readonly string cosmosUri = Environment.GetEnvironmentVariable("COSMOS_URI");
        [FunctionName("ProcessADTRoutedDataToNoSql")]
        public static async Task ProcessADTRoutedDataToNoSqlAsync([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential();
            DigitalTwinsClient digitalTwinsClient = new DigitalTwinsClient(endpoint: new Uri(adtInstanceUrl), credential: defaultAzureCredential);
            log.LogInformation(eventGridEvent.Data.ToString());
            log.LogInformation($"ADT service client connection created.");
            CosmosClient cosmosClient = new CosmosClient(accountEndpoint: cosmosUri, tokenCredential: defaultAzureCredential);
            log.LogInformation($"Cosmos service client connection created.");
            log.LogInformation(eventGridEvent.Data.ToString());
            Database cosmosDatabase = await cosmosClient.CreateDatabaseIfNotExistsAsync(id: "cobotDb");
            log.LogInformation(cosmosDatabase.ToString());
            Container cobotContainer = cosmosDatabase.GetContainer(id: "cobotContainer");
            log.LogInformation(cobotContainer.ToString());
            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                RootModel rootModel = JsonConvert.DeserializeObject<RootModel>(eventGridEvent.Data.ToString());
                log.LogInformation(JsonConvert.SerializeObject(rootModel, Formatting.Indented));
                Azure.JsonPatchDocument jsonPatchDocument = new Azure.JsonPatchDocument();
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                log.LogInformation(timestamp);
                switch (rootModel.Data.ModelId)

                {
                    case "dtmi:com:Cobot:Cobot;1":
                        log.LogInformation(rootModel.Data.ModelId);

                        Cobot cobot = new(
                            id: $"cobot-{timestamp}",
                            timestamp: timestamp,
                            elapsedTime: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/ElapsedTime")).Value
                        );
                        Cobot cobotCreatedItem = await cobotContainer.CreateItemAsync<Cobot>(
                            item: cobot,
                            partitionKey: new PartitionKey(timestamp)
                        );
                        Console.WriteLine($"Created item:\t{cobotCreatedItem.id}\t{cobotCreatedItem.elapsedTime}\t[{cobotCreatedItem.timestamp}]");

                        break;
                    case "dtmi:com:Cobot:Payload;1":
                        //jsonPatchDocument.AppendReplace("/Mass", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Mass")).Value);
                        //jsonPatchDocument.AppendReplace("/CogX", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/CogX")).Value);
                        //jsonPatchDocument.AppendReplace("/CogY", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/CogY")).Value);
                        //jsonPatchDocument.AppendReplace("/CogZ", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/CogZ")).Value);
                        break;
                    case "dtmi:com:Cobot:ControlBox;1":
                        //jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        //await client.UpdateDigitalTwinAsync("TControlBox", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Elbow;1":
                        //jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        //jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        //jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        //jsonPatchDocument.AppendReplace("/X", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/X")).Value);
                        //jsonPatchDocument.AppendReplace("/Y", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Y")).Value);
                        //jsonPatchDocument.AppendReplace("/Z", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Z")).Value);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Base;1":
                        //jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        //jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        //jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Shoulder;1":
                        //jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        //jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        //jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist1;1":
                        //jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        //jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        //jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        //await client.UpdateDigitalTwinAsync("TWrist1", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist2;1":
                        //jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        //jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        //jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        //await client.UpdateDigitalTwinAsync("TWrist2", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist3;1":
                        //jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        //jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        //jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        //await client.UpdateDigitalTwinAsync("TWrist3", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Tool;1":
                        //jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        //jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        //jsonPatchDocument.AppendReplace("/X", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/X")).Value);
                        //jsonPatchDocument.AppendReplace("/Y", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Y")).Value);
                        //jsonPatchDocument.AppendReplace("/Z", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Z")).Value);
                        //jsonPatchDocument.AppendReplace("/Rx", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Rx")).Value);
                        //jsonPatchDocument.AppendReplace("/Ry", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Ry")).Value);
                        //jsonPatchDocument.AppendReplace("/Rz", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Rz")).Value);
                        //await client.UpdateDigitalTwinAsync("TTool", jsonPatchDocument);
                        break;
                    default:
                        break;
                }
            }

        }
    }
}