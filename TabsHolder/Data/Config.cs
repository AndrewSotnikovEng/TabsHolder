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
        public string repositoryPath;

        public ObservableCollection<HistoryItem> TabsHistory;

        public Config(string browserPath, string repositoryPath, ObservableCollection<HistoryItem> tabsHistory)
        {
            this.browserPath = browserPath;
            this.repositoryPath = repositoryPath;

            TabsHistory = tabsHistory;
        }

        public Config()
        {

        }
    }


}
