using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using TabsHolder.Commands;
using TabsHolder.ViewModels;
using TabsHolder.View;
using TabsHolder.Services;
using TabsHolder.Data;
using System.Windows;
using TabsHolder.Model;

namespace TabsHolder
{
    public class MainWinViewModel : ViewModelBase
    {
        public ApplicationContext db;
        private ObservableCollection<TabItem> tabItems = new ObservableCollection<TabItem>();
        public ObservableCollection<HistoryItem> TabsHistory { get; set; } = new ObservableCollection<HistoryItem>();
        private string filterWord;
        public bool IsSessionLoaded { get; set; } = false;
        private ICollectionView tabItemsView;
        private string browserPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";

        string _repositoryPath;
        public string RepositoryPath
        {
            get
            {
                return (_repositoryPath == null) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : _repositoryPath;
            }
            set { _repositoryPath = value; }
        }

        string _defaultRatingValue;
        public string DefaultRatingValue
        {
            get
            {
                return (_defaultRatingValue == null) ? "7" : _defaultRatingValue;
            }
            set { _defaultRatingValue = value; }
        }

        private TabItem selectedItem;
        public string CurrentSessionPath { get; set; }

        public Session InitialSession { get; set; }
        public Session CurrentSession { get; set; }

        public string recentlySavedSession;

        public ObservableCollection<TabItem> TabItems
        {
            get 
            {
                return tabItems;
            }
            set
            {
                tabItems = value;
                OnPropertyChanged("TabItems");
            }
        }

