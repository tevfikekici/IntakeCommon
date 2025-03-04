using Amazon.S3;
using Intake.Common.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intake.Common
{
    public class MainStatusManager : IMainStatusManager
    {

        private readonly IServerlessLogger serverlessLogger;
        private readonly string s3StatusBucketName;

        public MainStatusManager(AwsOptions awsOptions)
        {
            var configuration = new ConfigurationService().GetConfiguration();
            s3StatusBucketName = awsOptions.CrvStatusBucketName;
            serverlessLogger = new ServerlessLogger(configuration[Constants.AWS_LogGroup]);
        }

        public virtual async Task<MainSettings> GetStatus()
        {

            var s3 = new AmazonS3Client();
            Log.Debug($"reading MainSettings.json: key: {s3StatusBucketName}, file: {Constants.MainSettingsContainer} ");

            MainSettings result;
            try
            {
                result = await AmazonS3.GetObjectAsync<MainSettings>(s3, Constants.MainSettingsContainer, s3StatusBucketName);
            }
            catch (Exception ex)
            {
                result = null;
                Log.Debug(ex, ex.Message);
            }
            if (result == null)
            {
                Log.Debug($"Creating a new MainSettings.json");
                result = new MainSettings()
                {
                    LastConfigurationError = 0,
                    LastConfigurationErrorMsg = string.Empty,
                    LastConfigurationErrorStatus = ErrorStatus.Ok,
                    LastMappingError = 0,
                    LastMappingErrorMsg = string.Empty,
                    LastMappingErrorStatus = ErrorStatus.Ok,
                    Level = LevelEnum.Error,
                    OngoingMappingRequestCounter = 0,
                    IsMappingPossible = true
                };
            }
            return result;
        }

        public async Task SetLogSetings(LevelEnum level)
        {
            try
            {
                Log.Debug($"SetLogSetings level: {level}");

                var mainstatus = await GetStatus();
                mainstatus.Level = level;
                var s3 = new AmazonS3Client();
                _ = await AmazonS3.SetObjectAsync(mainstatus, s3, Constants.MainSettingsContainer, s3StatusBucketName);
                Log.Debug($"SetLogSetings after save");
            }
            catch (Exception ex)
            {
                Log.Debug($"SetLogSetings Error!");
                Log.Error(ex, ex.Message);
            }
        }

         }
}
