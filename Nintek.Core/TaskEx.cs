using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core
{
    public static class TaskEx
    {
        public static async Task<T[]> OneAtATime<T>(IEnumerable<Task<T>> tasks)
        {
            var results = new List<T>();
            foreach (var task in tasks)
            {
                var result = await task;
                results.Add(result);
            }
            return results.ToArray();
        }
    }
}
