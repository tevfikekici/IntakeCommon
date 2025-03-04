using System;
using System.Collections.Generic;
using System.Text;

namespace Intake.Common
{
    public interface IServerlessLogger
    {
        public void LoadLogger(Serilog.Core.LoggingLevelSwitch levelSwitch, string lambdaName);
    }
}
