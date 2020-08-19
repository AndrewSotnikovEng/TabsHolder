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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWinViewModel mainWinViewModel = new MainWinViewModel();
        ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();


            this.DataContext = mainWinViewModel;
            MessengerStatic.CloseAddTabWindow += AddTabClosing;

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddTabWindow addTabWin = new AddTabWindow();
            addTabWin.Show();
            MessengerStatic.Bus += Receive;
        }



        private void Receive(object data)
        {
            if (data is TabItem)
            {
                TabItem tabItem = (TabItem)data;
                mainWinViewModel.db.tabItems.Add(tabItem);
                mainWinViewModel.db.SaveChanges();
                mainWinViewModel.loadDbModels();
                mainWinViewModel.InitialTabItems = mainWinViewModel.TabItems;
            }
        }


        private void AddTabClosing(object data)
        {
            MessengerStatic.Bus -= Receive;
        }


    }
}
