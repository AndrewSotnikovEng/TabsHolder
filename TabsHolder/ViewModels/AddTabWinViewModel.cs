using System.ComponentModel;
using System.Text.RegularExpressions;
using TabsHolder.Commands;
using TabsHolder.ViewModels;

namespace TabsHolder
{
    public class AddTabWinViewModel : ViewModelBase, IDataErrorInfo
    {

   
        public AddTabWinViewModel()
        {
            AddBtnClickCmd = new RelayCommand(o => { AddBtnСlick(); }, CanExecute);
        }


        public string Url { get; set; }

        public int Rating { get; set; }


        private void AddBtnСlick()
        {
            TabItem tabItem = new TabItem(Url, Rating, 0);
            MessengerStatic.NotifyAboutTabItemAdding(tabItem);
        }

        public RelayCommand AddBtnClickCmd
        {
            get;
            private set;
        }

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

        public bool CanExecute(object parameter)
        {
            bool result = false;

            bool urlIsValid = false;
            bool ratingIsValid = false;

            string pattern = "^https?://.*";
            if (Url == null) return false;
            var match = Regex.Match(Url, pattern);
            if (match.Success)
            {
                urlIsValid = true;
            }

            if (Rating >= 1 && Rating <= 10)
            {
                ratingIsValid = true;
            }

            if (urlIsValid && ratingIsValid)
            {
                result = true;
            }

            return result;
        }
    }



}
