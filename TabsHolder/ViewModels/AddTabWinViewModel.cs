using TabsHolder.Commands;
using TabsHolder.ViewModels;

namespace TabsHolder
{
    public class AddTabWinViewModel : ViewModelBase
    {
        string url;
        int rating;

        public AddTabWinViewModel()
        {
            AddBtnClickCmd = new RelayCommand(o => { AddBtnСlick(); });
        }

        public string Url { get => url; set => url = value; }

        public int Rating
        {
            get => rating;
            set
            {
                System.Console.WriteLine(value.GetType());
                rating = value;
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

    }
}