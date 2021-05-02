using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder.Data
{
    public class Session : Config
    {
        public ObservableCollection<TabItem> TabItems;

        public Session()
        {

        }

        public Session(Session session)
        {
            TabItems = new ObservableCollection<TabItem>();
            foreach (var item in session.TabItems)
            {
                TabItem newTabItem = new TabItem(item.Url, item.Rating, item.IsChecked, item.Title);
                TabItems.Add(newTabItem);
            }
            browserPath = session.browserPath;
        }

        public override bool Equals(object obj)
        {
            bool result = false;

            Session objToCompare = (Session)obj;
            if (TabItems.SequenceEqual(objToCompare.TabItems) 
                && 
                browserPath == objToCompare.browserPath)
            {
                result = true;
            }

            return result;
        }
    }
}
