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

namespace KG1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private byte R = 0, G=0, B=0;
        private double X = 0, Y = 0, Z = 0;
        private double C =0, M =0,Y2 =0, K = 0;
        private bool rg = false;
        private bool ts = false;
        private void Rect_Change()// byte R, byte G, byte B)
        {
            MRectangle.Fill = new SolidColorBrush(Color.FromRgb(R, G, B));
            
            

        }

        private void RGB_to_XYZ()
        {
            double r = this.R / 255.0;
            double g = this.G / 255.0;
            double b = this.B / 255.0;
            if (r > 0.04045)
                r = Math.Pow(((r + 0.055) / 1.055), 2.4);
            else
                r = r / 12.92;
            if (g > 0.04045)
                g = Math.Pow(((g + 0.055) / 1.055), 2.4);
            else
                g = g / 12.92;
            if (b > 0.04045)
                b = Math.Pow(((b + 0.055) / 1.055), 2.4);
            else
                b = b / 12.92;
            r *= 100;
            g *= 100;
            b *= 100;
            this.X = r * 0.4124 + g * 0.3576 + b * 0.1805;
            this.Y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            this.Z = r * 0.0193 + g * 0.1192 + b * 0.9505;


            XSlider.Value = this.X;
            YSlider.Value = this.Y;
            ZSlider.Value = this.Z;
        }

        private void XYZ_to_RGB()
        {       
           

            var x = this.X / 100;
            var y = this.Y / 100;
            var z = this.Z / 100;
            /**
             * var_R = var_X *  3.2406 + var_Y * -1.5372 + var_Z * -0.4986
             * var_G = var_X * -0.9689 + var_Y *  1.8758 + var_Z *  0.0415
             * var_B = var_X *  0.0557 + var_Y * -0.2040 + var_Z *  1.0570
             */

            var r = x * 3.2406 + y * -1.5372 + z * -0.4986;
            var g = x * -0.9689 + y * 1.8758 + z * 0.0415;
            var b = x * 0.0557 + y * -0.2040 + z * 1.0570;
          
            if (r > 0.0031308)
                r = 1.055 * (Math.Pow(r, (1 / 2.4))) - 0.055;
            else
                r *= 12.92;
            if (g > 0.0031308)
                g = 1.055 * (Math.Pow(g, (1 / 2.4))) - 0.055;
            else
                g *= 12.92;
            if (b > 0.0031308)
                b = 1.055 * (Math.Pow(b, (1 / 2.4))) - 0.055;
            else
                b *= 12.92;

            r *= 255;
            g *= 255;       
            b *= 255;
            if (r <= 255 && r >= 0)
            {
                this.R = (byte)r;
                SliderR.Value = this.R;
            }
                
            if (g <= 255 && g >= 0)
            {
                this.G = (byte)g;
                SliderG.Value = this.G;
            }
            if (b <= 255 && b >= 0)
            {           
                this.B = (byte)b;
                this.SliderB.Value = this.B;
            }
        }
        private void CMYK_to_RGB()
        {
            this.R = (byte)( 255 *(1 - this.C) * (1 - this.K));
            this.G = (byte)(255 * (1 - this.M) * (1 - this.K));
            this.B = (byte)(255 * (1 - this.Y2) * (1 - this.K));
            SliderR.Value = this.R;
            SliderB.Value = this.B;
            SliderG.Value = this.G;
        }
        private void RGB_to_CMYK()
        {
            
            var r = this.R / 255.0;
            var g = this.G / 255.0;
            var b = this.B / 255.0;
            if (r == 0 || g == 0 || b == 0)
                return;
            this.K = 1 - Math.Max(r, Math.Max(g, b));
            this.C = (1 - r - this.K) / (1 - this.K);
            this.M = (1 - g - this.K) / (1 - this.K);
            //Y = (1-B'-K) / (1-K)
            this.Y2 = (1 - b - this.K) / (1 - this.K);
            KSlider.Value = this.K;
            CSlider.Value = this.C;
            Y2Slider.Value = this.Y2;
            MSlider.Value = this.M;
        }
        private void XSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.rg)
                return;
            this.ts = true;
            var slider = sender as Slider;
            this.X = slider.Value;
            XYZ_to_RGB();
            RGB_to_CMYK();
            Rect_Change();
            this.ts = false;
        }
        private void YSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.rg)
                return;
            this.ts = true;
            var slider = sender as Slider;
            this.Y = slider.Value;
            XYZ_to_RGB();
            RGB_to_CMYK();
            Rect_Change();
            this.ts = false;
        }

        private void ZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.rg)
                return;
            this.ts = true;
            var slider = sender as Slider;
            this.Z = slider.Value;
            XYZ_to_RGB();
            RGB_to_CMYK();
            Rect_Change();
            this.ts = false;

        }

        private void CSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            this.C = slider.Value;
            CMYK_to_RGB();
            Rect_Change();
        }

        private void BtRed_Click(object sender, RoutedEventArgs e)
        {
            this.R = 255;
            this.G = 0;
            this.B = 0;
            Rect_Change();
        }

        private void BtYellow_Click(object sender, RoutedEventArgs e)
        {
            this.R = 255;
            this.G = 255;
            Rect_Change();  

        }

        private void TbGreen_Click(object sender, RoutedEventArgs e)
        {
            this.R = 0;
            this.B = 0;
            this.G = 255;
            Rect_Change();

        }

        private void BtBlue_Click(object sender, RoutedEventArgs e)
        {
            this.R = 0;
            this.B = 255;
            this.G = 0;
            Rect_Change();
            
        }

        private void BtPurple_Click(object sender, RoutedEventArgs e)
        {
            this.R = 150;
            this.B = 100;
            this.G = 0;
            Rect_Change();

        }

        private void BtGray_Click(object sender, RoutedEventArgs e)
        {
            this.R = 150;
            this.B = 150;
            this.G = 150;
            Rect_Change();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = tbHex as TextBox;
            String s = tb.Text;
            MRectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(s));

        }
        

        private void MSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            this.M = slider.Value;
            CMYK_to_RGB();
            Rect_Change();

        }

        private void Y2Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            this.Y2 = slider.Value;
            CMYK_to_RGB();
            Rect_Change();
        }

        private void KSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            this.K = slider.Value;
            CMYK_to_RGB();
            Rect_Change();
        }


        /// <summary>
        /// RGB event functionality
        /// </summary>
        /// <param name="sender"> Event host </param> 
        /// <param name="e">changed detector</param>
        private void SliderR_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ts)
                return;

            this.rg = true;
            var slider = sender as Slider;
            byte r = (byte)slider.Value;
            this.R = r;
            RGB_to_XYZ();
            Rect_Change();
            this.rg = false;
        }

        private void SliderG_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ts)
                return;
            this.rg = true;
            var slider = sender as Slider;
            byte g = (byte)slider.Value;
            this.G = g;
            RGB_to_XYZ();
       
            Rect_Change();
            this.rg = false;
        }

        private void SliderB_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ts)
                return;
            this.rg = true;
            var slider = sender as Slider;
            byte b = (byte)slider.Value;
            this.B = b;
            RGB_to_XYZ();
            Rect_Change();
            this.rg = false;
        }
    }
}
