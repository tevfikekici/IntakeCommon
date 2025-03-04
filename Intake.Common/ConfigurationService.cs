using Microsoft.Extensions.Configuration;

namespace Intake.Common
{
    public class ConfigurationService : IConfigurationService
    {
        public IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder().AddEnvironmentVariables().Build();
        }
    }
}
