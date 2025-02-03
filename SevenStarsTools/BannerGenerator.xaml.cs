using Microsoft.Win32;
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

namespace SevenStarsTools
{
    /// <summary>
    /// Interaction logic for BannerGenerator.xaml
    /// </summary>
    public partial class BannerGenerator : Window
    {
        public BannerGenerator()
        {
            InitializeComponent();
        }

        BitmapImage shieldSourceTemplateBitImage = null;
        BitmapImage shieldSplitTemplateBitImage = null;


        private void btn_openExplorer_shieldSourceTemplate(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an the shield source template";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                shieldSourceTemplateBitImage = new BitmapImage(new Uri(op.FileName));
                shieldSourceTemplateBitImage.Freeze();
                shieldSourceTemplate.Source = shieldSourceTemplateBitImage;
                shieldSourceTemplate.Width = 128;
                shieldSourceTemplate.Height = 128;
            }
        }

        private void btn_openExplorer_shieldTemplate(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an the shield source template";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                shieldSplitTemplateBitImage = new BitmapImage(new Uri(op.FileName));
                shieldSplitTemplateBitImage.Freeze();
                shieldTemplate.Source = shieldSplitTemplateBitImage;
                shieldTemplate.Width = 128;
                shieldTemplate.Height = 128;
            }
        }

        private void btn_openExplorer_shieldFiles(object sender, RoutedEventArgs e)
        {

        }

    }
}
