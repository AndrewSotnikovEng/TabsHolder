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

        public string DefaultRatingValue { get; set; }

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
            int defaultRatingValue;
            try
            {
                defaultRatingValue = Int32.Parse(DefaultRatingValue);
            }
            catch (System.FormatException e)
            {
                return result;
            }
            if (defaultRatingValue >= 1 && defaultRatingValue <= 10 &&  Directory.Exists(Path))
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
                        int defaultRatingValue;
                        try
                        {
                            defaultRatingValue = Int32.Parse(DefaultRatingValue);
                        }
                        catch (System.FormatException e)
                        {
                            error = "Value is empty!";
                            break;
                        }
                        if (defaultRatingValue < 1 || defaultRatingValue > 10) error = "Value is out of range!";
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
