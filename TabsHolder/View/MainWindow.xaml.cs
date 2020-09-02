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
            //for closing via File -> Exit
            MessengerStatic.CloseMainWindow += MainWindowClose;

        }

        void MainWindowClose(object obj)
        {
            this.Close();
        }

        //for closing via "X" sign
        void ClosingFromRightCorner(object sender, CancelEventArgs e)
        {
            ((MainWinViewModel)DataContext).SaveSession();
        }

        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            ((MainWinViewModel)DataContext).LoadLastSession();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }
    }
}
