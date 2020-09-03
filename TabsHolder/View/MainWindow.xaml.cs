using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TabsHolder
{

    public partial class MainWindow : Window
    {

        MainWinViewModel mainWinViewModel = new MainWinViewModel();
        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = mainWinViewModel;

        }

        void MainWindowClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void BeforeClosingActions(object sender, CancelEventArgs e)
        {
            ((MainWinViewModel)DataContext).SaveConfig();
        }

        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            ((MainWinViewModel)DataContext).LoadConfig();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }
    }
}
