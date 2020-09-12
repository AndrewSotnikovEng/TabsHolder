using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabsHolder.Commands;

namespace TabsHolder.ViewModels
{
    class RenameTabViewModel : ViewModelBase
    {
        private TabItem selectedTabItem;
        public RenameTabViewModel()
        {
   
            OkBtnCmd = new RelayCommand(o => { RenameValue(); });
            CancelBtnCmd = new RelayCommand(o => { Cancel(); });
        }

        private void Cancel()
        {
            MessengerStatic.NotifyRenameTabWindowClosing();
        }

        private void RenameValue()
        {
            SelectedTabItem.Title = TabItemName;
            MessengerStatic.NotifyTabItemNameChanging(SelectedTabItem);
            MessengerStatic.NotifyRenameTabWindowClosing();

        }

        public string TabItemName { get; set; }
        public RelayCommand OkBtnCmd { get; }
        public RelayCommand CancelBtnCmd { get; }

        public TabItem SelectedTabItem { get => selectedTabItem; set
            {
                selectedTabItem = value;
                TabItemName = selectedTabItem.Title;
            }
        }
    }
}
