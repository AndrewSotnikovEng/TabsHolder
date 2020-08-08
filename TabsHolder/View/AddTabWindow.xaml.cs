using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TabsHolder
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddTabWindow : Window
    {
        public AddTabWindow()
        {
            InitializeComponent();

            

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

            TabItem tabItem = new TabItem(urlInput.Text, Int32.Parse(ratingInput.Text), 1);
            MessengerStatic.Send(tabItem);

        }


        void AddTabWindow_Closing(object sender, CancelEventArgs e)
        {
            MessengerStatic.NotifyAttTabClosing("Add tab have been closed");
            
        }


    }
}
