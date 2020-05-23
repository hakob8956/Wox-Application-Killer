using System.Collections.Generic;
using WoxApplicationKiller.DTO;

namespace WoxApplicationKiller
{
    internal interface IProcessManager
    {
        List<ProcessInformation> GetAllProcessesInfo();
        void KillProcess(int id);
        void KillProcesses(int[] ids = null);
    }
}
