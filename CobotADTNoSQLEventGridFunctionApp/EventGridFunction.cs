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
using CobotADTNoSQLEventGridFunctionApp.Helper;

namespace CobotADTNoSQLEventGridFunctionApp
{
    public static class EventGridFunction
    {
        private static readonly string adtInstanceUrl = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");
        private static readonly string cosmosUri = Environment.GetEnvironmentVariable("COSMOS_URI");
        private static readonly string cosmosKey = Environment.GetEnvironmentVariable("COSMOS_KEY");
        [FunctionName("ProcessADTRoutedDataToNoSql")]
        public static async Task ProcessADTRoutedDataToNoSqlAsync([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential();
            DigitalTwinsClient digitalTwinsClient = new DigitalTwinsClient(endpoint: new Uri(adtInstanceUrl), credential: defaultAzureCredential);
            CosmosClient cosmosClient = new CosmosClient(accountEndpoint: cosmosUri, authKeyOrResourceToken: cosmosKey);
            Database cobotDatabase = await cosmosClient.CreateDatabaseIfNotExistsAsync(id: "cobotDatabase");
            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                RootModel rootModel = JsonConvert.DeserializeObject<RootModel>(eventGridEvent.Data.ToString());
                log.LogInformation(JsonConvert.SerializeObject(rootModel, Formatting.Indented));
                Azure.JsonPatchDocument jsonPatchDocument = new Azure.JsonPatchDocument();
                DateTime dateTime = DateTime.Now;
                switch (rootModel.Data.ModelId)
                {
                    case "dtmi:com:Cobot:Cobot;1":
                        CobotRecord cobotRecord = new(
                            id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                            deviceId: "Cobot",
                            timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                            elapsedTime: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/ElapsedTime")).Value);
                        Container cobotContainer = cobotDatabase.GetContainer(id: "cobotContainer");
                        CobotRecord cobotRecordItem = await cobotContainer.CreateItemAsync<CobotRecord>(
                            item: cobotRecord,
                            partitionKey: new PartitionKey("Cobot"));
                        log.LogInformation(JsonConvert.SerializeObject(cobotRecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:Payload;1":
                        PayloadRecord payloadRecord = new(
                            id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                            deviceId: "Payload",
                            timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                            mass: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Mass")).Value,
                            cogx: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/CogX")).Value,
                            cogy: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/CogY")).Value,
                            cogz: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/CogZ")).Value);
                        Container payloadContainer = cobotDatabase.GetContainer(id: "payloadContainer");
                        PayloadRecord payloadRecordItem = await payloadContainer.CreateItemAsync<PayloadRecord>(
                            item: payloadRecord,
                            partitionKey: new PartitionKey("Payload"));
                        log.LogInformation(JsonConvert.SerializeObject(payloadRecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:ControlBox;1":
                        ControlBoxRecord controlBoxRecord = new(
                            id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                            deviceId: "ControlBox",
                            timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                            voltage: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        Container controlBoxContainer = cobotDatabase.GetContainer(id: "controlBoxContainer");
                        ControlBoxRecord controlBoxRecordItem = await controlBoxContainer.CreateItemAsync<ControlBoxRecord>(
                            item: controlBoxRecord,
                            partitionKey: new PartitionKey("ControlBox"));
                        log.LogInformation(JsonConvert.SerializeObject(controlBoxRecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:JointLoad:Elbow;1":
                        ElbowRecord elbowRecord = new(
                            id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                            deviceId: "Elbow",
                            timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                            position: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value,
                            temperature: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value,
                            voltage: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value,
                            x: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/X")).Value,
                            y: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Y")).Value,
                            z: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Z")).Value);
                        Container elbowContainer = cobotDatabase.GetContainer(id: "elbowContainer");
                        ElbowRecord elbowRecordItem = await elbowContainer.CreateItemAsync<ElbowRecord>(
                            item: elbowRecord,
                            partitionKey: new PartitionKey("Elbow"));
                        log.LogInformation(JsonConvert.SerializeObject(elbowRecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:JointLoad:Base;1":
                        BaseRecord baseRecord = new(
                            id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                            deviceId: "Base",
                            timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                            position: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value,
                            temperature: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value,
                            voltage: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        Container baseContainer = cobotDatabase.GetContainer(id: "baseContainer");
                        BaseRecord baseRecordItem = await baseContainer.CreateItemAsync<BaseRecord>(
                            item: baseRecord,
                            partitionKey: new PartitionKey("Base"));
                        log.LogInformation(JsonConvert.SerializeObject(baseRecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:JointLoad:Shoulder;1":
                        ShoulderRecord shoulderRecord = new(
                           id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                           deviceId: "Shoulder",
                           timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                           position: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value,
                           temperature: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value,
                           voltage: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        Container shoulderContainer = cobotDatabase.GetContainer(id: "shoulderContainer");
                        ShoulderRecord shoulderRecordItem = await shoulderContainer.CreateItemAsync<ShoulderRecord>(
                            item: shoulderRecord,
                            partitionKey: new PartitionKey("Shoulder"));
                        log.LogInformation(JsonConvert.SerializeObject(shoulderRecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist1;1":
                        Wrist1Record wrist1Record = new(
                          id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                          deviceId: "Wrist1",
                          timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                          position: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value,
                          temperature: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value,
                          voltage: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        Container wrist1Container = cobotDatabase.GetContainer(id: "wrist1Container");
                        Wrist1Record wrist1RecordItem = await wrist1Container.CreateItemAsync<Wrist1Record>(
                            item: wrist1Record,
                            partitionKey: new PartitionKey("Wrist1")
                        );
                        log.LogInformation(JsonConvert.SerializeObject(wrist1RecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist2;1":
                        Wrist2Record wrist2Record = new(
                         id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                         deviceId: "Wrist2",
                         timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                         position: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value,
                         temperature: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value,
                         voltage: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        Container wrist2Container = cobotDatabase.GetContainer(id: "wrist2Container");
                        Wrist2Record wrist2RecordItem = await wrist2Container.CreateItemAsync<Wrist2Record>(
                            item: wrist2Record,
                            partitionKey: new PartitionKey("Wrist2")
                        );
                        log.LogInformation(JsonConvert.SerializeObject(wrist2RecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist3;1":
                        Wrist3Record wrist3Record = new(
                        id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                        deviceId: "Wrist3",
                        timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                        position: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value,
                        temperature: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value,
                        voltage: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        Container wrist3Container = cobotDatabase.GetContainer(id: "wrist3Container");
                        Wrist3Record wrist3RecordItem = await wrist3Container.CreateItemAsync<Wrist3Record>(
                            item: wrist3Record,
                            partitionKey: new PartitionKey("Wrist3"));
                        log.LogInformation(JsonConvert.SerializeObject(wrist3RecordItem, Formatting.Indented));
                        break;
                    case "dtmi:com:Cobot:JointLoad:Tool;1":
                        ToolRecord toolRecord = new(
                         id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                         deviceId: "Tool",
                         timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                         temperature: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value,
                         voltage: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value,
                         x: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/X")).Value,
                         y: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Y")).Value,
                         z: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Z")).Value,
                         rx: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Rx")).Value,
                         ry: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Ry")).Value,
                         rz: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/Rz")).Value);
                        Container toolContainer = cobotDatabase.GetContainer(id: "toolContainer");
                        ToolRecord toolRecordItem = await toolContainer.CreateItemAsync<ToolRecord>(
                            item: toolRecord,
                            partitionKey: new PartitionKey("Tool"));
                        log.LogInformation(JsonConvert.SerializeObject(toolRecordItem, Formatting.Indented));
                        break;
                    default:
                        break;
                }
            }

        }
    }
}