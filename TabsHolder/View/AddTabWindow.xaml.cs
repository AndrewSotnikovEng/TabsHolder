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

    public partial class AddTabWindow : Window
    {
        public AddTabWindow()
        {
            
            InitializeComponent();
            DataContext = new AddTabWinViewModel();
        }


        void AddTabWindow_Closing(object sender, CancelEventArgs e)
        {
            MessengerStatic.NotifyAddTabWinClosing("Add tab have been closed");
        }
    }
}
