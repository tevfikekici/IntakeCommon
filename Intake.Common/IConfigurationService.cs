using Microsoft.Extensions.Configuration;

namespace Intake.Common
{
    public interface IConfigurationService
    {
        IConfiguration GetConfiguration();
    }
}
