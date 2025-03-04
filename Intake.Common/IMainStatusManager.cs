using Intake.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intake.Common
{
    public interface IMainStatusManager
    {
        Task<MainSettings> GetStatus();
        Task SetLogSetings(LevelEnum level);     
     
    }
}
