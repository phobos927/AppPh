using System;
using System.Collections.Generic;
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

namespace AppPh
{
    /// <summary>
    /// Логика взаимодействия для newDialog.xaml
    /// </summary>
    public partial class newDialog : Window
    {
        public newDialog()
        {
            InitializeComponent();
        }
        public bool res = false;
        private void btrue_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void bfalse_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            res = true;
            DialogResult = true;
        }
    }
}
