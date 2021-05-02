using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabsHolder.Model;

namespace TabsHolder.Data
{
    public class Config
    {
        public string browserPath;
        public ObservableCollection<HistoryItem> TabsHistory;

        public Config(string browserPath, ObservableCollection<HistoryItem> tabsHistory)
        {
            this.browserPath = browserPath;
            TabsHistory = tabsHistory;
        }

        public Config()
        {

        }
    }


}
