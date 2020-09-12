using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TabsHolder.ViewModels;

namespace TabsHolder
{

    public partial class RenameTabWindow : Window
    {
        public RenameTabWindow()
        {
            
            InitializeComponent();
            DataContext = new RenameTabViewModel();

            MessengerStatic.RenameTabWindowClosed += CloseWin;

        }

        private void CloseWin(object obj)
        {
            this.Close();
        }
    }
}
