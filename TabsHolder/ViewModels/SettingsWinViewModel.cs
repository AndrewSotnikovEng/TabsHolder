using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabsHolder.Commands;
using TabsHolder.Model;

namespace TabsHolder.ViewModels
{
    class SettingsWinViewModel : ViewModelBase, IDataErrorInfo
    {
        public string Path { get; set; }

        public SettingsWinViewModel(SettingsViewBag viewBag)
        {
            SaveSettingsCmd = new RelayCommand(o => { SaveSettings(); }, (object arg) => true );
            BrowseFolderPathCmd = new RelayCommand(o => { BrowseFolder(); }, (object arg) => true );

            Path = viewBag.RepositoryPath;

        }


        private void SaveSettings()
        {
            SettingsViewBag viewBag = new SettingsViewBag
            {
                RepositoryPath = Path
            };

            MessengerStatic.NotifySettingsWindowClosing(viewBag);
        }

        private void BrowseFolder()
        {
            MessengerStatic.NotifyBrowseFolderPathOpen();
        }


        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Path":
                        if (!Directory.Exists(Path)) error = "Directory not exist!";
                        break;
                }
                this.Error = error;

                return error;
            }
        }
        public string Error { get; set; }
        public RelayCommand SaveSettingsCmd { get; }
        public RelayCommand BrowseFolderPathCmd { get; }
    }
}
