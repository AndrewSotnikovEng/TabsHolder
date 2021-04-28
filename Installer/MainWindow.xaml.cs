using System;
using System.Windows;
using System.Windows.Forms;

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            MessengerStatic.OutputFolderNotEmpty += MessengerStatic_OutputFolderNotEmpty;
        }

        private void MessengerStatic_OutputFolderNotEmpty(object obj)
        {
            System.Windows.MessageBox.Show($"Folder {obj.ToString()} is not empty!", "Installer error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            ((MainWindowViewModel)DataContext).OutputFolder = dialog.SelectedPath;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

