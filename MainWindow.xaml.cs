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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Markup.Localizer;
using System.IO;
using Microsoft.Win32;
using ColorControls;
using System.Data.SqlClient;
using System.Windows.Ink;
using System.IO.Compression;

namespace AppPh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

      
        private String[] IMAGES = {"def/noimage.png"};
        private String[] IMAGES1 = { "def/noimage.png" };
        private String[] IMAGES2 = { "def/noimage.png" };
        private String[] IMAGES3 = { "def/noimage.png" };
        private String[] IMAGES4 = { "def/noimage.png" };


        private static double IMAGE_WIDTH = 128;        // Image Width
        private static double IMAGE_HEIGHT = 128;       // Image Height        
        private static double SPRINESS = 0.4;		    // Control the Spring Speed
        private static double DECAY = 0.5;			    // Control the bounce Speed
        private static double SCALE_DOWN_FACTOR = 0.1;  // Scale between images
        private static double OFFSET_FACTOR = 100;      // Distance between images
        private static double OPACITY_DOWN_FACTOR = 0.3;    // Alpha between images
        private static double MAX_SCALE = 0.5;            // Maximum Scale
        private int typep;
        private int i1,i2,i3,i4,i5,x1,x2,x3,x4,x5,y1,y2,y3,y4,y5,r1,r2,r3,r4,r5,t1,t2,t3,t4,t5;
        private bool color_ch = false;
        private double _xCenter;
        private double _yCenter;
        private int selim, cheB , cheB1;
        private bool wellcome1 = true;
        private double _target = 0;		// Target moving position
        private double _current = 0;	// Current position
        private double _spring = 0;		// Temp used to store last moving 
        private List<Image> _images = new List<Image>();	// Store the added images        
        private static int FPS = 24;                // fps of the on enter frame event
        private DispatcherTimer _timer = new DispatcherTimer(); // on enter frame simulator
        string fPath = @"lib/new.edm";
        byte colorRed, colorGreen, colorBlue;
        private bool notdo = false;
        private bool rezh = false;
        private bool twochek = false;
        //Изменение цвета кисти
        public class ColorRGB
        {
            public byte red { get; set; }
            public byte green { get; set; }
            public byte blue { get; set; }
        }
        public ColorRGB mcolor { get; set; }      
        public Color clr { get; set; }
        public Color bcolor { get; set; }
        //цвет кисти - конец


        public MainWindow()
        {
            InitializeComponent();
            //Зададим все i = -1, чтобы не сохранялись не выбранные элементы
            i1 = -1;
            i2 = -1;
            i3 = -1;
            i4 = -1;
            i5 = -1;           
            // Save the center position
            _xCenter = Width / 2;
            _yCenter = Height / 10;
            //цвет кисти начало
            mcolor = new ColorRGB();
            mcolor.red = 0;
            mcolor.green = 0;
            mcolor.blue = 0;
            this.lbl1.Background = new SolidColorBrush(Color.FromRgb(mcolor.red, mcolor.green, mcolor.blue));
            //цвет кисти - конец
        }
        //цветRGB
        private void sld_Color_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            string crlName = slider.Name; //Определяем имя контрола, который покрутили
            double value = slider.Value; // Получаем значение контрола
                                         //В зависимости от выбранного контрола, меняем ту или иную компоненту и переводим ее в тип byte
            if (crlName.Equals("sld_RedColor"))
            {
                mcolor.red = Convert.ToByte(value);
            }
            if (crlName.Equals("sld_GreenColor"))
            {
                mcolor.green = Convert.ToByte(value);
            }
            if (crlName.Equals("sld_BlueColor"))
            {
                mcolor.blue = Convert.ToByte(value);
            }

            //Задаем значение переменной класса clr 
            clr = Color.FromRgb(mcolor.red, mcolor.green, mcolor.blue);
            //Устанавливаем фон для контрола Label 
            this.lbl1.Background = new SolidColorBrush(Color.FromRgb(mcolor.red, mcolor.green, mcolor.blue));
            el_size.Fill = lbl1.Background;
            // Задаем цвет кисти для контрола InkCanvas
            this.inkCanvas1.DefaultDrawingAttributes.Color = clr;
        }
        //конецRGB

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            //запрет копий
            string processName;
            System.Diagnostics.Process activePro = System.Diagnostics.Process.GetCurrentProcess();
            processName = activePro.ProcessName;
            if (System.Diagnostics.Process.GetProcessesByName(processName).Length > 1)
            {
                mainWindow.Visibility = Visibility.Hidden;
                MessageBox.Show("Приложение уже запущено "+ processName);
                twochek = true;
                Close();                
               // return;               
            }
            else
            {               
            }
            ///запрет копий конец
            Canv_down.Visibility = Visibility.Hidden;
            //  ink_vkl();
            
            Start();          
            uzip();
            loadmenu();
            IMAGES = Directory.GetFiles(@"lib/face/");
            IMAGES1 = Directory.GetFiles(@"lib/eyes/");
            IMAGES2 = Directory.GetFiles(@"lib/nose/");
            IMAGES3 = Directory.GetFiles(@"lib/ears/");
            IMAGES4 = Directory.GetFiles(@"lib/mouth/");
            defLoad();


        }

        private void moveClear(int value)
        {
            _target += value;
           

        }

        public void MovImg()
        {
            Canva.MouseMove += (ss, ee) =>
            {
                if ((ee.LeftButton == MouseButtonState.Pressed) & (typep == 1))
                {

                    Point temp = ee.GetPosition(this);
                    Point res = new Point(firstPoint.X - temp.X, firstPoint.Y - temp.Y);
                    Canvas.SetLeft(imgFace, Canvas.GetLeft(imgFace) - res.X);
                    Canvas.SetTop(imgFace, Canvas.GetTop(imgFace) - res.Y);
                    firstPoint = temp;
                }

                if ((ee.LeftButton == MouseButtonState.Pressed) & (typep == 2))
                {

                    Point temp = ee.GetPosition(this);
                    Point res = new Point(firstPoint.X - temp.X, firstPoint.Y - temp.Y);
                    Canvas.SetLeft(imgEyes, Canvas.GetLeft(imgEyes) - res.X);
                    Canvas.SetTop(imgEyes, Canvas.GetTop(imgEyes) - res.Y);
                    firstPoint = temp;
                }

                if ((ee.LeftButton == MouseButtonState.Pressed) & (typep == 3))
                {

                    Point temp = ee.GetPosition(this);
                    Point res = new Point(firstPoint.X - temp.X, firstPoint.Y - temp.Y);
                    Canvas.SetLeft(imgNose, Canvas.GetLeft(imgNose) - res.X);
                    Canvas.SetTop(imgNose, Canvas.GetTop(imgNose) - res.Y);
                    firstPoint = temp;
                }

                if ((ee.LeftButton == MouseButtonState.Pressed) & (typep == 4))
                {

                    Point temp = ee.GetPosition(this);
                    Point res = new Point(firstPoint.X - temp.X, firstPoint.Y - temp.Y);
                    Canvas.SetLeft(imgEars, Canvas.GetLeft(imgEars) - res.X);
                    Canvas.SetTop(imgEars, Canvas.GetTop(imgEars) - res.Y);
                    firstPoint = temp;
                }

                if ((ee.LeftButton == MouseButtonState.Pressed) & (typep == 5))
                {

                    Point temp = ee.GetPosition(this);
                    Point res = new Point(firstPoint.X - temp.X, firstPoint.Y - temp.Y);
                    Canvas.SetLeft(imgMouth, Canvas.GetLeft(imgMouth) - res.X);
                    Canvas.SetTop(imgMouth, Canvas.GetTop(imgMouth) - res.Y);
                    firstPoint = temp;
                }
            };
        }

        //Перетаскивание
        Point firstPoint = new Point();


        public void INIT()
        {
            MouseEventArgs e;
            
            Canva.MouseLeftButtonDown += (ss, ee) =>
            {
                firstPoint = ee.GetPosition(this);
                Canva.CaptureMouse();
            };

            Canva.MouseMove += (ss, ee) => { MovImg(); };
            Canva.MouseUp += (ss, ee) => { Canva.ReleaseMouseCapture(); };
           
        } 
        //конец перетаскивания






        /////////////////////////////////////////////////////        
        // Handlers 
        /////////////////////////////////////////////////////	

        // reposition the images
        void _timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < _images.Count; i++)
            {
                Image image = _images[i];
                posImage(image, i);
            }


            // compute the current position
            // added spring effect
            if (_target == _images.Count)
                _target = 0;
            _spring = (_target - _current) * SPRINESS + _spring * DECAY;
            _current += _spring;
        }

        /////////////////////////////////////////////////////        
        // Private Methods 
        /////////////////////////////////////////////////////	


        // add images to the stage
        private void addImages()
        {
           
            _images.Clear();
            for (int i = 0; i < IMAGES.Length; i++)
            {
                // get the image resources from the xap
                string url;
                url = Environment.CurrentDirectory + "/" + IMAGES[i];

                Image image = new Image();
                image.Height = 128;
                image.Width = 128;
                image.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));

                // add and reposition the image
                LayoutRoot.Children.Add(image);
                posImage(image, i);
                _images.Add(image);
            }
        }

        private void addImages1()
        {
           

            _images.Clear();

            for (int i = 0; i < IMAGES1.Length; i++)
            {
                // get the image resources from the xap
                string url;
                url = Environment.CurrentDirectory + "/" + IMAGES1[i];


                Image image = new Image();
                image.Height = 128;
                image.Width = 128;
                image.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));

                // add and reposition the image
                LayoutRoot.Children.Add(image);
                posImage(image, i);


                _images.Add(image);
            }
        }

        ///////////
        private void addImages2()
        {
           

            _images.Clear();

            for (int i = 0; i < IMAGES2.Length; i++)
            {
                // get the image resources from the xap
                string url;
                url = Environment.CurrentDirectory + "/" + IMAGES2[i];


                Image image = new Image();
                image.Height = 128;
                image.Width = 128;
                image.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));

                // add and reposition the image
                LayoutRoot.Children.Add(image);
                posImage(image, i);


                _images.Add(image);
            }
        }

        private void addImages3()
        {
          

            _images.Clear();

            for (int i = 0; i < IMAGES3.Length; i++)
            {
                // get the image resources from the xap
                string url;
                url = Environment.CurrentDirectory + "/" + IMAGES3[i];


                Image image = new Image();
                image.Height = 128;
                image.Width = 128;
                image.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));

                // add and reposition the image
                LayoutRoot.Children.Add(image);
                posImage(image, i);


                _images.Add(image);
            }
        }

        private void addImages4()
        {
          

            _images.Clear();

            for (int i = 0; i < IMAGES4.Length; i++)
            {
                // get the image resources from the xap
                string url;
                url = Environment.CurrentDirectory + "/" + IMAGES4[i];


                Image image = new Image();
                image.Height = 128;
                image.Width = 128;
                image.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));

                // add and reposition the image
                LayoutRoot.Children.Add(image);
                posImage(image, i);


                _images.Add(image);
            }
        }


        // move the index
        private void moveIndex(int value)
        {
            _target += value;
            _target = Math.Max(0, _target);
            _target = Math.Min(_images.Count - 1, _target);
        }



        // reposition the image
        private void posImage(Image image, int index)
        {
            double diffFactor = index - _current;


            // scale and position the image according to their index and current position
            // the one who closer to the _current has the larger scale
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = MAX_SCALE - Math.Abs(diffFactor) * SCALE_DOWN_FACTOR;
            scaleTransform.ScaleY = MAX_SCALE - Math.Abs(diffFactor) * SCALE_DOWN_FACTOR;
            image.RenderTransform = scaleTransform;

            // reposition the image
            double left = _xCenter - (IMAGE_WIDTH * scaleTransform.ScaleX) / 2 + diffFactor * OFFSET_FACTOR;
            double top = _yCenter - (IMAGE_HEIGHT * scaleTransform.ScaleY) / 2;
            image.Opacity = 1 - Math.Abs(diffFactor) * OPACITY_DOWN_FACTOR;

            image.SetValue(Canvas.LeftProperty, left);
            image.SetValue(Canvas.TopProperty, top);

            // order the element by the scaleX
            image.SetValue(Canvas.ZIndexProperty, (int)Math.Abs(scaleTransform.ScaleX * 100));
        }

        /////////////////////////////////////////////////////        
        // Public Methods
        /////////////////////////////////////////////////////	

        // start the timer
        public void Start()
        {
            // start the enter frame event
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 100 / FPS);
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }
        

        public void show_from_file(int i1,int i2,int i3,int i4,int i5, int r1, int r2, int r3, int r4, int r5)
        {
            string urlw;
           

           


           

            imgFace.Width = r1;
            imgFace.Height = r1 + selim;
            imgEyes.Width = r2;
            imgEyes.Height = r2 + selim;
            imgNose.Width = r3;
            imgNose.Height = r3 + selim;
            imgEars.Width = r4;
            imgEars.Height = r4 + selim;
            imgMouth.Width = r5;
            imgMouth.Height = r5 + selim;
            if (i1 != (-1))
            {
               
                urlw = Environment.CurrentDirectory + "/" + IMAGES[i1];
                imgFace.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
               
            }
            if (i2 != (-1))
            {
                urlw = Environment.CurrentDirectory + "/" + IMAGES1[i2];
                imgEyes.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
            }
            if (i3 != (-1))
            {
                urlw = Environment.CurrentDirectory + "/" + IMAGES2[i3];
                imgNose.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
            }

            if (i4 != (-1))
            {
                urlw = Environment.CurrentDirectory + "/" + IMAGES3[i4];
                imgEars.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
            }

            if (i5 != (-1))
            {
                urlw = Environment.CurrentDirectory + "/" + IMAGES4[i5];
                imgMouth.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
            }
           
            

        }


        public void show(int i)
        {
            string urlw;
           

            if (typep == 1)
            {
              
                urlw = Environment.CurrentDirectory + "/" + IMAGES[i];
                imgFace.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
                i1 = i;



            }
            else if (typep == 2)
            {
              
                urlw = Environment.CurrentDirectory + "/" + IMAGES1[i];
                imgEyes.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
                i2 = i;
            }
            else if (typep == 3)
            {
             
                urlw = Environment.CurrentDirectory + "/" + IMAGES2[i];
                imgNose.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
                i3 = i;
            }
            else if (typep == 4)
            {
            
                urlw = Environment.CurrentDirectory + "/" + IMAGES3[i];
                imgEars.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
                i4 = i;
            }
            else if (typep == 5)
            {
             
                urlw = Environment.CurrentDirectory + "/" + IMAGES4[i];
                imgMouth.Source = new BitmapImage(new Uri(urlw, UriKind.RelativeOrAbsolute));
                i5 = i;
            }
          

            //если что-то не так
            else
            {
                urlw = "image/white.png";
                
            }






        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            
           
            moveIndex(-1);
            disbut();

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            

            moveIndex(1);
            disbut();
          


        }

        private void disbut()
        {
            int _t = Convert.ToInt32(_target);
          
            if ((_t - 1) < 0)
            {
                button.IsEnabled = false;
            }
            else button.IsEnabled = true;

            if ((_t + 1) >= cheB1)
            {
                button2.IsEnabled = false;
            }
            else button2.IsEnabled = true;

            if (button.IsEnabled == false)
            {
                button.Visibility = Visibility.Hidden;
            }
            else button.Visibility = Visibility.Visible;

            if (button2.IsEnabled == false)
            {
                button2.Visibility = Visibility.Hidden;
            }
            else button2.Visibility = Visibility.Visible;
        }

        



        private void ins_Click(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt32(_target);
            show(i);
        }

        private void bFace_proc()
        {
            button.IsEnabled = false;
            button2.IsEnabled = true;
            cheB = 0;
            cheB1 = IMAGES.Length;
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.None;
            pic_time.Visibility = Visibility.Hidden;
            rezh = false;
            typep = 1;
            if (typep == 1)
            {

                MasVal.Value = imgFace.Width;
                selim = Convert.ToInt32(imgFace.Width - imgFace.Height);
            }
            INIT();
            addImages();
            moveIndex(0);
          
        }

        private void bFace_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            button2.IsEnabled = true;
            cheB = 0;
            cheB1 = IMAGES.Length;
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.None;
            pic_time.Visibility = Visibility.Hidden;
            rezh = false;
            typep = 1;
            if (typep == 1)
            {

                MasVal.Value = imgFace.Width;
                selim = Convert.ToInt32(imgFace.Width - imgFace.Height);
            }
            INIT();
            addImages();
            moveIndex(0);
           

        }

        private void bEyes_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            button2.IsEnabled = true;
            cheB = 0;
            cheB1 = IMAGES1.Length;
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.None;
            pic_time.Visibility = Visibility.Hidden;
            typep = 2;
            if (typep == 2)
            {

                MasVal.Value = imgEyes.Width;
                selim = Convert.ToInt32(imgEyes.Width - imgEyes.Height);
            }
            INIT();
            addImages1();
            moveIndex(0);
           
        }







        

        private void uzip()
        {
            DirectoryInfo di = Directory.CreateDirectory("lib");
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            ZipArchive zip = ZipFile.OpenRead(@"lib.xyz");
            zip.ExtractToDirectory("lib");
        }

       

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (!twochek)
            {

                RoutedEventArgs e2 = null;
                newDialog nd = new newDialog();
                nd.Owner = this;

                if (nd.ShowDialog().Value)
                {
                    if (nd.res)
                    {
                        s_2_Click(sender, e2);
                    }
                    else

                    {
                        System.Diagnostics.Process prc = new System.Diagnostics.Process();
                        prc.StartInfo.FileName = @"DelLib.exe";
                        prc.Start();
                    }

                }
                else
                {
                    e.Cancel = true;

                }
            }
            else
            {
               // Close();               
            }
                


        }

        

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            
            if (sel_ce.Visibility == Visibility.Visible)
            {
                sel_ce.Visibility = Visibility.Hidden;
                sel_le.Visibility = Visibility.Hidden;
                sel_ri.Visibility = Visibility.Hidden;
                View_1.IsChecked = false;
            }else
            {
                sel_ce.Visibility = Visibility.Visible;
                sel_le.Visibility = Visibility.Visible;
                sel_ri.Visibility = Visibility.Visible;
                View_1.IsChecked = true;
            }

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (wellcome1 == true)
            {
                WellcomeCanvas.Visibility = Visibility.Hidden;
                Canv_down.Visibility = Visibility.Visible;
                wellcome1 = false;
                FaceMenu.IsEnabled = true;
                EyesMenu.IsEnabled = true;
                NoseMenu.IsEnabled = true;
                EarsMenu.IsEnabled = true;
                MouthMenu.IsEnabled = true;
                EditMenu.IsEnabled = true;
                disbut();
            }
            else
            {
                newDialog nd = new newDialog();
                nd.Owner = this;

                if (nd.ShowDialog().Value)
                {
                    if (nd.res)
                    {
                        s_2_Click(sender, e);
                        crnew();

                    }
                    else
                    {
                        crnew();
                    }
                }
            }
           
        }
        private void crnew()
        {
            this.inkCanvas1.Strokes.Clear();
            ink_b.IsEnabled = true;
            Clean_b.IsEnabled = true;
            CanvaImg.Background = new SolidColorBrush();
            imgFace.Source = null;
            imgEyes.Source = null;
            imgNose.Source = null;
            imgEars.Source = null;
            imgMouth.Source = null;

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
        }

     

        private void left_arrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            left_arrow.Source = new BitmapImage(new Uri("images/arrows/le_d.png", UriKind.RelativeOrAbsolute));

        }                

        private void M_left_b_MouseLeave(object sender, MouseEventArgs e)
        {
            left_arrow.Source = new BitmapImage(new Uri("images/arrows/le.png", UriKind.RelativeOrAbsolute));

        }

        private void left_arrow_MouseMove(object sender, MouseEventArgs e)
        {
            left_arrow.Source = new BitmapImage(new Uri("images/arrows/le_d1.png", UriKind.RelativeOrAbsolute));

        }

        private void right_arrow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            right_arrow.Source = new BitmapImage(new Uri("images/arrows/ri_d.png", UriKind.RelativeOrAbsolute));

        }

        private void M_right_b_MouseLeave(object sender, MouseEventArgs e)
        {
            right_arrow.Source = new BitmapImage(new Uri("images/arrows/ri.png", UriKind.RelativeOrAbsolute));

        }

        private void right_arrow_MouseMove(object sender, MouseEventArgs e)
        {
            right_arrow.Source = new BitmapImage(new Uri("images/arrows/ri_d1.png", UriKind.RelativeOrAbsolute));

        }

        private void View_2_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Owner = this;
            if (cd.ShowDialog().Value)
            {
               parentGrid.Background = new SolidColorBrush(cd.SelectColor);
                WellcomeCanvas.Background = parentGrid.Background;
            }
        }

        private void ins_ma_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ins_ma.Source = new BitmapImage(new Uri("images/inim/ins_c2.png", UriKind.RelativeOrAbsolute));

        }

        private void ins_ma_MouseMove(object sender, MouseEventArgs e)
        {
            ins_ma.Source = new BitmapImage(new Uri("images/inim/ins_c1.png", UriKind.RelativeOrAbsolute));

        }

        private void ins_ma_MouseLeave(object sender, MouseEventArgs e)
        {
            ins_ma.Source = new BitmapImage(new Uri("images/inim/ins_c0.png", UriKind.RelativeOrAbsolute));

        }

        private void ins_l_MouseLeave(object sender, MouseEventArgs e)
        {
            ins_la.Source = new BitmapImage(new Uri("images/inim/ins_l0.png", UriKind.RelativeOrAbsolute));

        }

        private void ins_l_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ins_la.Source = new BitmapImage(new Uri("images/inim/ins_l2.png", UriKind.RelativeOrAbsolute));

        }

        private void ins_la_MouseMove(object sender, MouseEventArgs e)
        {
            ins_la.Source = new BitmapImage(new Uri("images/inim/ins_l1.png", UriKind.RelativeOrAbsolute));

        }

        private void ins_r_MouseLeave(object sender, MouseEventArgs e)
        {
            ins_ra.Source = new BitmapImage(new Uri("images/inim/ins_r0.png", UriKind.RelativeOrAbsolute));

        }

        private void ins_r_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ins_ra.Source = new BitmapImage(new Uri("images/inim/ins_r2.png", UriKind.RelativeOrAbsolute));

        }

        private void ins_ra_MouseMove(object sender, MouseEventArgs e)
        {
            ins_ra.Source = new BitmapImage(new Uri("images/inim/ins_r1.png", UriKind.RelativeOrAbsolute));

        }

        private void loadmenu()
        {
            FaceMenu.Items.Clear();
            EyesMenu.Items.Clear();
            NoseMenu.Items.Clear();
            EarsMenu.Items.Clear();
            MouthMenu.Items.Clear();
            FileStream myStream = new FileStream(fPath, FileMode.Open);
            StreamReader sr = new StreamReader(myStream);

            int max1 = Convert.ToInt32(sr.ReadLine());
            int max2 = Convert.ToInt32(sr.ReadLine());
            int max3 = Convert.ToInt32(sr.ReadLine());
            int max4 = Convert.ToInt32(sr.ReadLine());
            int max5 = Convert.ToInt32(sr.ReadLine());
            string[] array1 = new string[max1];
            string[] array2 = new string[max2];
            string[] array3 = new string[max3];
            string[] array4 = new string[max4];
            string[] array5 = new string[max5];
            for (int i = 0; i < max1; i++)
            {
                array1[i] = sr.ReadLine();
                MenuItem newFace = new MenuItem();
                newFace.Header = array1[i];
                FaceMenu.Items.Add(newFace);
                newFace.Click += NewFace_Click;
                newFace.PreviewMouseLeftButtonDown += NewMenuItem1_PreviewMouseLeftButtonDown;
            }
            for (int i = 0; i < max2; i++)
            {
                array2[i] = sr.ReadLine();
                MenuItem newEyes = new MenuItem();
                newEyes.Header = array2[i];
                EyesMenu.Items.Add(newEyes);
                newEyes.Click += NewEyes_Click;
                newEyes.PreviewMouseLeftButtonDown += NewMenuItem1_PreviewMouseLeftButtonDown;
            }
            for (int i = 0; i < max3; i++)
            {
                array3[i] = sr.ReadLine();
                MenuItem newNose = new MenuItem();
                newNose.Header = array3[i];
                NoseMenu.Items.Add(newNose);
                newNose.Click += NewNose_Click;
                newNose.PreviewMouseLeftButtonDown += NewMenuItem1_PreviewMouseLeftButtonDown;
            }
            for (int i = 0; i < max4; i++)
            {
                array4[i] = sr.ReadLine();
                MenuItem newEars = new MenuItem();
                newEars.Header = array4[i];
                EarsMenu.Items.Add(newEars);
                newEars.Click += NewEars_Click;
                newEars.PreviewMouseLeftButtonDown += NewMenuItem1_PreviewMouseLeftButtonDown;
            }
            for (int i = 0; i < max5; i++)
            {
                array5[i] = sr.ReadLine();
                MenuItem newMouth = new MenuItem();
                newMouth.Header = array5[i];
                MouthMenu.Items.Add(newMouth);
                newMouth.Click += NewMouth_Click;
                newMouth.PreviewMouseLeftButtonDown += NewMenuItem1_PreviewMouseLeftButtonDown;
            }



        }

        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
         

        }

        private void FileMain_Click(object sender, RoutedEventArgs e)
        {
           
            
            }

        private void FileMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (wellcome1 == true)
            {
                s_2.IsEnabled = false;
                s_2p.IsEnabled = false;
            }
            else
            {
                s_2.IsEnabled = true;
                s_2p.IsEnabled = true;
            }
        }

        private void NewMouth_Click(object sender, RoutedEventArgs e)
        {
            t5 = MouthMenu.Items.IndexOf(sender) + 1;
            change_type();
            if (IMAGES4.Length == 0)
            {
                button.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                ins_m.Visibility = Visibility.Hidden;
                IMAGES4 = Directory.GetFiles(@"def");
            }
            bMouth_Click(sender, e);
        }

        private void aboutMnu_Click(object sender, RoutedEventArgs e)
        {
            AboutWin aw = new AboutWin();
            aw.Owner = this;
            aw.ShowDialog();           
        }

        private void NewEars_Click(object sender, RoutedEventArgs e)
        {
            t4 = EarsMenu.Items.IndexOf(sender) + 1;
            change_type();
            if (IMAGES3.Length == 0)
            {
                button.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                ins_m.Visibility = Visibility.Hidden;
                IMAGES3 = Directory.GetFiles(@"def");
            }
            bEars_Click(sender, e);
        }

        private void NewNose_Click(object sender, RoutedEventArgs e)
        {
            t3 = NoseMenu.Items.IndexOf(sender) + 1;
            change_type();
            if (IMAGES2.Length == 0)
            {
                button.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                ins_m.Visibility = Visibility.Hidden;
                IMAGES2 = Directory.GetFiles(@"def");
            }
            bNose_Click(sender, e);
        }

        private void NewEyes_Click(object sender, RoutedEventArgs e)
        {
            t2 = EyesMenu.Items.IndexOf(sender) + 1;
            change_type();
            if (IMAGES1.Length == 0)
            {
                button.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                ins_m.Visibility = Visibility.Hidden;
                IMAGES1 = Directory.GetFiles(@"def");
            }
            bEyes_Click(sender, e);
           
        }

        private void NewFace_Click(object sender, RoutedEventArgs e)
        {
            t1 = FaceMenu.Items.IndexOf(sender) + 1;
            change_type();
            if (IMAGES.Length == 0)
            {
                button.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                ins_m.Visibility = Visibility.Hidden;
                IMAGES = Directory.GetFiles(@"def");
            } 
            bFace_Click(sender, e);
            
        }
        private void defLoad()
        {
            FaceMenu.IsEnabled = false;
            EyesMenu.IsEnabled = false;
            NoseMenu.IsEnabled = false;
            EarsMenu.IsEnabled = false;
            MouthMenu.IsEnabled = false;
            EditMenu.IsEnabled = false;
            object sender=null; 
            RoutedEventArgs e=null;
            t1 = 1;
            change_type();
            bFace_Click(sender, e);
        }

        

        private void NewMenuItem1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moveClear(-100);
        }

        
               

        

        private void change_type()
        {
            if (t1 == 0)
            {
                t1 = 1;
            }
            if (t2 == 0)
            {
                t2 = 1;
            }
            if (t3 == 0)
            {
                t3 = 1;
            }
            if (t4 == 0)
            {
                t4 = 1;
            }
            if (t5 == 0)
            {
                t5 = 1;
            }
            
            IMAGES = Directory.GetFiles(@"lib/face/t"+t1);            
            IMAGES1 = Directory.GetFiles(@"lib/eyes/t"+t2);           
            IMAGES2 = Directory.GetFiles(@"lib/nose/t"+t3);           
            IMAGES3 = Directory.GetFiles(@"lib/ears/t"+t4);
            IMAGES4 = Directory.GetFiles(@"lib/mouth/t"+t5);
           
            
                disbut();
                ins_m.Visibility = Visibility.Visible;
           


        }

       

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt32(_target)-1;
            show(i);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt32(_target)+1;
            show(i);
        }

       



        private void bNose_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            button2.IsEnabled = true;
            cheB = 0;
            cheB1 = IMAGES2.Length;
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.None;
            pic_time.Visibility = Visibility.Hidden;
            rezh = false;
            typep = 3;
            if (typep == 3)
            {

                MasVal.Value = imgNose.Width;
                selim = Convert.ToInt32(imgNose.Width - imgNose.Height);
            }
            INIT();
            addImages2();
            moveIndex(0);
           
        }

        private void bEars_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            button2.IsEnabled = true;
            cheB = 0;
            cheB1 = IMAGES3.Length;
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.None;
            pic_time.Visibility = Visibility.Hidden;
            rezh = false;
            typep = 4;
            if (typep == 4)
            {

                MasVal.Value = imgEars.Width;
                selim = Convert.ToInt32(imgEars.Width - imgEars.Height);
            }
            INIT();
            addImages3();
            moveIndex(0);
           
        }

        private void bMouth_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            button2.IsEnabled = true;
            cheB = 0;
            cheB1 = IMAGES4.Length;
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.None;
            pic_time.Visibility = Visibility.Hidden;
            rezh = false;
            typep = 5;
            if (typep == 5)
            {

                MasVal.Value = imgMouth.Width;
                selim = Convert.ToInt32(imgMouth.Width - imgMouth.Height);
            }
            INIT();
            addImages4();
            moveIndex(0);
           
        }

       

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (typep == 1)
            {
                      
                imgFace.Width = MasVal.Value;
                imgFace.Height = MasVal.Value + selim;
            }
            if (typep == 2)
            {
               
                imgEyes.Width = MasVal.Value;
                imgEyes.Height = MasVal.Value + selim;
            }
            if (typep == 3)
            {
               
                imgNose.Width = MasVal.Value;
                imgNose.Height = MasVal.Value + selim;
            }
            if (typep == 4)
            {
                
                imgEars.Width = MasVal.Value;
                imgEars.Height = MasVal.Value + selim;
            }
            if (typep == 5)
            {
               
                imgMouth.Width = MasVal.Value;
                imgMouth.Height = MasVal.Value + selim;
            }



        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

       

        public static ImageSource ToImageSource(FrameworkElement obj)
        {
            // Save current canvas transform
            Transform transform = obj.LayoutTransform;
            obj.LayoutTransform = null;

            // fix margin offset as well
            Thickness margin = obj.Margin;
            obj.Margin = new Thickness(0, 0,
                 margin.Right - margin.Left, margin.Bottom - margin.Top);

            // Get the size of canvas
            Size size = new Size(obj.Width, obj.Height);

            // force control to Update
            obj.Measure(size);
            obj.Arrange(new Rect(size));

            RenderTargetBitmap bmp = new RenderTargetBitmap(
                (int)obj.Width, (int)obj.Height, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(obj);

            // return values as they were before
            obj.LayoutTransform = transform;
            obj.Margin = margin;
            return bmp;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(SaveImg, 0);
            Canvas.SetTop(SaveImg, 0);
            SaveImg.Height = Canva.Height;
            SaveImg.Width = Canva.Width;
            restBorder.Visibility = Visibility.Hidden;           
            SaveImg.Source = ToImageSource(CanvaImg);
            restBorder.Visibility = Visibility.Visible;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png image (рекомендуется)|*.png;|Jpeg image|*.jpeg;| Bitmap image|*.bmp ";
            sfd.FileName = "без_имени";
            if (sfd.ShowDialog() == true && SaveImg.Source != null)
            {
                FileInfo finf = new FileInfo(sfd.FileName);
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                string ext = finf.Extension.ToLower();
                BitmapEncoder bmpEncoder = null;
                if (ext == ".png")
                {
                    bmpEncoder = new PngBitmapEncoder();
                }
                else if (ext == ".jpeg")
                {
                    bmpEncoder = new JpegBitmapEncoder();
                }
                else if (ext == ".bmp")
                {
                    bmpEncoder = new PngBitmapEncoder();
                }
                bmpEncoder.Frames.Add(BitmapFrame.Create(SaveImg.Source as BitmapSource));
                bmpEncoder.Save(fs);
                fs.Close();
            }

            //save

        }

        private void colBt_Click(object sender, RoutedEventArgs e)
        {
            /** Колордиалог  **/
            ColorDialog cd = new ColorDialog();
            cd.Owner = this;
            if (cd.ShowDialog().Value)
            {
                CanvaImg.Background = new SolidColorBrush(cd.SelectColor);
                colorRed = cd.SelectColor.R;
                colorGreen = cd.SelectColor.G;
                colorBlue = cd.SelectColor.B;
                color_ch = true;
            }
           
        }

        private void MenuItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            moveClear(-100);
           
        }

       

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            moveClear(-100);
            ink_vkl();
        }

        private void ink_vkl()
        {
            pic_time.Visibility = Visibility.Visible;
            button.Visibility = Visibility.Hidden;
            button2.Visibility = Visibility.Hidden;
            ins_m.Visibility = Visibility.Hidden;
            rezh = true;
            typep = 6;
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void br_size_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.inkCanvas1.DefaultDrawingAttributes.Width = br_size.Value;
            this.inkCanvas1.DefaultDrawingAttributes.Height = br_size.Value;
            

        }

        private void Clean_b_Click(object sender, RoutedEventArgs e)
        {
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.EraseByPoint;
            ink_b.IsEnabled = true;
            Clean_b.IsEnabled = false;
          
        }

        private void br_size_MouseMove(object sender, MouseEventArgs e)
        {
            
            this.el_size.Width = br_size.Value;
            this.el_size.Height = br_size.Value;
        }

        private void Clear_st_b_Click(object sender, RoutedEventArgs e)
        {
            this.inkCanvas1.Strokes.Clear();
            ink_b.IsEnabled = true;
            Clean_b.IsEnabled = true;
          
        }

        private void ink_b_Click(object sender, RoutedEventArgs e)
        {
            this.inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            ink_b.IsEnabled = false;
            Clean_b.IsEnabled = true;
         
        }

      

        private void MenuItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moveClear(-100);
        }

        private void s_2_Click(object sender, RoutedEventArgs e)
        {
            x1 = Convert.ToInt32(Canvas.GetLeft(imgFace));
            x2 = Convert.ToInt32(Canvas.GetLeft(imgEyes));
            x3 = Convert.ToInt32(Canvas.GetLeft(imgNose));
            x4 = Convert.ToInt32(Canvas.GetLeft(imgEars));
            x5 = Convert.ToInt32(Canvas.GetLeft(imgMouth));
            y1 = Convert.ToInt32(Canvas.GetTop(imgFace));
            y2 = Convert.ToInt32(Canvas.GetTop(imgEyes));
            y3 = Convert.ToInt32(Canvas.GetTop(imgNose));
            y4 = Convert.ToInt32(Canvas.GetTop(imgEars));
            y5 = Convert.ToInt32(Canvas.GetTop(imgMouth));
            r1 = Convert.ToInt32(imgFace.Width);
            r2 = Convert.ToInt32(imgEyes.Width);
            r3 = Convert.ToInt32(imgNose.Width);
            r4 = Convert.ToInt32(imgEars.Width);
            r5 = Convert.ToInt32(imgMouth.Width);

            int[] dats = { i1, i2, i3, i4, i5, x1, x2, x3, x4, x5, y1, y2, y3, y4, y5,r1,r2,r3,r4,r5,t1,t2,t3,t4,t5 };
            colorRed = ((SolidColorBrush)(CanvaImg.Background)).Color.R;
            colorGreen = ((SolidColorBrush)(CanvaImg.Background)).Color.G;
            colorBlue = ((SolidColorBrush)(CanvaImg.Background)).Color.B;
            byte[] array1 = { colorRed, colorGreen, colorBlue };           
            //сохранение
          
           
            


            FileStream fs = null;
            BinaryWriter bw = null;
           

            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.Filter = "файл проекта (*.imk) | *.imk";
            sfd1.FileName = "без_имени";
            if (sfd1.ShowDialog() == true)
            {
               

                try
                {
                     fs = new FileStream(sfd1.FileName, FileMode.Create);
                    bw = new BinaryWriter(fs);

                    // запись массива байтов в файл


                    //строук в поток и в байт                    
                    bw.Write(dats.Length);
                    bw.Write(array1.Length);
                    for (int i=0; i < dats.Length; i++)
                    {
                        bw.Write(dats[i]);
                    }
                   
                    bw.Write(array1);
                    bw.Write(color_ch);
                    inkCanvas1.Strokes.Save(fs);
                }
                finally
                {
                    if (fs != null)
                    {
                        bw.Close();
                      //  fs.Close();
                    }
                }

              
            }
            

        }

        private void Open_pro_Click(object sender, RoutedEventArgs e)
        {

            if (wellcome1 == true)
            {               
                Opener();
               if (notdo==false)
                {
                    WellcomeCanvas.Visibility = Visibility.Hidden;
                    Canv_down.Visibility = Visibility.Visible;
                    wellcome1 = false;
                    FaceMenu.IsEnabled = true;
                    EyesMenu.IsEnabled = true;
                    NoseMenu.IsEnabled = true;
                    EarsMenu.IsEnabled = true;
                    MouthMenu.IsEnabled = true;
                    EditMenu.IsEnabled = true;
                }
            }
            else
            {
                newDialog nd = new newDialog();
                nd.Owner = this;

                if (nd.ShowDialog().Value)
                {
                    if (nd.res)
                    {
                        s_2_Click(sender, e);
                    }
                    else
                    {
                        Opener();

                    }
                }
            }
        }

        private void Opener()
        {
            int t1b, t2b, t3b, t4b, t5b;
            t1b = t1;
            t2b = t2;
            t3b = t3;
            t4b = t4;
            t5b = t5;

            byte[] array1_r = null;
            int[] array_r = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, Convert.ToInt32(imgFace.Width), Convert.ToInt32(imgEyes.Width), Convert.ToInt32(imgNose.Width), Convert.ToInt32(imgEars.Width), Convert.ToInt32(imgMouth.Width), 0, 0, 0, 0, 0 };
            FileStream fs = null;
            BinaryReader br = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "файл проекта | *.imk";

            if (ofd.ShowDialog() == true)
            {
              
               
                FileInfo finf = new FileInfo(ofd.FileName);
            }


            if (!File.Exists(ofd.FileName))
            {
                notdo = true;
              
                return;
            }

            try
            {
                fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                int arl = br.ReadInt32();
                int arl1 = br.ReadInt32();

                for (int i = 0; i < arl; i++)
                {
                    array_r[i] = br.ReadInt32();
                }
                array1_r = br.ReadBytes(arl1);
                color_ch = br.ReadBoolean();
                StrokeCollection strokes = new StrokeCollection(fs);
                inkCanvas1.Strokes = strokes;

            }
            finally
            {
                if (fs != null)
                {
                    //  fs.Close();
                    br.Close();
                }
            }
           
            //Применяем параметры...:)
            System.Threading.Thread.Sleep(1000);
            //Strokes


            //конец Strokes
            Canvas.SetLeft(imgFace, array_r[5]);
            Canvas.SetLeft(imgEyes, array_r[6]);
            Canvas.SetLeft(imgNose, array_r[7]);
            Canvas.SetLeft(imgEars, array_r[8]);
            Canvas.SetLeft(imgMouth, array_r[9]);
            Canvas.SetTop(imgFace, array_r[10]);
            Canvas.SetTop(imgEyes, array_r[11]);
            Canvas.SetTop(imgNose, array_r[12]);
            Canvas.SetTop(imgEars, array_r[13]);
            Canvas.SetTop(imgMouth, array_r[14]);
            if (color_ch == true)
            {
                CanvaImg.Background = new SolidColorBrush(Color.FromRgb(array1_r[0], array1_r[1], array1_r[2]));
            }
            else CanvaImg.Background = new SolidColorBrush();
            t1 = array_r[20];
            t2 = array_r[21];
            t3 = array_r[22];
            t4 = array_r[23];
            t5 = array_r[24];
            change_type();

            show_from_file(array_r[0], array_r[1], array_r[2], array_r[3], array_r[4], array_r[15], array_r[16], array_r[17], array_r[18], array_r[19]);
            i1 = array_r[0];
            i2 = array_r[1];
            i3 = array_r[2];
            i4 = array_r[3];
            i5 = array_r[4];
            r1 = array_r[15];
            r2 = array_r[16];
            r3 = array_r[17];
            r4 = array_r[18];
            r5 = array_r[19];
            //закончили
            if (rezh)
            {
                ink_vkl();
            }

            t1 = t1b;
            t2 = t2b;
            t3 = t3b;
            t4 = t4b;
            t5 = t5b;
            change_type();

        }

    }
}
