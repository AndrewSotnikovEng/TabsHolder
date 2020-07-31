using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder
{
    public class MainWinViewModel
    {

        private ObservableCollection<TabItem> tabItems = new ObservableCollection<TabItem>();

        

        public MainWinViewModel()
        {


            TabItems.Add(new TabItem(@"https://www.strava.com/", 5));
            TabItems.Add(new TabItem(@"https://www.eway.in.ua/ru/cities/kyiv", 5));
            TabItems.Add(new TabItem(@"https://stackoverflow.com/questions/16642196/get-html-code-from-website-in-c-sharp", 5));

        }

        public ObservableCollection<TabItem> TabItems { get => tabItems; set => tabItems = value; }
    }
}
