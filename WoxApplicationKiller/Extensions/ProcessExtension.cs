using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WoxApplicationKiller.DTO;

namespace WoxApplicationKiller.Extensions
{
    internal static class ProcessExtension
    {
        public static Process[] SetFilter(this Process[] processes, ProcessType type)
        {
            switch (type)
            {
                case ProcessType.Application:
                    return processes.Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowHandle != IntPtr.Zero).ToArray();
                default:
                    return Array.Empty<Process>();
            }
        }

        public static IEnumerable<ProcessInformation> GetProcessInformation(this IEnumerable<ProcessInformation> processes, string search)
        {
            var searchBinder = BuilderBind.BuildSearchBinder(search);

            if (searchBinder.Id == null)
            {
                return processes.Where(p => p.Name.ToLower().Contains(searchBinder.Name));
            }
            else
            {
                return processes.Where(p => p.Name.ToLower().Contains(searchBinder.Name) && p.Id.ToString().StartsWith(searchBinder.Id));
            }


        }

    }
}
