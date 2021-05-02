using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TabsHolder.Model;
using TabsHolder.ViewModels;

namespace TabsHolder.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RecentlyUsedWindow : Window
    {
        public RecentlyUsedWindow(ObservableCollection<HistoryItem> TabsHistory)
        {
            InitializeComponent();
            DataContext = new RecentlyUsedWinViewModel(TabsHistory);

        }

        private void MouseDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ((RecentlyUsedWinViewModel)DataContext).ShareLastSession();
                this.Close();

            }
        }

    }
}
