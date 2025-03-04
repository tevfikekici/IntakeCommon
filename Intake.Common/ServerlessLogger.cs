using System;
using System.IO;
using Amazon;
using Amazon.CloudWatchLogs;
using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Sinks.AwsCloudWatch.LogStreamNameProvider;

namespace Intake.Common
{
    public class ServerlessLogger : IServerlessLogger
    {
        private string LogGroupName { get; set; }

        public ServerlessLogger(string logGroupName)
        {
            LogGroupName = logGroupName;
        }

        public void LoadLogger(Serilog.Core.LoggingLevelSwitch levelSwitch, string labdaName)
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.ControlledBy(levelSwitch)
                 .WriteTo.AmazonCloudWatch(

                 logGroup: LogGroupName,
                 //logStreamPrefix: DateTime.UtcNow.ToString("yyyMMddHHmmssfff"),
                 logStreamPrefix: labdaName,
                 createLogGroup: false,
                 cloudWatchClient: new AmazonCloudWatchLogsClient(RegionEndpoint.EUCentral1))
                 .CreateLogger();
        }
    }
}
