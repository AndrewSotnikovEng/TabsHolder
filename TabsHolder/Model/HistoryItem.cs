using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder.Model
{
    public class HistoryItem
    {
        public string FullPath { get; set; }
        public string Name { get; set; }

        public HistoryItem(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileNameWithoutExtension(FullPath);
        }

        public HistoryItem()
        {
        }
    }
}
