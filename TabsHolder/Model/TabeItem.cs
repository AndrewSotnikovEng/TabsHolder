using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder 
{
    [Table("tab_items")]
    public class TabItem : INotifyPropertyChanged
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
        public int IsChecked { get => isChecked;
            set
            {
                isChecked = value;
            }
            }

        [Key]
        public int ID { get => id; set => id = value; }

        [NotMapped]
        public bool IsCheckedBoolean { 
            get {
                return Convert.ToBoolean(IsChecked);
            }

            set { 
                isCheckedBoolean = value;
                NotifyPropertyChanged();
            }
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
