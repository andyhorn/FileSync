using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public class SyncOption : ISyncOption
    {
        public string Name { get; }
        public SyncType Type { get; }

        public SyncOption(SyncType type)
        {
            Type = type;
            Name = GetName(type);
        }

        private string GetName(SyncType type)
        {
            var remove = new string[] { "Copy" };
            var str = type.ToString();
            var sections = str.Split(remove, StringSplitOptions.RemoveEmptyEntries);
            return $"Sync {sections[0]}";
        }
    }
}
