using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        string title;


        public TabItem(string url, int rating, int isChecked)
        {
            this.Url = url;
            this.Rating = rating;
            this.IsChecked = isChecked;
        }

        public TabItem()
        {
        }

        public string Title
        {
            get
            {
                if (title == null)
                {
                    title = TabItemService.getUrlTitle(Url);
                }
                return title;
            }
        }


        public string Url { get => url; set => url = value; }
        public int Rating { get => rating; set => rating = value; }
        public int IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
            }
        }

        [Key]
        public int ID { get => id; set => id = value; }

        [NotMapped]
        public bool IsCheckedBoolean
        {
            get
            {
                return Convert.ToBoolean(IsChecked);
            }

            set
            {
                IsChecked = Convert.ToInt32(value);
            }
        }



    }
}
