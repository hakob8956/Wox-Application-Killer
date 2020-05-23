using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WoxApplicationKiller.DTO;
using WoxApplicationKiller.Extensions;

namespace WoxApplicationKiller
{
    internal sealed class ProcessManager:IProcessManager
    {
        private ProcessType ProcessType { get; set; }
        public ProcessManager(ProcessType type)
        {
            this.ProcessType = type;
        }

        public  List<ProcessInformation> GetAllProcessesInfo()
        {
            List<ProcessInformation> processesInformation = new List<ProcessInformation>();
            try
            {
                processesInformation = Process.GetProcesses()
                    .SetFilter(ProcessType)
                    .Select(p => new ProcessInformation
                    {
                        Id = p.Id,
                        Name = p.ProcessName,
                        Title = p.MainWindowTitle,
                        Path = GetPath(p)
                    }).ToList();
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }

            return processesInformation;
        }

        public void KillProcess(int id)
        {
            Process.GetProcessById(id).Kill();
        }
        private string GetPath(Process p)
        {
            return p.MainModule?.FileName;

        }

    }
}
