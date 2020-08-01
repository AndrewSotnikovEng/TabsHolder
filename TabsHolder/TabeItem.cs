using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder
{
    [Table("tab_items")]
    public class TabItem
    {

        int id;
        string url;
        int rating;
        int isChecked;
        bool isCheckedBoolean;


        public TabItem(string url, int rating, int isChecked)
        {
            this.Url = url;
            this.Rating = rating;
            this.IsChecked = isChecked;
        }

        public TabItem()
        {
        }

        public string Title { get => TabItemService.getUrlTitle(url); }
        public string Url { get => url; set => url = value; }
        public int Rating { get => rating; set => rating = value; }
        public int IsChecked { get; set; }

        [Key]
        public int ID { get => id; set => id = value; }

        [NotMapped]
        public bool IsCheckedBoolean { get => Convert.ToBoolean(IsChecked); 
            set => isCheckedBoolean = value; }
    }
}