        public TabItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != selectedItem)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                    if (selectedItem != null)
                    {
                        Clipboard.SetDataObject(selectedItem.Url);
                    }
                }
            }
        }

        public MainWinViewModel()
        {

            db = new ApplicationContext();
            LoadDbModels();
            WireFilter();

            DeleteTabItemCmd = new RelayCommand(o => { DeleteTabItem(); }, DeleteTabItemCanExecute);
            OpenInFirefoxCmd = new RelayCommand(o => { OpenInFirefox(); });
            OpenAboutWindowCmd = new RelayCommand(o => { OpenAboutWindow(); });
            OpenSettingsWindowCmd = new RelayCommand(o => { OpenSettingsWindow(); });
            AddBtnClickCmd = new RelayCommand(o => { AddBtnСlick(); }, AddBtnClickCanExecute);
            RenameTabItemCmd = new RelayCommand(o => { RenameBtnСlick(); }, RenameBtnClickCanExecute);
            SaveConfigCmd = new RelayCommand(o => { SaveConfig(); });
            UnloadSessionCmd = new RelayCommand(o => { UnloadSession(); }, UnloadSessionCanExecute);
            OverwriteSessionCmd = new RelayCommand(o => { OverwriteSession(); }, OverwriteSessionCanExecute);

            

            MessengerStatic.AddTabWindowClosed += AddTabClosing;
            MessengerStatic.TabItemNameChanged += SelectedItemChanged;
            MessengerStatic.SessionOverwrited += (obj) => OverwriteSession();
            MessengerStatic.LastSessionSelected += (obj) => LoadSession(((HistoryItem)obj).FullPath);
            MessengerStatic.SettingsWindowClosed += UpdateSettings;
        }

        private void UpdateSettings(object viewBag)
        {
            RepositoryPath = ((SettingsViewBag)viewBag).RepositoryPath;
            DefaultRatingValue = ((SettingsViewBag)viewBag).DefaultRatingValue;
        }

        public void CreateSession(string fileName)
        {
            TabItems.Clear();
            SaveSession(fileName);
            LoadSession(fileName);
        }


        private void OverwriteSession()
        {
            SaveSession(CurrentSessionPath);
        }

        private bool OverwriteSessionCanExecute(object arg)
        {
            bool result = IsSessionLoaded ? true : false;

            return result;
        }

        private void SelectedItemChanged(object obj)
        {
            tabItemsView.Refresh();
            db.SaveChanges();

        }

        public RelayCommand RenameTabItemCmd
        {
            get;
            private set;
        }
        private void RenameBtnСlick()
        {
            MessengerStatic.NotifyRenameTabWindowOpenning(SelectedItem);
        }

        private bool RenameBtnClickCanExecute(object arg)
        {
            //bool result = IsSessionLoaded ? false : true;

            return true;
        }


        private void WireFilter()
        {
            tabItemsView = CollectionViewSource.GetDefaultView(TabItems);
            tabItemsView.Filter = o => String.IsNullOrEmpty(FilterWord) ?
                    true : Regex.IsMatch(((TabItem)o).Title, $"{FilterWord}", RegexOptions.IgnoreCase);
        }

        public RelayCommand DeleteTabItemCmd
        {
            get;
            private set;
        }
        private void DeleteTabItem()
        {
            for (int i = 0; i < TabItems.Count; i++)
            {
                if (SelectedItem == null) break;
                if (TabItems.ElementAt(i).Title == SelectedItem.Title)
                {
                    if (!IsSessionLoaded) //for database
                    {
                        db.tabItems.Remove(TabItems.ElementAt(i));
                        db.SaveChanges();
                        LoadDbModels();
                    } else //for xml file
                    {
                        TabItems.Remove(TabItems.ElementAt(i));
                    }
                }
            }

        }
        private bool DeleteTabItemCanExecute(object arg)
        {
            //bool result = IsSessionLoaded ? false : true;

            return true;
        }

        public RelayCommand AddBtnClickCmd
        {
            get;
            private set;
        }

        
        private void AddBtnСlick()
        {
            MessengerStatic.NotifyAddTabWindowOpenning();
            MessengerStatic.TabItemAdded += AddTabItem;
        }

        private bool AddBtnClickCanExecute(object arg)
        {
            //bool result = IsSessionLoaded ? false : true;

            return true;
        }

        public string FilterWord
        {
            get
            {
                return filterWord;
            }
            set
            {
                if (value != filterWord)
                {
                    filterWord = value;
                    tabItemsView.Refresh();
                    OnPropertyChanged("FilterWord");
                }
            }
        }

        public ObservableCollection<TabItem> InitialTabItems { get; set; } = new ObservableCollection<TabItem>();
        public bool CheckAll { 
        get {
                return CheckAll;
            }

        set {
                ToggleCheckBoxes(value);
                Console.WriteLine("Do something");
            } 
        }

        private void ToggleCheckBoxes(bool isChecked)
        {
            ObservableCollection<TabItem> tmpTabItems = new ObservableCollection<TabItem>();
            foreach (TabItem item in TabItems)
            {
                tmpTabItems.Add(item);
            }

            for (int i = 0; i < TabItems.Count; i++)
            {
                tmpTabItems.ElementAt(i).IsCheckedBoolean = isChecked;
            }

            TabItems.Clear();
            foreach (var item in tmpTabItems)
            {
                TabItems.Add(item);
            }
        }

        public void LoadDbModels()
        {
            db.tabItems.Load();
            var someList = db.tabItems.Local.ToBindingList();
            TabItems.Clear();
            foreach (TabItem item in someList)
            {
                TabItems.Add(item);
            }
            InitialTabItems = TabItems;
        }


        public void LoadConfig()
        {
            string configFileName = "config.ses";
            if (!File.Exists(configFileName)) return;
            Config cfg = XmlSerializerService.DeserializeConfig(configFileName);
            browserPath = cfg.BrowserPath;
            RepositoryPath = cfg.RepositoryPath;
            DefaultRatingValue = cfg.DefaultRatingValue;
            TabsHistory = cfg.TabsHistory;
        }

        public void LoadSession(string fileName)
        {
            if (IsSessionChanged())
            {
                MessengerStatic.NotifySessionOverwriting(null);
            }

            CurrentSessionPath = fileName;
            if (!File.Exists(fileName)) return;
            CurrentSession = XmlSerializerService.DeserializeSession(fileName);
            browserPath = CurrentSession.BrowserPath;
            TabItems = CurrentSession.TabItems;

            IsSessionLoaded = true;
            InitialSession = new Session(CurrentSession);

            TabsHistory.Insert(0, new HistoryItem(CurrentSessionPath));
            CompressTabsHisotry();

            WireFilter();
        }

        public void SaveSession(string fileName)
        {
            CurrentSession= new Session();
            CurrentSession.BrowserPath = browserPath;
            CurrentSession.RepositoryPath = RepositoryPath;
            CurrentSession.TabItems = TabItems;
                
            XmlSerializerService.SerializeSeesion(fileName, CurrentSession);
        }

        public RelayCommand UnloadSessionCmd
        {
            get;
            private set;
        }
        public RelayCommand OverwriteSessionCmd { get; }

        public void UnloadSession()
        {
            if (IsSessionLoaded)
            {
                if (IsSessionChanged())
                {
                    MessengerStatic.NotifySessionOverwriting(null);
                }
                CurrentSession = null;
                InitialSession = null;
                CurrentSessionPath = null;
            }
            LoadDbModels();
            IsSessionLoaded = false;
            WireFilter();
        }
        private bool UnloadSessionCanExecute(object arg)
        {

            bool result = IsSessionLoaded ? true : false;

            return result;
        }

        public RelayCommand SaveConfigCmd
        {
            get;
            private set;
        }
        public void SaveConfig()
        {
            Config cfg = new Config();
            cfg.BrowserPath = browserPath;
            cfg.RepositoryPath = RepositoryPath;
            cfg.DefaultRatingValue = DefaultRatingValue;

            CompressTabsHisotry();
            cfg.TabsHistory = TabsHistory;

            XmlSerializerService.SerializeConfg("config.ses", cfg);
        }


        public RelayCommand OpenInFirefoxCmd
        {
            get;
            private set;
        }
        private void OpenInFirefox()
        {
            //get urls
            List<string> urls = new List<string>();
            foreach (TabItem item in TabItems)
            {
                if (item.IsCheckedBoolean)
                {
                    urls.Add(item.Url);

                }
            }
            if (urls.Count == 0) return;

            //check browser location
            if (!File.Exists(browserPath))
            {
                // Create OpenFileDialog
                Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

                // Launch OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = openFileDlg.ShowDialog();
                // Get the selected file name and display in a TextBox.
                // Load content of file in a TextBlock
                if (result == true)
                {
                    browserPath = openFileDlg.FileName;
                }
            }


            //open in browser
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            foreach (string url in urls)
            {
                startInfo.Arguments = $"/C \"{browserPath}\" -new-tab -url {url}";
                process.StartInfo = startInfo;
                process.Start();
            }
        }

        public RelayCommand OpenAboutWindowCmd
        {
            get;
            private set;
        }


        private void OpenAboutWindow()
        {
            AboutWindow about = new AboutWindow();
            about.Show();
        }

        public RelayCommand OpenSettingsWindowCmd
        {
            get;
            private set;
        }

        private void OpenSettingsWindow()
        {
            SettingsViewBag viewBag = new SettingsViewBag
            {
                RepositoryPath = this.RepositoryPath,
                DefaultRatingValue = this.DefaultRatingValue
            };
                
            SettingsWindow settings = new SettingsWindow(viewBag);
            settings.Show();
        }

        void AddTabItem(object data)
        {
            if (data is TabItem)
            {
                TabItem tabItem = (TabItem)data;
                if (!IsSessionLoaded) //for database
                {
                    db.tabItems.Add(tabItem);
                    db.SaveChanges();
                    LoadDbModels();
                    InitialTabItems = TabItems;
                } else //for xml
                {
                    TabItems.Add(tabItem);
                }

            }
        }

        private void AddTabClosing(object data)
        {
            MessengerStatic.TabItemAdded -= AddTabItem;
        }

        public bool IsSessionChanged()
        {
            bool result = false;
            if (CurrentSession == null || InitialSession == null)
            {
                return false;
            }
            result = (!CurrentSession.Equals(InitialSession)) ? true : false;

            return result;

        }

        public void CompressTabsHisotry()
        {
            List<HistoryItem> tmpHistory = TabsHistory.GroupBy(x => x.FullPath).Select(x => x.First()).ToList();
            TabsHistory = new ObservableCollection<HistoryItem>(tmpHistory);
        }

    }
}
