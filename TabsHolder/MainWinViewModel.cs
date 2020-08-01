using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder
{
    public class MainWinViewModel
    {
        public ApplicationContext db;
        private ObservableCollection<TabItem> tabItems = new ObservableCollection<TabItem>();


        

        public MainWinViewModel()
        {

            db = new ApplicationContext();
            loadDbModels();
        }

        public ObservableCollection<TabItem> TabItems { get => tabItems; set => tabItems = value; }

        public void loadDbModels()
        {
            db.tabItems.Load();
            var someList = db.tabItems.Local.ToBindingList();
            TabItems.Clear();
            foreach (TabItem item in someList)
            {
                TabItems.Add(item);
            }
        }
    }
}
