using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabsHolder.Commands;

namespace TabsHolder.ViewModels
{
    class EditTabViewModel : ViewModelBase
    {
        private TabItem selectedTabItem;
        public EditTabViewModel()
        {
   
            OkBtnCmd = new RelayCommand(o => { RenameValue(); });
            CancelBtnCmd = new RelayCommand(o => { Cancel(); });

            MessengerStatic.TabItemAddedByEnter += MessengerStatic_TabItemAddedByEnter;
        }

        private void MessengerStatic_TabItemAddedByEnter(object obj)
        {
            RenameValue();
        }

        private void Cancel()
        {
            MessengerStatic.NotifyRenameTabWindowClosing();
        }

        private void RenameValue()
        {
            SelectedTabItem.Title = TabItemName;
            SelectedTabItem.Rating = RatingValue;
            MessengerStatic.NotifyTabItemNameChanging(SelectedTabItem);
            MessengerStatic.NotifyRenameTabWindowClosing();

        }

        public string TabItemName { get; set; }

        public int RatingValue { get; set; }
        public RelayCommand OkBtnCmd { get; }
        public RelayCommand CancelBtnCmd { get; }

        public TabItem SelectedTabItem { get => selectedTabItem; set
            {
                selectedTabItem = value;
                TabItemName = selectedTabItem.Title;
                RatingValue = selectedTabItem.Rating;
            }
        }
    }
}
