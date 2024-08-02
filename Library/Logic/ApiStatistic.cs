using Azure.Data.Tables;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.Functions.Worker.Http;
using System.ComponentModel;
using ScottPlot.Palettes;
using System.Net;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public static class ApiStatistic
    {
        /// <summary>
        /// sample holder type when doing interop
        /// </summary>
        public record GeoLocationRawAPI(dynamic MainRow, dynamic MetadataRow);

        private static readonly TableServiceClient ipAddressServiceClient;
        private static readonly TableServiceClient webPageServiceClient;
        private static readonly TableServiceClient requestUrlStatisticServiceClient;
        private static readonly TableServiceClient subscriberStatisticServiceClient;
        private static readonly TableServiceClient userAgentStatisticServiceClient;
        private static readonly TableServiceClient rawRequestStatisticServiceClient;

        private static readonly TableClient ipAddressStatisticTableClient;
        private static readonly TableClient webPageStatisticTableClient;
        private static readonly TableClient requestUrlStatisticTableClient;
        private static readonly TableClient subscriberStatisticTableClient;
        private static readonly TableClient userAgentStatisticTableClient;
        private static readonly TableClient rawRequestStatisticTableClient;




        /// <summary>
        /// init Table access
        /// </summary>
        static ApiStatistic()
        {
            string accountName = Secrets.Get("CentralStorageAccountName");


            //# RAW REQUEST : (use only when needed, costly🤑)
            //------------------------------------
            //Initialize address table 
            string tableNameRawRequestStatistic = "RawRequestStatistic";
            var storageUriRawRequestStatistic = $"https://{accountName}.table.core.windows.net/{tableNameRawRequestStatistic}";
            //save reference for late use
            rawRequestStatisticServiceClient = new TableServiceClient(new Uri(storageUriRawRequestStatistic), new TableSharedKeyCredential(accountName, Secrets.Get("CentralStorageKey")));
            rawRequestStatisticTableClient = rawRequestStatisticServiceClient.GetTableClient(tableNameRawRequestStatistic);


            //# REQUEST URL
            //------------------------------------
            //Initialize address table 
            string tableNameRequestUrlStatistic = "RequestUrlStatistic";
            var storageUriRequestUrlStatistic = $"https://{accountName}.table.core.windows.net/{tableNameRequestUrlStatistic}";
            //save reference for late use
            requestUrlStatisticServiceClient = new TableServiceClient(new Uri(storageUriRequestUrlStatistic), new TableSharedKeyCredential(accountName, Secrets.Get("CentralStorageKey")));
            requestUrlStatisticTableClient = requestUrlStatisticServiceClient.GetTableClient(tableNameRequestUrlStatistic);

            //# SUBSCRIBER
            //------------------------------------
            //Initialize address table 
            string tableNameSubscriberStatistic = "SubscriberStatistic";
            var storageUriSubscriberStatistic = $"https://{accountName}.table.core.windows.net/{tableNameSubscriberStatistic}";
            //save reference for late use
            subscriberStatisticServiceClient = new TableServiceClient(new Uri(storageUriSubscriberStatistic), new TableSharedKeyCredential(accountName, Secrets.Get("CentralStorageKey")));
            subscriberStatisticTableClient = subscriberStatisticServiceClient.GetTableClient(tableNameSubscriberStatistic);

            //# USER AGENT
            //------------------------------------
            //Initialize address table 
            string tableNameUserAgentStatistic = "UserAgentStatistic";
            var storageUriUserAgentStatistic = $"https://{accountName}.table.core.windows.net/{tableNameUserAgentStatistic}";
            //save reference for late use
            userAgentStatisticServiceClient = new TableServiceClient(new Uri(storageUriUserAgentStatistic), new TableSharedKeyCredential(accountName, Secrets.Get("CentralStorageKey")));
            userAgentStatisticTableClient = userAgentStatisticServiceClient.GetTableClient(tableNameUserAgentStatistic);


            //# IP ADDRESS (ML FEED DATASET)
            //------------------------------------
            //Initialize address table 
            string tableNameIpAddressStatistic = "IpAddressStatistic";
            var storageUriIpAddressStatistic = $"https://{accountName}.table.core.windows.net/{tableNameIpAddressStatistic}";
            //save reference for late use
            ipAddressServiceClient = new TableServiceClient(new Uri(storageUriIpAddressStatistic), new TableSharedKeyCredential(accountName, Secrets.Get("CentralStorageKey")));
            ipAddressStatisticTableClient = ipAddressServiceClient.GetTableClient(tableNameIpAddressStatistic);


            //# WEB PAGE
            //------------------------------------
            //Initialize address table 
            string tableNameWebPageStatistic = "WebPageStatistic";
            var storageUriWebPageStatistic = $"https://{accountName}.table.core.windows.net/{tableNameWebPageStatistic}";
            //save reference for late use
            webPageServiceClient = new TableServiceClient(new Uri(storageUriWebPageStatistic), new TableSharedKeyCredential(accountName, Secrets.Get("CentralStorageKey")));
            webPageStatisticTableClient = webPageServiceClient.GetTableClient(tableNameWebPageStatistic);


        }

        //-------------------------------------


        /// <summary>
        /// Logs IP to for statistics
        /// </summary>

        public static void LogIpAddress(HttpRequestData incomingRequest)
        {
            // Step 1: Get the current month and year in the format "yyyy-MM"
            var todayRecord = DateTime.Now.ToString("yyyy-MM");

            // Step 2: Get the caller's IP address (or use "0.0.0.0" if not available)
            var ipAddress = incomingRequest?.GetCallerIp()?.ToString() ?? "0.0.0.0";

            // Step 3: Check if the IP address already exists in the table
            Expression<Func<IpAddressStatisticEntity, bool>> expression =
                call => call.PartitionKey == ipAddress && call.RowKey == todayRecord;
            var recordFound = ipAddressStatisticTableClient.Query(expression).FirstOrDefault();

            // If the IP address exists, update call statistics
            if (recordFound != null)
            {

                // Calculate calls per second
                if (recordFound.PerSecondTimestamp == null ||
                    ((DateTimeOffset.UtcNow - recordFound.PerSecondTimestamp.Value).TotalSeconds >= 60))
                {
                    recordFound.CallsPerSecond = 1;
                    recordFound.PerSecondTimestamp = DateTimeOffset.UtcNow;
                }
                else
                {
                    recordFound.CallsPerSecond++;
                }

                // Calculate calls per minute
                if (recordFound.PerMinuteTimestamp == null ||
                    ((DateTimeOffset.UtcNow - recordFound.PerMinuteTimestamp.Value).TotalMinutes >= 1))
                {
                    recordFound.CallsPerMinute = recordFound.CallsPerSecond;
                    recordFound.CallsPerSecond = 0;
                    recordFound.PerSecondTimestamp = null;
                    recordFound.PerMinuteTimestamp = DateTimeOffset.UtcNow;
                }
                else
                {
                    recordFound.CallsPerMinute += recordFound.CallsPerSecond;
                    recordFound.CallsPerSecond = 0;
                }

                // Calculate calls per hour
                if (recordFound.PerHourTimestamp == null ||
                    ((DateTimeOffset.UtcNow - recordFound.PerHourTimestamp.Value).TotalHours >= 1))
                {
                    recordFound.CallsPerHour = recordFound.CallsPerMinute;
                    recordFound.CallsPerMinute = 0;
                    recordFound.PerMinuteTimestamp = null;
                    recordFound.PerHourTimestamp = DateTimeOffset.UtcNow;
                }
                else
                {
                    recordFound.CallsPerHour += recordFound.CallsPerMinute;
                    recordFound.CallsPerMinute = 0;
                }

                // Calculate calls per day
                if (recordFound.PerDayTimestamp == null ||
                    ((DateTimeOffset.UtcNow - recordFound.PerDayTimestamp.Value).TotalDays >= 1))
                {
                    recordFound.CallsPerDay = recordFound.CallsPerHour;
                    recordFound.CallsPerHour = 0;
                    recordFound.PerHourTimestamp = null;
                    recordFound.PerDayTimestamp = DateTimeOffset.UtcNow;
                }
                else
                {
                    recordFound.CallsPerDay += recordFound.CallsPerHour;
                    recordFound.CallsPerHour = 0;
                }

                // Calculate calls per month
                if (recordFound.PerMonthTimestamp == null ||
                    ((DateTimeOffset.UtcNow - recordFound.PerMonthTimestamp.Value).TotalDays >= DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month)))
                {
                    recordFound.CallsPerMonth = recordFound.CallsPerDay;
                    recordFound.CallsPerDay = 0;
                    recordFound.PerDayTimestamp = null;
                    recordFound.PerMonthTimestamp = DateTimeOffset.UtcNow;
                }
                else
                {
                    recordFound.CallsPerMonth += recordFound.CallsPerDay;
                    recordFound.CallsPerDay = 0;
                }

                // Update the entity in the table
                ipAddressStatisticTableClient.UpsertEntityAsync(recordFound);
            }
            else
            {
                //Create a new log entry for the IP address
                var newRow = new IpAddressStatisticEntity();
                newRow.PartitionKey = Tools.CleanAzureTableKey(ipAddress);
                newRow.RowKey = todayRecord;
                ipAddressStatisticTableClient.AddEntity(newRow);
            }

        }


        public static void LogWebPage(string webPage)
        {
            //get month and year in correct format 2019-10
            var todayRecord = DateTime.Now.ToString("yyyy-MM");

            //# get ip address out
            //var ipAddress = incomingRequest?.GetCallerIp()?.ToString() ?? "0.0.0.0";
            var cleanWebPageUrl = Tools.CleanAzureTableKey(webPage);
            //# check if already exist
            Expression<Func<WebPageStatisticEntity, bool>> expression = call => call.PartitionKey == cleanWebPageUrl && call.RowKey == todayRecord;

            //execute search
            var recordFound = webPageStatisticTableClient.Query(expression).FirstOrDefault();

            //# if existed, update call count
            var isExist = recordFound != null;
            if (isExist)
            {
                //update row
                recordFound.CallCount = ++recordFound.CallCount; //increment call count
                webPageStatisticTableClient.UpsertEntity(recordFound);
            }

            //# if not exist, make new log
            else
            {
                var newRow = new WebPageStatisticEntity();

                newRow.PartitionKey = cleanWebPageUrl;
                //get month and year in correct format 2019-10
                newRow.RowKey = todayRecord;
                newRow.CallCount = 1;
                webPageStatisticTableClient.AddEntity(newRow);
            }
        }

        public static void LogRequestUrl(HttpRequestData incomingRequest)
        {

            //# get request URL
            var requestUrl = incomingRequest?.Url.AbsolutePath ?? "no URL";

            //get month and year in correct format 2019-10
            var todayRecord = DateTime.Now.ToString("yyyy-MM");

            //# check if URL already exist
            //make a search for ip address stored under row key
            var cleanAzureTableKey = Tools.CleanAzureTableKey(requestUrl, "-").Truncate(100); //keep short as not overcrowd
            Expression<Func<RequestUrlStatisticEntity, bool>> expression = call => call.PartitionKey == cleanAzureTableKey && call.RowKey == todayRecord;

            //execute search
            var recordFound = requestUrlStatisticTableClient.Query(expression).FirstOrDefault();

            //# if existed, update call count
            var isExist = recordFound != null;
            if (isExist)
            {
                //update row
                recordFound.CallCount = ++recordFound.CallCount; //increment call count
                requestUrlStatisticTableClient.UpsertEntity(recordFound);
            }

            //# if not exist, make new log
            else
            {
                var newRow = new RequestUrlStatisticEntity();

                newRow.PartitionKey = cleanAzureTableKey;
                //get month and year in correct format 2019-10
                newRow.RowKey = todayRecord;
                newRow.CallCount = 1;
                requestUrlStatisticTableClient.AddEntity(newRow);
            }
        }

        public static void LogSubscriber(HttpRequestData incomingRequest)
        {
            //get host address as main ID of record
            var host = incomingRequest.ExtractHostAddress();

            //get date that this record would be in (Row Key)
            var currentDate = DateTime.Now.ToString("yyyy-MM");

            //# check if URL already exist
            //make a search for ip address stored under row key
            var cleanHostAddress = Tools.CleanAzureTableKey(host, "|");
            Expression<Func<RequestUrlStatisticEntity, bool>> expression = call =>
                    call.PartitionKey == cleanHostAddress &&
                    call.RowKey == currentDate;

            //execute search
            var recordFound = subscriberStatisticTableClient.Query(expression).FirstOrDefault();

            //# if existed, update call count
            var isExist = recordFound != null;
            if (isExist)
            {
                //update row
                recordFound.CallCount = ++recordFound.CallCount; //increment call count
                subscriberStatisticTableClient.UpsertEntity(recordFound);
            }

            //# if not exist, make new log
            else
            {
                var newRow = new SubscriberStatisticEntity();
                newRow.PartitionKey = cleanHostAddress;
                //get month and year in correct format 2019-10
                newRow.RowKey = currentDate;
                newRow.CallCount = 1; //start with 1
                //save to db
                subscriberStatisticTableClient.AddEntity(newRow);
            }
        }


        public static void LogUserAgent(HttpRequestData incomingRequest)
        {
            //get host address as main ID of record
            var requestHeaderList = incomingRequest.Headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
            requestHeaderList.TryGetValue("User-Agent", out var userAgentValues);
            var userAgent = userAgentValues?.FirstOrDefault() ?? "no User-Agent";

            //get date that this record would be in (Row Key)
            var currentDate = DateTime.Now.ToString("yyyy-MM");

            //# check if User-Agent already exist
            //make a search for ip address stored under row key
            var cleanUserAgent = Tools.CleanAzureTableKey(userAgent, "|");
            Expression<Func<UserAgentStatisticEntity, bool>> expression = call => call.PartitionKey == cleanUserAgent && call.RowKey == currentDate;

            //execute search
            var recordFound = userAgentStatisticTableClient.Query(expression).FirstOrDefault();

            //# if existed, update call count
            var isExist = recordFound != null;
            if (isExist)
            {
                //update row
                recordFound.CallCount = ++recordFound.CallCount; //increment call count
                userAgentStatisticTableClient.UpsertEntity(recordFound);
            }

            //# if not exist, make new log
            else
            {
                var newRow = new UserAgentStatisticEntity();
                newRow.PartitionKey = cleanUserAgent;
                //get month and year in correct format 2019-10
                newRow.RowKey = currentDate;
                newRow.CallCount = 1; //start with 1
                //save to db
                userAgentStatisticTableClient.AddEntity(newRow);
            }
        }

        /// <summary>
        /// Makes raw full header log of what ever that comes in
        /// NOTE: high cost carefully use
        /// </summary>
        public static void LogRawRequest(HttpRequestData incomingRequest)
        {
            //step 1: extract needed data from request
            var newRow = new RawRequestStatisticEntity();

            //convert to list
            var requestHeaderList = incomingRequest.Headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);

            for (int i = 0; i < requestHeaderList.Count; i++)
            {
                var currentHeader = requestHeaderList.ElementAt(i);
                var currentHeaderKey = currentHeader.Key;
                string currentValue = Tools.ListToString(currentHeader.Value.ToList());

                //debug print
                //Console.WriteLine($"{currentHeaderKey}:{currentValue}");

                //match with correct header based on attribute and fill in the value
                // Get all properties of the current instance
                var properties = newRow.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var attribute = (DescriptionAttribute)property.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                    if (attribute?.Description.Equals(currentHeaderKey, StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        property.SetValue(newRow, currentValue);
                        break;
                    }
                }
            }

            //step 2: generate hash to identify the data
            newRow.PartitionKey = incomingRequest?.GetCallerIp()?.ToString() ?? "no ip";
            //newRow.PartitionKey = newRow.CalculateCombinedHash();
            var url = incomingRequest.Url.ToString() ?? "no URL";
            newRow.RowKey = Tools.CleanAzureTableKey(url, "|"); //place url

            //step 3: add entry to database
            //TODO check if exist before overwrite
            rawRequestStatisticTableClient.UpsertEntity(newRow);
        }


        public static void Log(HttpRequestData incomingRequest)
        {
            //ApiStatistic.LogIpAddress(incomingRequest);
            //ApiStatistic.LogRequestUrl(incomingRequest);
            //ApiStatistic.LogRawRequest(incomingRequest);
            //ApiStatistic.LogSubscriber(incomingRequest);
            //ApiStatistic.LogUserAgent(incomingRequest);

        }
    }

}
