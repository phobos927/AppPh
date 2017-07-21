using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ColorControls
{
    /// <summary>
    /// Логика взаимодействия для ColoDialog.xaml
    /// </summary>
    public partial class ColorDialog : Window
    {
        public Color SelectColor { get; set; }
      

        public ColorDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            colorControl1.OkButton.Click += new RoutedEventHandler(OkButton_Click);
            colorControl1.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            SelectColor = new Color();
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectColor = Color.FromRgb(colorControl1.R, colorControl1.G, colorControl1.B);            
            this.Close();
        }

        private void colorControl1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (colorControl1.Width > 400)
            {
                this.Width = 490;
                this.MaxWidth = 490;
            }
        }
    }
}
