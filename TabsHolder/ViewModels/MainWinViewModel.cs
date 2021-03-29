using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using TabsHolder.Commands;
using TabsHolder.ViewModels;
using TabsHolder.View;
using TabsHolder.Services;
using TabsHolder.Data;

namespace TabsHolder
{
    public class MainWinViewModel : ViewModelBase
    {
        public ApplicationContext db;
        private ObservableCollection<TabItem> tabItems = new ObservableCollection<TabItem>();
        private string filterWord;
        public bool IsSessionLoaded { get; set; } = false;
        private ICollectionView tabItemsView;
        private string browserPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";

        private TabItem selectedItem;
        public string CurrentSesstion { get; set; }

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
            get { return this.selectedItem; }
            set
            {
                if (value != this.selectedItem)
                {
                    this.selectedItem = value;
                    this.OnPropertyChanged("SelectedItem");
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
            AddBtnClickCmd = new RelayCommand(o => { AddBtnСlick(); }, AddBtnClickCanExecute);
            RenameTabItemCmd = new RelayCommand(o => { RenameBtnСlick(); }, RenameBtnClickCanExecute);
            SaveConfigCmd = new RelayCommand(o => { SaveConfig(); });
            UnloadSessionCmd = new RelayCommand(o => { UnloadSession(); }, UnloadSessionCanExecute);
            OverwriteSessionCmd = new RelayCommand(o => { OverwriteSession(); }, OverwriteSessionCanExecute);
            

            MessengerStatic.AddTabWindowClosed += AddTabClosing;
            MessengerStatic.TabItemNameChanged += SelectedItemChanged;

        }

        private void OverwriteSession()
        {
            SaveSession(CurrentSesstion);
        }

        private bool OverwriteSessionCanExecute(object arg)
        {
            bool result = IsSessionLoaded ? true : false;

            return result;
        }

        private void SelectedItemChanged(object obj)
        {
            if (!IsSessionLoaded)
            {
                tabItemsView.Refresh();
                db.SaveChanges();
            }
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
            Session ses = XmlSerializerService.Deserialize(configFileName);
            browserPath = ses.browserPath;
        }

        public void LoadSession(string fileName)
        {
            CurrentSesstion = fileName;
            if (!File.Exists(fileName)) return;
            Session ses = XmlSerializerService.Deserialize(fileName);
            browserPath = ses.browserPath;
            TabItems = ses.TabItems;

            IsSessionLoaded = true;

            WireFilter();
        }

        public void SaveSession(string fileName)
        {
                Session ses = new Session();
                ses.browserPath = browserPath;
                ses.TabItems = TabItems;
                
                XmlSerializerService.Serialize(fileName, ses);
        }

        public RelayCommand UnloadSessionCmd
        {
            get;
            private set;
        }
        public RelayCommand OverwriteSessionCmd { get; }

        public void UnloadSession()
        {
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
            Session ses = new Session();
            ses.browserPath = browserPath;
            XmlSerializerService.Serialize("config.ses", ses);
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


    }
}
