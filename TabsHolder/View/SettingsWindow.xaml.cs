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
using System.Windows.Forms;
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
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(SettingsViewBag viewBag)
        {
            InitializeComponent();
            DataContext = new SettingsWinViewModel(viewBag);

            MessengerStatic.BrowseFolderPathClicked += MessengerStatic_BrowseFolderPathClicked;
            MessengerStatic.SettingsWindowClosed += MessengerStatic_SettingsWindowClosed;

        }

        private void MessengerStatic_SettingsWindowClosed(object obj)
        {
            this.Close();
        }

        private void MessengerStatic_BrowseFolderPathClicked(object obj)
        {

            var dialog = new FolderBrowserDialog
            {
                Description = "Please select repository location",
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                ShowNewFolderButton = true
            };

            dialog.ShowDialog();
            FolderPathTextBox.Text = dialog.SelectedPath;
        }
    }
}
