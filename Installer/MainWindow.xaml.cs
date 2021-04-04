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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Builds.Add(new Build("v0.1.0", "url1"));
            Builds.Add(new Build("v1.1", "url2"));

            InitializeComponent();
            DataContext = this;


        }

        public ObservableCollection<Build> Builds { get; set; } = new ObservableCollection<Build>();
    }
}
