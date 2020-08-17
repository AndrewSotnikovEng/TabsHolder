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

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var tabItemsList = (DataGrid)this.FindName("tabItemsList");
            for (int i = 0; i < mainWinViewModel.TabItems.Count; i++)
            {
                if (tabItemsList.SelectedItem == null) break;
                TabItem selectedItem = (TabItem)tabItemsList.SelectedItem;
                if (mainWinViewModel.TabItems.ElementAt(i).Title == selectedItem.Title)
                {
                    mainWinViewModel.db.tabItems.Remove(mainWinViewModel.TabItems.ElementAt(i));
                    mainWinViewModel.db.SaveChanges();
                    mainWinViewModel.loadDbModels();
                }
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            //get urls
            List<string> urls = new List<string>();
            foreach (TabItem item in mainWinViewModel.TabItems)
            {
                if (item.IsCheckedBoolean)
                {
                    urls.Add(item.Url);

                }
            }
            if (urls.Count == 0) return;

            //check browser location
            string browserPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            if (!File.Exists(browserPath))
            {
                // Create OpenFileDialog
                Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

                // Launch OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = openFileDlg.ShowDialog();
                // Get the selected file name and display in a TextBox.
                // Load content of file in a TextBlock
                if (result == true)
                {
                    browserPath = openFileDlg.FileName;
                }
            }


            //open in browser
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            foreach (string url in urls)
            {
                startInfo.Arguments = $"/C \"{browserPath}\" -new-tab -url {url}";
                process.StartInfo = startInfo;
                process.Start();
            }


        }

      


        



    }
}
