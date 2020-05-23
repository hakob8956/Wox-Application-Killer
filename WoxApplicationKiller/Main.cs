using System.Collections.Generic;
using System.Linq;
using Wox.Plugin;
using WoxApplicationKiller.DTO;
using WoxApplicationKiller.Extensions;

namespace WoxApplicationKiller
{
    public class Main : IPlugin
    {
        private IProcessManager ProcessManager { get; }
        private IEnumerable<ProcessInformation> ProcessesInformation { get; set; }

        public Main()
        {
            ProcessManager = new ProcessManager(ProcessType.Application);
            ProcessesInformation = new List<ProcessInformation>();
        }

        public void Init(PluginInitContext context)
        { }

        public List<Result> Query(Query query)
        {
            IEnumerable<ProcessInformation> result;
            if (string.IsNullOrEmpty(query.Search))
            {
                ProcessesInformation = ProcessManager.GetAllProcessesInfo();
                result = ProcessesInformation;
            }
            else
            {
                result = ProcessesInformation.GetProcessInformation(query.Search);
            }
            return ResultsMaker(result);
        }

        private List<Result> ResultsMaker(IEnumerable<ProcessInformation> processesInformation)
        {
            var results = new List<Result>();
            foreach (var processInformation in processesInformation)
            {
                results.Add(new Result
                {
                    IcoPath = processInformation.Path,
                    Title = processInformation.Name + " - " + processInformation.Id + " | " + processInformation.Title,
                    SubTitle = processInformation.Path,
                    Action = (c =>
                    {
                        ProcessManager.KillProcess(processInformation.Id);
                        processesInformation = ProcessManager.GetAllProcessesInfo();
                        return true;
                    }),

                });
            }

            return results.ToList();
        }

    }
}
