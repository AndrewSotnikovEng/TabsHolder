using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using TabsHolder.View;
using TabsHolder.ViewModels;

namespace TabsHolder
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWinViewModel();
            MessengerStatic.AddTabWindowOpened += CreateAddTabWindow;
            MessengerStatic.RenameTabWindowOpened += CreateRenameTabWin;

            Closing += MainWindow_Closing;

        }


        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (((MainWinViewModel)DataContext).IsSessionChanged())
            {
                var result = MessageBox.Show("Session was changed, do you want to save it?", "Session changed", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MessengerStatic.NotifySessionOverwriting(null);
                }
            }
        }

        //to replace
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
            MainWinViewModel vm = ((MainWinViewModel)DataContext);
            vm.LoadConfig();
            if (vm.TabsHistory.Count > 0)
            {
                ShowRecetlryUsedWindow();
            }
        }

        private void RecentlyUsed_Click(object sender, EventArgs e)
        {
            ShowRecetlryUsedWindow();
        }


        private void ShowRecetlryUsedWindow()
        {
            RecentlyUsedWindow recentlyUsed = new RecentlyUsedWindow(
                        ((MainWinViewModel)DataContext).TabsHistory
            );
            recentlyUsed.Show();
        }

        public void CreateSession_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = $"Session_{DateTime.Now.ToString("ddmmhhmmss")}"; // Default file name
            dlg.DefaultExt = ".ses"; // Default file extension
            dlg.Filter = "Session files (.ses)|*.ses"; // Filter files by extension
            dlg.InitialDirectory = ((MainWinViewModel)DataContext).RepositoryPath;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                ((MainWinViewModel)DataContext).CreateSession(dlg.FileName);
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


        public void SaveSessionAs_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = $"Session_{DateTime.Now.ToString("ddmmhhmmss")}"; // Default file name
            dlg.DefaultExt = ".ses"; // Default file extension
            dlg.Filter = "Session files (.ses)|*.ses"; // Filter files by extension
            dlg.InitialDirectory = ((MainWinViewModel)DataContext).RepositoryPath;

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
            openFileDialog.InitialDirectory = ((MainWinViewModel)DataContext).RepositoryPath;
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
            int defaultRatingValue = ((MainWinViewModel)DataContext).DefaultRatingValue;
            AddTabWindow addTabWin = new AddTabWindow(defaultRatingValue);
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
