using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TabsHolder.ViewModels;

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


        public MainWinViewModel()
        {

            db = new ApplicationContext();
            loadDbModels();
            tabItemsView = CollectionViewSource.GetDefaultView(TabItems);
            tabItemsView.Filter = o => String.IsNullOrEmpty(FilterWord) ? true : ((TabItem)o).Title.Contains(FilterWord);
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


    }

}
