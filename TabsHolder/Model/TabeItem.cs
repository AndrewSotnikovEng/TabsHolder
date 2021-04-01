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
        public TabItem(string url, int rating, int isChecked)
        {
            this.Url = url;
            this.Rating = rating;
            this.IsChecked = isChecked;
            this.Title = TabItemService.getUrlTitle(Url);
        }


        //more faster version, no need parsing
        public TabItem(string url, int rating, int isChecked, string title)
        {
            this.Url = url;
            this.Rating = rating;
            this.IsChecked = isChecked;
            this.Title = title;
        }


        public TabItem()
        {
        }

        public string Title { get; set; }


        public string Url { get; set; }
        public int Rating { get; set; }
        public int IsChecked { get; set; }

        [Key]
        public int ID { get; set; }

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

        public override bool Equals(object obj)
        {
            bool result = false;
            try
            {
                TabItem objToCompare = (TabItem)obj;
                if (Title == objToCompare.Title && Url == objToCompare.Url && Rating == objToCompare.Rating)
                {
                    result = true;
                }
            } catch (InvalidCastException e) { }
            

            return result;
        }
    }
}
