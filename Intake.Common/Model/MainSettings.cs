using System.Collections.Generic;
using System.Text.Json.Serialization;
using Serilog.Events;

namespace Intake.Common.Model
{

    public enum LevelEnum
    {
        Error, Warning, Information, Debug
    }

    public class MainSettings
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LevelEnum Level { get; set; }
      
        public string LastMappingErrorMsg { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ErrorStatus LastMappingErrorStatus { get; set; } = ErrorStatus.Ok;
        public int LastMappingError { get; set; } = 0;

        public string LastConfigurationErrorMsg { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ErrorStatus LastConfigurationErrorStatus { get; set; } = ErrorStatus.Ok;
        public int LastConfigurationError { get; set; } = 0;

        public int OngoingMappingRequestCounter { get; set; } = 0;
        public bool IsMappingPossible { get; set; } = true;


        public static Serilog.Core.LoggingLevelSwitch MapLogLevel(LevelEnum level)
        {
            var loggingLevelSwitch = new Serilog.Core.LoggingLevelSwitch
            {
                MinimumLevel = level switch
                {
                    LevelEnum.Error => LogEventLevel.Error,
                    LevelEnum.Warning => LogEventLevel.Warning,
                    LevelEnum.Information => LogEventLevel.Information,
                    LevelEnum.Debug => LogEventLevel.Debug,
                    _ => LogEventLevel.Error,
                }
            };
            return loggingLevelSwitch;
        }
    }
}

