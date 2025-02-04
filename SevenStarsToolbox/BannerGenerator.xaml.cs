using Microsoft.Win32;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static SevenStarsToolbox.ImageUtils;

namespace SevenStarsToolbox
{
    /// <summary>
    /// Interaction logic for BannerGenerator.xaml
    /// </summary>
    public partial class BannerGenerator : Window
    {
        public BannerGenerator()
        {
            InitializeComponent();
            version.Text = App.VERSION;
        }

        BitmapImage? sourceTemplateBitmap = null;
        BitmapImage? conversionTemplateBitmap = null;
        List<BitmapImage> sourceShieldBitmaps = null;
        List<BitmapImage> convertedShieldBitmaps = null;

        Dictionary<PixelColor, List<Vector2>> sourceGrids = new Dictionary<PixelColor, List<Vector2>>();
        Dictionary<PixelColor, List<Vector2>> templateGrids = new Dictionary<PixelColor, List<Vector2>>();

        Image?[,]? shieldBitImagesTable;
        BannerPresets currentBannerPreset = BannerPresets.CUSTOM;

        private void OpenExplorerForSourceTemplate(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an the shield source template";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                if (currentBannerPreset != BannerPresets.CUSTOM)
                {
                    currentBannerPreset = BannerPresets.CUSTOM;
                    bannerPresets.SelectedIndex = 0;
                }
                UpdateSourceTemplateImage(new BitmapImage(new Uri(op.FileName)));
            }
        }
        private void CopySourceImage(object sender, RoutedEventArgs e)
        {
            if (sourceTemplateBitmap != null)
            {
                OpenFolderDialog saveDialog = new OpenFolderDialog();
                saveDialog.Title = "Save Source Image";
                if (saveDialog.ShowDialog() == true)
                {
                    string path = saveDialog.FolderName + $"\\{sourceTemplateBitmap.UriSource.Segments.Last()}";
                    ImageUtils.SaveBitmapImage(path, sourceTemplateBitmap);
                }
            }
        }

