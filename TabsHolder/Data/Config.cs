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
        public string BrowserPath { get; set; }
        public string RepositoryPath { get; set; }
        public int DefaultRatingValue { get; set; }

        public ObservableCollection<HistoryItem> TabsHistory;

        public Config(string browserPath, string repositoryPath, int defaultRatingValue, ObservableCollection<HistoryItem> tabsHistory)
        {
            this.BrowserPath = browserPath;
            this.RepositoryPath = repositoryPath;
            this.DefaultRatingValue = defaultRatingValue;

            TabsHistory = tabsHistory;
        }

        public Config()
        {

        }
    }


}
