using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder
{
    public class TabItem
    {
        string url;
        int rating;

        public TabItem(string url, int rating)
        {
            this.Url = url;
            this.Rating = rating;
        }

        public string Title { get => TabItemService.getUrlTitle(url); }
        public string Url { get => url; set => url = value; }
        public int Rating { get => rating; set => rating = value; }
    }
}
