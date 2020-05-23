using System;
using WoxApplicationKiller.DTO;

namespace WoxApplicationKiller
{
    internal static class BuilderBind
    {
        public static SearchBinder BuildSearchBinder(string search)
        {
            search = search.Replace(" ", "").ToLower();

            var result = new SearchBinder { Name = search };
            if (search.IndexOf("-", StringComparison.Ordinal) > 0)
            {
                var searchArray = search.Split('-');
                result.Name = searchArray[0];
                result.Id = searchArray[1];
            }
            return result;
        }
    }
}
