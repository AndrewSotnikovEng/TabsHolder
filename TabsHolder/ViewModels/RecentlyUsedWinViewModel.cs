using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabsHolder.Model;

namespace TabsHolder.ViewModels
{
    class RecentlyUsedWinViewModel
    {
        public ObservableCollection<HistoryItem> TabsHistory { get; set; }
        public HistoryItem SelectedItem { get; set; }

        public RecentlyUsedWinViewModel(ObservableCollection<HistoryItem> tabsHistory)
        {
            TabsHistory = tabsHistory;
        }

        public void ShareLastSession()
        {
            MessengerStatic.NotifyLastSessionSelecting(SelectedItem);
        }
    }
}