        private void OpenExplorerForConversionTemplate(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an the shield source template";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                if(currentBannerPreset != BannerPresets.CUSTOM)
                {
                    currentBannerPreset = BannerPresets.CUSTOM;
                    bannerPresets.SelectedIndex = 0;
                }
                UpdateSplitTemplateImage(new BitmapImage(new Uri(op.FileName)));
            }
        }

        
        private void CopyTemplateImage(object sender, RoutedEventArgs e)
        {
            if (conversionTemplateBitmap != null)
            {
                OpenFolderDialog saveDialog = new OpenFolderDialog();
                saveDialog.Title = "Save Template Image";

                if (saveDialog.ShowDialog() == true)
                {
                    string path = saveDialog.FolderName + $"\\{conversionTemplateBitmap.UriSource.Segments.Last()}";
                    ImageUtils.SaveBitmapImage(path, conversionTemplateBitmap);
                }
            }
        }

        private void OpenExplorerForShieldSources(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = true;
            op.Title = "Select the shield base images";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                if(sourceShieldBitmaps == null)
                    sourceShieldBitmaps = new List<BitmapImage>();
                else
                    sourceShieldBitmaps.Clear();

                foreach (var filename in op.FileNames)
                {
                    BitmapImage image = new BitmapImage(new Uri(filename));
                    image.Freeze();
                    sourceShieldBitmaps.Add(image);
                }
            }


            if (sourceShieldBitmaps == null)
            {
                MessageBox.Show("Error : You need shields to convert before doing a conversion attempt.", "Banner Generator - Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            int capColumns = 4;
            int capRows = 4;

            int maxRows = Math.Max(1, (int)Math.Round((sourceShieldBitmaps.Count / 4) + .5f));

            maxRows = Math.Min(capRows, maxRows);

            // Clear the shield bitmap image table
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

            // Rebuild the table
            int count = 0;
            for (int x = 0; x < shieldBitImagesTable.GetLength(0) && count < sourceShieldBitmaps.Count; x++)
            {
                for (int y = 0; y < shieldBitImagesTable.GetLength(1) && count < sourceShieldBitmaps.Count; y++)
                {
                    Image? newImage = shieldBitImagesTable[x, y];
                    if(newImage == null)
                    {
                        newImage = new Image();
                    }

                    BitmapImage bitImage = sourceShieldBitmaps[count];

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

        private void BeginShieldConversion(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if(convertedShieldBitmaps == null)
                convertedShieldBitmaps = new List<BitmapImage>();
            else
                convertedShieldBitmaps.Clear();

            gemeratedImageGrid.Children.Clear();


            if(shieldBitImagesTable == null || sourceShieldBitmaps == null)
            {
                MessageBox.Show("Error : You need shields to convert before doing a conversion attempt.", "Banner Generator - Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            try
            {
                for (int x = 0; x < shieldBitImagesTable.GetLength(0) && count < sourceShieldBitmaps.Count; x++)
                {
                    for (int y = 0; y < shieldBitImagesTable.GetLength(1) && count < sourceShieldBitmaps.Count; y++)
                    {
                        Image? newImage = shieldBitImagesTable[x, y];
                        if (newImage == null)
                        {
                            newImage = new Image();
                        }


                        BitmapImage bitImage = ImageUtils.Convert(sourceShieldBitmaps[count], sourceGrids, templateGrids);
                        convertedShieldBitmaps.Add(bitImage);
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
            catch (Exception ex)
            {
                MessageBox.Show("Error : Source and Template images might not have the same colors", "Banner Generator - Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void btnClick_save(object sender, RoutedEventArgs e)
        {
            if (convertedShieldBitmaps == null)
            {
                return;
            }

            OpenFolderDialog saveDialog = new OpenFolderDialog();
            saveDialog.Title = "Save";

            if (saveDialog.ShowDialog() == true)
            {
                int count = 0;

                for (int x = 0; x < convertedShieldBitmaps.Count; x++)
                {

                    string path = saveDialog.FolderName + $"/{sourceShieldBitmaps[x].UriSource.Segments.Last()}";
                    ImageUtils.SaveBitmapImage(path, (BitmapImage)convertedShieldBitmaps[x]);

                    count++;
                    
                }

                if (onsave_explorer_checkbox.IsChecked == true)
                {
                    Process.Start("explorer.exe", saveDialog.FolderName);
                }
            }
        }

        private void BannerPresetSelectionUpdate(object sender, SelectionChangedEventArgs e)
        {
            if(bannerPresets != null)
            {
                ComboBoxItem currentItem = (ComboBoxItem)bannerPresets.SelectedItem;
                String name = currentItem.Name;
                currentBannerPreset = (BannerPresets)Enum.Parse(typeof(BannerPresets), name.ToUpper(), true);

                switch (currentBannerPreset)
                {
                    case BannerPresets.CUSTOM:
                        break;
                    case BannerPresets.KITE_SHIELD:
                        UpdateSourceTemplateImage(new BitmapImage(new Uri("pack://application:,,,/Assets/BannerPresets/kite_source_template.png")));
                        UpdateSplitTemplateImage(new BitmapImage(new Uri("pack://application:,,,/Assets/BannerPresets/kite_split_template.png")));
                        break;
                    case BannerPresets.HEATER_SHIELD:
                        UpdateSourceTemplateImage(new BitmapImage(new Uri("pack://application:,,,/Assets/BannerPresets/heater_source_template.png")));
                        UpdateSplitTemplateImage(new BitmapImage(new Uri("pack://application:,,,/Assets/BannerPresets/heater_split_template.png")));
                        break;
                    case BannerPresets.ROUND_SHIELD:
                        UpdateSourceTemplateImage(new BitmapImage(new Uri("pack://application:,,,/Assets/BannerPresets/round_source_template.png")));
                        UpdateSplitTemplateImage(new BitmapImage(new Uri("pack://application:,,,/Assets/BannerPresets/round_split_template.png")));
                        break;
                }
            }
        }

        private void UpdateSourceTemplateImage(BitmapImage? image)
        {
            if(image != null)
            {
                sourceTemplateBitmap = image;
                sourceTemplateBitmap.Freeze();
                shieldSourceTemplate.Source = sourceTemplateBitmap;
                shieldSourceTemplate.Width = 128;
                shieldSourceTemplate.Height = 128;

                sourceGrids = ImageUtils.GenerateColorGrid(sourceGrids, sourceTemplateBitmap);
            }
            else // Clear
            {
                sourceTemplateBitmap = null;
                if(shieldSourceTemplate != null)
                    shieldSourceTemplate.Source = null;
                sourceGrids.Clear();
            }
        }

        private void UpdateSplitTemplateImage(BitmapImage? image)
        {
            if (image != null)
            {
                conversionTemplateBitmap = image;
                conversionTemplateBitmap.Freeze();
                shieldTemplate.Source = conversionTemplateBitmap;
                shieldTemplate.Width = 128;
                shieldTemplate.Height = 128;

                templateGrids = ImageUtils.GenerateColorGrid(templateGrids, conversionTemplateBitmap);
            }
            else // Clear
            {
                conversionTemplateBitmap = null;
                if (shieldTemplate != null)
                    shieldTemplate.Source = null;
                templateGrids.Clear();
            }
        }
    }
}
