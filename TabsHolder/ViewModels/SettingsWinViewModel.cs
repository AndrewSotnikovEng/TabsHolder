using System;
using System.ComponentModel;
using System.IO;
using TabsHolder.Commands;
using TabsHolder.Model;

namespace TabsHolder.ViewModels
{
    class SettingsWinViewModel : ViewModelBase, IDataErrorInfo
    {
        public string Path { get; set; }

        public int DefaultRatingValue { get; set; }

        public SettingsWinViewModel(SettingsViewBag viewBag)
        {
            SaveSettingsCmd = new RelayCommand(o => { SaveSettings(); }, SaveSettingsCanExecute);
            BrowseFolderPathCmd = new RelayCommand(o => { BrowseFolder(); }, (object arg) => true );

            Path = viewBag.RepositoryPath;
            DefaultRatingValue = viewBag.DefaultRatingValue;

        }

        private bool SaveSettingsCanExecute(object arg)
        {
            bool result = false;
        
            if (DefaultRatingValue >= 1 && DefaultRatingValue <= 10 &&  Directory.Exists(Path))
            {
                result = true;
            }

                return result;
        }

        private void SaveSettings()
        {
            SettingsViewBag viewBag = new SettingsViewBag
            {
                RepositoryPath = Path,
                DefaultRatingValue = this.DefaultRatingValue
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

                    case "DefaultRatingValue":

                        if (DefaultRatingValue < 1 || DefaultRatingValue > 10) error = "Value is out of range!";
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
