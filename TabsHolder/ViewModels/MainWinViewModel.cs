using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder
{
    public class MainWinViewModel : INotifyPropertyChanged
    {
        public ApplicationContext db;
        private ObservableCollection<TabItem> tabItems = new ObservableCollection<TabItem>();
        private ObservableCollection<TabItem> initialTabItems = new ObservableCollection<TabItem>();
        private string filterWord;
        private bool checkAll;


        public MainWinViewModel()
        {

            db = new ApplicationContext();
            loadDbModels();
        }

        public ObservableCollection<TabItem> TabItems
        {
            get => tabItems;
            set
            {
                tabItems = value;
                NotifyPropertyChanged();
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
                filterWord = value;
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
            ObservableCollection<TabItem> tmpTabItems = tabItems;
            for (int i = 0; i < tabItems.Count; i++)
            {
                tmpTabItems.ElementAt(i).IsCheckedBoolean = isChecked;
            }
            TabItems = tmpTabItems;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
