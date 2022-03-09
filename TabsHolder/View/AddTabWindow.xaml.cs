using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace TabsHolder
{

    public partial class AddTabWindow : Window
    {
        public AddTabWindow(int defaultRatingValue)
        {
            
            InitializeComponent();
            DataContext = new AddTabWinViewModel(defaultRatingValue);
            MessengerStatic.TabItemAdded += (obj) => this.Close();
        }


        void AddTabWindow_Closing(object sender, CancelEventArgs e)
        {
            MessengerStatic.NotifyAddTabWinClosing("Add tab have been closed");
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                this.Title = "Processing, please wait...";
                MessengerStatic.NotifyAboutTabItemAddingByEnter();
            }
        }
    }
}
