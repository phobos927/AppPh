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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using ColorControls;

namespace ColorControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ColorControl : UserControl
    {
        float red, green, blue;
        float hue, sat, lig;

        public byte H { get; set; }
        public byte S { get; set; }
        public byte L { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        Button[] BCS;
        int csi = 0;


        public ColorControl()
        {
            InitializeComponent();

            BCS = new Button[] { buttonCS0, buttonCS1, buttonCS2, buttonCS3, buttonCS4, buttonCS5, buttonCS6, buttonCS7, buttonCS8, buttonCS9, buttonCS10, buttonCS11, buttonCS12, buttonCS13, buttonCS14, buttonCS15 };
        }



        void ChangeRGB()
        {
            float h, s, l;
            float r = red;
            float g = green;
            float b = blue;
            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));


            if (max == r)
                if (g < b)
                    h = (g - b) / (6 * (max - min));
                else
                    h = 1 + (g - b) / (6 * (max - min));
            else if (max == g)
                h = 1.0f / 3 + (b - r) / (6 * (max - min));
            else //(max == b)
                h = 2.0f / 3 + (r - g) / (6 * (max - min));

            s = (max - min) / (1 - Math.Abs(1 - (max + min)));
            l = (max + min) / 2;
            hue = h;
            sat = s;
            lig = l;
        }


        void ChangeHSV()
        {

            float Q = (lig < 0.5) ? (lig * (1 + sat)) : (lig + sat - lig * sat);
            float P = 2 * lig - Q;
            float Tr = hue + 1.0f / 3;
            float Tg = hue;
            float Tb = hue - 1.0f / 3;

            Tr = (Tr < 0) ? (Tr + 1) : ((Tr > 1) ? (Tr - 1) : Tr);
            Tg = (Tg < 0) ? (Tg + 1) : ((Tg > 1) ? (Tg - 1) : Tg);
            Tb = (Tb < 0) ? (Tb + 1) : ((Tb > 1) ? (Tb - 1) : Tb);


            if (Tr < 1.0 / 6)
                red = P + ((Q - P) * 6 * Tr);
            else if (Tr < 1.0 / 2)
                red = Q;
            else if (Tr < 2.0 / 3)
                red = P + ((Q - P) * 6 * ((2.0f / 3) - Tr));
            else
                red = P;

            if (Tg < 1.0 / 6)
                green = P + ((Q - P) * 6 * Tg);
            else if (Tg < 1.0 / 2)
                green = Q;
            else if (Tg < 2.0 / 3)
                green = P + ((Q - P) * 6 * ((2.0f / 3) - Tg));
            else
                green = P;


            if (Tb < 1.0 / 6)
                blue = P + ((Q - P) * 6 * Tb);
            else if (Tb < 1.0 / 2)
                blue = Q;
            else if (Tb < 2.0 / 3)
                blue = P + ((Q - P) * 6 * ((2.0f / 3) - Tb));
            else
                blue = P;
        }

        bool RGBfreez = false;
        bool HSVfreez = false;

        private void BlueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!RGBfreez)
            {
                HSVfreez = true;
                blue = B / 256.0f;
                ChangeRGB();
                updateHSV();
                buttonColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
                HSVfreez = false;
            }
        }

        private void GreenTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!RGBfreez)
            {
                HSVfreez = true;
                green = G / 256.0f;
                ChangeRGB();
                updateHSV();
                buttonColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
                HSVfreez = false;
            }
        }

        private void RedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!RGBfreez)
            {
                HSVfreez = true;
                red = R / 256.0f;
                ChangeRGB();
                updateHSV();
                buttonColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
                HSVfreez = false;
            }
        }

        private void HTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!HSVfreez)
            {
                RGBfreez = true;
                if (H >= 240)
                {
                    H = 239;
                    HTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                }
                hue = H / 240.0f;
                ChangeHSV();
                updateRGB();
                buttonColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
                RGBfreez = false;
            }
        }

        private void STextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!HSVfreez)
            {
                RGBfreez = true;
                if (S > 240)
                {
                    S = 240;
                    STextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                }
                sat = S / 240.0f;
                ChangeHSV();
                updateRGB();
                buttonColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
                RGBfreez = false;
            }
        }

        private void LTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!HSVfreez)
            {
                RGBfreez = true;
                if (L > 240)
                {
                    L = 240;
                    LTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                }
                lig = L / 240.0f;
                ChangeHSV();
                updateRGB();
                buttonColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
                RGBfreez = false;
            }
        }

        void updateRGB()
        {
            R = TruncateFloat(red * 256, 0, 255);
            G = TruncateFloat(green * 256, 0, 255);
            B = TruncateFloat(blue * 256, 0, 255);
            RedTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            GreenTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            BlueTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        byte TruncateFloat(float value, byte min, byte max)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return (byte)value;
        }

        void updateHSV()
        {
            H = TruncateFloat(hue * 240, 0, 239);
            S = TruncateFloat(sat * 240, 0, 240);
            L = TruncateFloat(lig * 240, 0, 240);
            HTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            STextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            LTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetCross(e.GetPosition(image1));
        }

        private void image1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SetCross(e.GetPosition(image1));
            }
        }

        void SetCross(Point p)
        {
            CrossTranslate.X = p.X / image1.ActualWidth;
            CrossTranslate.Y = p.Y / image1.ActualWidth;
            if (!HSVfreez)
            {
                RGBfreez = true;
                hue = (float)CrossTranslate.X;
                sat = 1 - (float)CrossTranslate.Y;
                if (hue < 0.001) hue = 0.001f;
                if (sat < 0.001) sat = 0.001f;
                if (hue >= 1) hue = 0.999f;
                if (sat >= 1) sat = 0.999f;
                H = (byte)(hue * 240);
                S = (byte)(sat * 240);
                HSVfreez = true;
                HTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                STextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                HSVfreez = false;

                ChangeHSV();
                updateRGB();
                buttonColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
                GradientStop.Color = GetMaxColor();
                RGBfreez = false;
            }
        }

        Color GetMaxColor()
        {
            float r, g, b;

            float lig = 0.5f;

            float Q = (lig < 0.5) ? (lig * (1 + sat)) : (lig + sat - lig * sat);
            float P = 2 * lig - Q;
            float Tr = hue + 1.0f / 3;
            float Tg = hue;
            float Tb = hue - 1.0f / 3;

            Tr = (Tr < 0) ? (Tr + 1) : ((Tr > 1) ? (Tr - 1) : Tr);
            Tg = (Tg < 0) ? (Tg + 1) : ((Tg > 1) ? (Tg - 1) : Tg);
            Tb = (Tb < 0) ? (Tb + 1) : ((Tb > 1) ? (Tb - 1) : Tb);


            if (Tr < 1.0 / 6)
                r = P + ((Q - P) * 6 * Tr);
            else if (Tr < 1.0 / 2)
                r = Q;
            else if (Tr < 2.0 / 3)
                r = P + ((Q - P) * 6 * ((2.0f / 3) - Tr));
            else
                r = P;

            if (Tg < 1.0 / 6)
                g = P + ((Q - P) * 6 * Tg);
            else if (Tg < 1.0 / 2)
                g = Q;
            else if (Tg < 2.0 / 3)
                g = P + ((Q - P) * 6 * ((2.0f / 3) - Tg));
            else
                g = P;


            if (Tb < 1.0 / 6)
                b = P + ((Q - P) * 6 * Tb);
            else if (Tb < 1.0 / 2)
                b = Q;
            else if (Tb < 2.0 / 3)
                b = P + ((Q - P) * 6 * ((2.0f / 3) - Tb));
            else
                b = P;

            return Color.FromScRgb(1, r, g, b);
        }

        void SetTriangle(float Y)
        {
            TriangleTransform.Y = Y;
            if (!HSVfreez)
            {
                RGBfreez = true;
                lig = (1 - Y / 189);
                if (lig < 0.001) lig = 0.001f;
                if (lig >= 1) lig = 0.999f;
                L = (byte)(lig * 240);
                HSVfreez = true;
                LTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                HSVfreez = false;
                ChangeHSV();
                updateRGB();
                buttonColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
                RGBfreez = false;
            }
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTriangle((float)e.GetPosition(image2).Y);
        }

        private void image2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SetTriangle((float)e.GetPosition(image2).Y);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = 230;
            Grid1.Width = 230;
            Grid1.MaxWidth = 230;
            image1.Visibility = System.Windows.Visibility.Collapsed;
            image2.Visibility = System.Windows.Visibility.Collapsed;
            image3.Visibility = System.Windows.Visibility.Collapsed;
            button65.Visibility = System.Windows.Visibility.Collapsed;
            buttonColor.Visibility = System.Windows.Visibility.Collapsed;
            HTextBox.Visibility = System.Windows.Visibility.Collapsed;
            STextBox.Visibility = System.Windows.Visibility.Collapsed;
            LTextBox.Visibility = System.Windows.Visibility.Collapsed;
            RedTextBox.Visibility = System.Windows.Visibility.Collapsed;
            GreenTextBox.Visibility = System.Windows.Visibility.Collapsed;
            BlueTextBox.Visibility = System.Windows.Visibility.Collapsed;
            label3.Visibility = System.Windows.Visibility.Collapsed;
            label4.Visibility = System.Windows.Visibility.Collapsed;
            label5.Visibility = System.Windows.Visibility.Collapsed;
            label6.Visibility = System.Windows.Visibility.Collapsed;
            label7.Visibility = System.Windows.Visibility.Collapsed;
            label8.Visibility = System.Windows.Visibility.Collapsed;
            label9.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 466;
            this.MinWidth = 466;
            Grid1.Width = 466;
            Grid1.MaxWidth = 466;
            image1.Visibility = System.Windows.Visibility.Visible;
            image2.Visibility = System.Windows.Visibility.Visible;
            image3.Visibility = System.Windows.Visibility.Visible;
            button65.Visibility = System.Windows.Visibility.Visible;
            buttonColor.Visibility = System.Windows.Visibility.Visible;
            HTextBox.Visibility = System.Windows.Visibility.Visible;
            STextBox.Visibility = System.Windows.Visibility.Visible;
            LTextBox.Visibility = System.Windows.Visibility.Visible;
            RedTextBox.Visibility = System.Windows.Visibility.Visible;
            GreenTextBox.Visibility = System.Windows.Visibility.Visible;
            BlueTextBox.Visibility = System.Windows.Visibility.Visible;
            label3.Visibility = System.Windows.Visibility.Visible;
            label4.Visibility = System.Windows.Visibility.Visible;
            label5.Visibility = System.Windows.Visibility.Visible;
            label6.Visibility = System.Windows.Visibility.Visible;
            label7.Visibility = System.Windows.Visibility.Visible;
            label8.Visibility = System.Windows.Visibility.Visible;
            label9.Visibility = System.Windows.Visibility.Visible;
            ChangeColor.IsEnabled = false;
        }

        private void SetColorButton_Click(object sender, RoutedEventArgs e)
        {
            Color c = (((Button)sender).Background as SolidColorBrush).Color;
            buttonColor.Background = new SolidColorBrush(c);
            red = c.ScR;
            green = c.ScG;
            blue = c.ScB;
            ChangeRGB();
            CrossTranslate.X = hue;
            CrossTranslate.Y = 1 - sat;
            if (CrossTranslate.X >= 1) CrossTranslate.X = 0.999;
            if (CrossTranslate.X <= 0) CrossTranslate.X = 0.001;
            if (CrossTranslate.Y >= 1) CrossTranslate.Y = 0.999;
            if (CrossTranslate.Y <= 0) CrossTranslate.Y = 0.001;


            TriangleTransform.Y = (1 - lig) * 189;
            if (TriangleTransform.Y > 189) TriangleTransform.Y = 189;
            if (TriangleTransform.Y < 1) TriangleTransform.Y = 1;

            RGBfreez = true;
            HSVfreez = true;
            updateRGB();
            updateHSV();
            HSVfreez = false;
            RGBfreez = false;
        }

        private void button65_Click(object sender, RoutedEventArgs e)
        {
            BCS[csi].Background = buttonColor.Background.Clone();
            csi++;
            if (csi > 15) csi = 0;
        }


    }
}
