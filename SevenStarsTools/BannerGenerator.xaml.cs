using Microsoft.Win32;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using static SevenStarsTools.ImageUtils;

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
        List<BitmapImage> shieldBitImages = null;
        List<BitmapImage> convertedImages = null;

        Dictionary<PixelColor, List<Vector2>> sourceGrids = new Dictionary<PixelColor, List<Vector2>>();
        Dictionary<PixelColor, List<Vector2>> templateGrids = new Dictionary<PixelColor, List<Vector2>>();

        Image?[,]? shieldBitImagesTable;

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

                // Create source color/grid
                sourceGrids = ImageUtils.GenerateColorGrid(sourceGrids, shieldSourceTemplateBitImage);
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


                // Create source color/grid
                templateGrids = ImageUtils.GenerateColorGrid(templateGrids, shieldSplitTemplateBitImage);
                App.CopyImageToClipboard(shieldSplitTemplateBitImage);
            }
        }

        private void btn_openExplorer_shieldFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = true;
            op.Title = "Select the shield base images";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                shieldBitImages = new List<BitmapImage>();

                foreach(var filename in op.FileNames)
                {
                    BitmapImage image = new BitmapImage(new Uri(filename));
                    image.Freeze();
                    shieldBitImages.Add(image);
                }
            }

            if(shieldBitImages.Count > 0)
            {
                int capColumns = 4;
                int capRows = 4;

                int maxRows = (int)Math.Round((shieldBitImages.Count / 4) + .5f);

                maxRows = Math.Min(capRows, maxRows);

                if(shieldBitImagesTable != null)
                {
                    for(int x = 0; x < shieldBitImagesTable.GetLength(0); x++)
                    {
                        for (int y = 0; y < shieldBitImagesTable.GetLength(1); y++)
                        {
                            Image? foundImage = shieldBitImagesTable[x, y];
                            if (foundImage != null)
                            {
                                foundImage.Source = null;
                            }
                        }
                    }
                } else
                {
                    shieldBitImagesTable = new Image[maxRows, capColumns];
                }
                sourceImageGrid.Children.Clear();

                int count = 0;
                for (int x = 0; x < shieldBitImagesTable.GetLength(0) && count < shieldBitImages.Count; x++)
                {
                    for (int y = 0; y < shieldBitImagesTable.GetLength(1) && count < shieldBitImages.Count; y++)
                    {
                        Image? newImage = shieldBitImagesTable[x, y];
                        if(newImage == null)
                        {
                            newImage = new Image();
                        }

                        BitmapImage bitImage = shieldBitImages[count];

                        newImage.Source = bitImage;
                        newImage.Width = 128;
                        newImage.Height = 128;

                        RenderOptions.SetBitmapScalingMode(newImage, BitmapScalingMode.NearestNeighbor);

                        newImage.VerticalAlignment = VerticalAlignment.Top;
                        newImage.HorizontalAlignment = HorizontalAlignment.Left;

                        Thickness margin = Margin;
                        margin.Top = newImage.Width * x;
                        margin.Left = newImage.Height * y;
                        newImage.Margin = margin;
                        newImage.SnapsToDevicePixels = true;

                        sourceImageGrid.Children.Add(newImage);

                        count++;
                    }
                }


            }
        }

        private void btnClick_generateShields(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if(convertedImages == null)
                convertedImages = new List<BitmapImage>();
            else
                convertedImages.Clear();

            gemeratedImageGrid.Children.Clear();


            for (int x = 0; x < shieldBitImagesTable.GetLength(0) && count < shieldBitImages.Count; x++)
            {
                for (int y = 0; y < shieldBitImagesTable.GetLength(1) && count < shieldBitImages.Count; y++)
                {
                    Image? newImage = shieldBitImagesTable[x, y];
                    if (newImage == null)
                    {
                        newImage = new Image();
                    }

                    BitmapImage bitImage = ImageUtils.Convert(shieldBitImages[count], sourceGrids, templateGrids);
                    convertedImages.Add(bitImage);
                    newImage.Source = bitImage;
                    newImage.Width = 128;
                    newImage.Height = 128;

                    RenderOptions.SetBitmapScalingMode(newImage, BitmapScalingMode.NearestNeighbor);

                    newImage.VerticalAlignment = VerticalAlignment.Top;
                    newImage.HorizontalAlignment = HorizontalAlignment.Left;

                    Thickness margin = Margin;
                    margin.Top = newImage.Width * x;
                    margin.Left = newImage.Height * y;
                    newImage.Margin = margin;
                    newImage.SnapsToDevicePixels = true;

                    gemeratedImageGrid.Children.Add(newImage);

                    count++;
                }
            }
        }
        private void btnClick_save(object sender, RoutedEventArgs e)
        {
            if (convertedImages == null)
            {
                return;
            }

            OpenFolderDialog saveDialog = new OpenFolderDialog();
            saveDialog.Title = "Save";

            if (saveDialog.ShowDialog() == true)
            {
                int count = 0;

                for (int x = 0; x < convertedImages.Count; x++)
                {

                    string path = saveDialog.FolderName + $"/{shieldBitImages[x].UriSource.Segments.Last()}";
                    ImageUtils.SaveBitmapImage(path, (BitmapImage)convertedImages[x]);

                    count++;
                    
                }

                if (onsave_explorer_checkbox.IsChecked == true)
                {
                    Process.Start("explorer.exe", saveDialog.FolderName);
                }
            }
        }

    }
}
