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
using TabsHolder.ViewModels;

namespace TabsHolder
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWinViewModel();
            MessengerStatic.AddTabWindowOpened += CreateAddTabWindow;
            MessengerStatic.RenameTabWindowOpened += CreateRenameTabWin;


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

        public void SaveSessionAs_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = $"Session_{DateTime.Now.ToString("ddmmhhmmss")}"; // Default file name
            dlg.DefaultExt = ".ses"; // Default file extension
            dlg.Filter = "Session files (.ses)|*.ses"; // Filter files by extension
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                ((MainWinViewModel)DataContext).SaveSession(dlg.FileName);
            }
            else
            {
                string message = "Please select file next time";
                string caption = "Info";
                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(message, caption, buttons, icon);
            }
        }

        public void LoadSession_Click(object senser, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Session files (.ses)|*.ses"; // Filter files by extension
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (openFileDialog.ShowDialog() == true)
            {
                ((MainWinViewModel)DataContext).LoadSession(openFileDialog.FileNames[0]);
            }
            else
            {
                string message = "Please select file next time";
                string caption = "Info";
                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(message, caption, buttons, icon);
            }
        }

        public void CreateAddTabWindow(object data)
        {

            AddTabWindow addTabWin = new AddTabWindow();
            addTabWin.Show();
        }

        public void CreateRenameTabWin(object selectedItem)
        {
            RenameTabWindow renameTabWindow = new RenameTabWindow();
            ((RenameTabViewModel)renameTabWindow.DataContext).SelectedTabItem = (TabItem)selectedItem;
            renameTabWindow.Show();
        }

    }
}
