using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public interface ISyncOption
    {
        string Name { get; }
        SyncType Type { get; }
    }
}
