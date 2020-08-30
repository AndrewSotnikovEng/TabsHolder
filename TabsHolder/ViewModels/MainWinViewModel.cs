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

namespace TabsHolder
{
    public class MainWinViewModel : ViewModelBase
    {
        public ApplicationContext db;
        private ObservableCollection<TabItem> tabItems = new ObservableCollection<TabItem>();
        private ObservableCollection<TabItem> initialTabItems = new ObservableCollection<TabItem>();
        private string filterWord;
        private bool checkAll;
        private ICollectionView tabItemsView;

        private TabItem selectedItem;

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
            loadDbModels();
            tabItemsView = CollectionViewSource.GetDefaultView(TabItems);
            tabItemsView.Filter = o => String.IsNullOrEmpty(FilterWord) ? 
                    true : Regex.IsMatch(((TabItem)o).Title, $"{FilterWord}", RegexOptions.IgnoreCase);

            DeleteTabItemCmd = new RelayCommand(o => { DeleteTabItem(); });
            OpenInFirefoxCmd = new RelayCommand(o => { OpenInFirefox(); });
            OpenAboutWindowCmd = new RelayCommand(o => { OpenAboutWindow(); });
            AddBtnClickCmd = new RelayCommand(o => { AddBtnСlick(); });


            MessengerStatic.CloseAddTabWindow += AddTabClosing;
        }

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

        public ObservableCollection<TabItem> InitialTabItems { get => initialTabItems; set => initialTabItems = value; }
        public bool CheckAll { 
        get {
                return checkAll;
            }

        set {
                toggleCheckBoxes(value);
                Console.WriteLine("Do something");
            } 
        }

        private void toggleCheckBoxes(bool isChecked)
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


        public void loadDbModels()
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



        private void DeleteTabItem()
        {
            for (int i = 0; i < TabItems.Count; i++)
            {
                if (SelectedItem == null) break;
                if (TabItems.ElementAt(i).Title == SelectedItem.Title)
                {
                    db.tabItems.Remove(TabItems.ElementAt(i));
                    db.SaveChanges();
                    loadDbModels();
                }
            }
        }

        private void AddBtnСlick()
        {
            AddTabWindow addTabWin = new AddTabWindow();
            addTabWin.Show();
            MessengerStatic.Bus += Receive;
        }


        public RelayCommand DeleteTabItemCmd
        {
            get;
            private set;
        }

        public RelayCommand OpenInFirefoxCmd
        {
            get;
            private set;
        }

        public RelayCommand OpenAboutWindowCmd
        {
            get;
            private set;
        }

        public RelayCommand AddBtnClickCmd
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
            string browserPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
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

        private void OpenAboutWindow()
        {
            AboutWindow about = new AboutWindow();
            about.Show();
        }

        private void Receive(object data)
        {
            if (data is TabItem)
            {
                TabItem tabItem = (TabItem)data;
                db.tabItems.Add(tabItem);
                db.SaveChanges();
                loadDbModels();
                InitialTabItems = TabItems;
            }
        }

        private void AddTabClosing(object data)
        {
            MessengerStatic.Bus -= Receive;
        }
    }
}
