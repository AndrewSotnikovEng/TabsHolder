using System.ComponentModel;
using System.Text.RegularExpressions;
using TabsHolder.Commands;
using TabsHolder.ViewModels;

namespace TabsHolder
{
    public class AddTabWinViewModel : ViewModelBase, IDataErrorInfo
    {
        string url;
        int rating;

        //bool urlValidationState = false;
        //bool ratingValidationState = false;



        //public bool ButtonEnableState
        //{
        //    get
        //    {
        //        bool state = false;
        //        if (UrlValidationState && RatingValidationState)
        //        {
        //            state = true;
        //        }
        //        return state;
        //    }        
        //    set
        //    {
        //        OnPropertyChanged("ButtonEnableState");
        //    }
        //}


        public AddTabWinViewModel()
        {
            AddBtnClickCmd = new RelayCommand(o => { AddBtnСlick(); });
            


        }


        public string Url { get => url; 
        set
            {
                url = value;
                //string pattern = "^https?://.*";
                //var match = Regex.Match(value, pattern);
                //if (match.Success)
                //{
                //    UrlValidationState = true;
                //    ButtonEnableState = false;
                //}
            }
        }

        public int Rating
        {
            get => rating;
            set
            {
                rating = value;
                //if (value >=1 && value <= 10)
                //{
                //    RatingValidationState = true;
                //    ButtonEnableState = false;
                //}
            }
        }


        private void AddBtnСlick()
        {

            TabItem tabItem = new TabItem(Url, Rating, 0);
            MessengerStatic.Send(tabItem);

        }

        public RelayCommand AddBtnClickCmd
        {
            get;
            private set;
        }
        //public bool UrlValidationState { get => urlValidationState; set => urlValidationState = value; }
        //public bool RatingValidationState { get => ratingValidationState; set => ratingValidationState = value; }

        public string Error => null;

        public string this[string name]
        {
            get
            {
                string result = null;

                if (name == "Url")
                {
                    if (Url == null) return null;
                    string pattern = "^https?://.*";
                    var match = Regex.Match(Url, pattern);
                    if (!match.Success)
                    {
                        result = "Please provide http address";
                    }
                }
                if (name == "Rating")
                {
                    if (Rating < 1 || Rating > 10)
                    {
                        result = "Rating should be in range 1-10";
                    }
                }

                return result;
            }
        }

    }
}