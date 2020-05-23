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
        private List<ProcessInformation> AllProcessesInformation { get; set; }
        private List<ProcessInformation> CurrentProcessesInformation { get; set; }


        public Main()
        {
            ProcessManager = new ProcessManager(ProcessType.Application);
            AllProcessesInformation = new List<ProcessInformation>();
        }

        public void Init(PluginInitContext context)
        { }

        public List<Result> Query(Query query)
        {
            bool kilAll = false;
            if (string.IsNullOrWhiteSpace(query.Search) || query.Search.ToLower() == "all")
            {
                AllProcessesInformation = ProcessManager.GetAllProcessesInfo();
                CurrentProcessesInformation = AllProcessesInformation;
                kilAll = true;
            }
            else
            {
                CurrentProcessesInformation = AllProcessesInformation.GetProcessInformation(query.Search);
            }
            return ResultsMaker(query.Search, kilAll);
        }

        private List<Result> ResultsMaker(string search, bool killAll)
        {
            var results = new List<Result>();
            AddAllProcessResult(search, killAll, ref results);
            foreach (var processInformation in CurrentProcessesInformation)
            {
                results.Add(new Result
                {
                    IcoPath = processInformation.Path,
                    Title = processInformation.Name + " - " + processInformation.Id + " | " + processInformation.Title,
                    SubTitle = processInformation.Path,
                    Action = (_ =>
                    {
                        ProcessManager.KillProcess(processInformation.Id);
                        CurrentProcessesInformation = ProcessManager.GetAllProcessesInfo();
                        return true;
                    }),

                });
            }

            return results.ToList();
        }

        private void AddAllProcessResult(string search, bool kilAll, ref List<Result> results)
        {
            if (CurrentProcessesInformation.Count() > 1 && (!string.IsNullOrWhiteSpace(search) || kilAll))
            {
                string title = $"kill all \"{search}\" process";
                if (kilAll)
                {
                    title = $"kill all";
                }
                results.Insert(0, new Result
                {
                    IcoPath = "Images\\all.png",
                    Title = title,
                    Action = (_ =>
                    {
                        ProcessManager.KillProcesses(CurrentProcessesInformation.Select(p => p.Id).ToArray());
                        CurrentProcessesInformation = ProcessManager.GetAllProcessesInfo();
                        return true;
                    }),
                });
            }
        }

    }
}
