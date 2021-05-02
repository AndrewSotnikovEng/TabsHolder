using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabsHolder.Commands;
using TabsHolder.Model;

namespace TabsHolder.ViewModels
{
    class RecentlyUsedWinViewModel
    {
        public ObservableCollection<HistoryItem> TabsHistory { get; set; }
        public RelayCommand OpenSelectedSessionCmd { get; }
        public HistoryItem SelectedItem { get; set; }

        public RecentlyUsedWinViewModel(ObservableCollection<HistoryItem> tabsHistory)
        {
            TabsHistory = tabsHistory;
            OpenSelectedSessionCmd = new RelayCommand(o => { ShareLastSession(); }, OpenSelectedSessionCanExecute);
        }

        private bool OpenSelectedSessionCanExecute(object arg)
        {
            bool result = SelectedItem != null ? true : false;

            return result;
        }

        public void ShareLastSession()
        {
            MessengerStatic.NotifyLastSessionSelecting(SelectedItem);
        }
    }
}
