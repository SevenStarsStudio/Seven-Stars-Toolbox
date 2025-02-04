using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;

namespace SevenStarsToolbox
{
    /// <summary>
    /// Interaction logic for DoorGenerator.xaml
    /// </summary>
    public partial class DoorGenerator : Window
    {
        bool openExplorerOnSave;
        const int WIDTH = 16;
        const int HEIGHT = 16;

        BitmapSource[,] generatedImages = null;

        Image rawImage = null;
        BitmapImage bitmapImage = null;

        public DoorGenerator()
        {
            InitializeComponent();
            version.Text = App.VERSION;
        }

        private void btnClick_findFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                bitmapImage = new BitmapImage(new Uri(op.FileName));
                bitmapImage.Freeze();
                fileName.Text = op.SafeFileName.Split(".")[0] + "_";
                sourceImage.Source = bitmapImage;
                sourceImage.Width = (int)bitmapImage.Width;
                sourceImage.Height = (int)bitmapImage.Height;
                rawImage = sourceImage;
                bitmapImage = bitmapImage;
            }
        }

        private void btnClick_generate(object sender, RoutedEventArgs e)
        {
            // Stopwatch
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // Clear current storage
            generatedImageGrid.Children.Clear();

            // Content fetching
            Image sourceRawImage = sourceImage;

            // Validation
            if (sourceRawImage == null)
            {
                details.Text = $"Source Raw Image is null";
                return;
            }
            if (bitmapImage == null)
            {
                details.Text = $"Source Bitmap Image is null";
                return;
            }
            if (sourceRawImage.Width % WIDTH != 0 || sourceRawImage.Height % HEIGHT != 0)
            {
                details.Text = $"Image size mismatch ({sourceRawImage.Width}, {sourceRawImage.Height})";
                return;
            }

            // Determine node amounts
            int xNodeAmount = (int)(sourceRawImage.Width / WIDTH);
            int yNodeAmount = (int)(sourceRawImage.Height / HEIGHT);

            generatedImageGrid.Children.Clear();
            generatedImages = new BitmapSource[xNodeAmount, yNodeAmount];

            ImageUtils.PixelColor[,] pixels = ImageUtils.GetPixels((BitmapSource)sourceRawImage.Source);


            for (int x = 0; x < xNodeAmount; x++)
            {
                for (int y = 0; y < yNodeAmount; y++)
                {
                    // Write image
                    Bitmap myBitmap = new Bitmap(WIDTH, HEIGHT, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                    myBitmap.SetResolution(WIDTH, HEIGHT);
                    for (int pixelX = 0; pixelX < WIDTH; pixelX++)
                    {
                        for (int pixelY = 0; pixelY < HEIGHT; pixelY++)
                        {
                            ImageUtils.PixelColor pixelColor = pixels[pixelX + (WIDTH * x), pixelY + (HEIGHT * y)];
                            Color color = Color.FromArgb(pixelColor.Alpha, pixelColor.Red, pixelColor.Green, pixelColor.Blue);
                            myBitmap.SetPixel(pixelX, pixelY, color);
                        }
                    }
                    generatedImages[x, y] = ImageUtils.GetBitmapSource(myBitmap);
                    addBitmapImageToCanvas(x, y);
                }
            }

            sw.Stop();
            details.Text = $"" +
                $"Result \n" +
                $"Width : {generatedImages.GetLength(0)}\n" +
                $"Height : {generatedImages.GetLength(1)}\n" +
                "\n" +
                $"Time : {sw.ElapsedMilliseconds} Ms";
        }

        private void addBitmapImageToCanvas(int x, int y)
        {
            Image croppedImage = new Image();
            croppedImage.Width = WIDTH;
            croppedImage.Height = HEIGHT;

            croppedImage.Source = generatedImages[x, y];
            croppedImage.VerticalAlignment = VerticalAlignment.Top;
            croppedImage.HorizontalAlignment = HorizontalAlignment.Left;

            Thickness margin = Margin;
            margin.Top = croppedImage.Width * y + 1 * y;
            margin.Left = croppedImage.Height * x + 1 * x;
            croppedImage.Margin = margin;
            

            generatedImageGrid.Children.Add(croppedImage);
        }

        private void btnClick_save(object sender, RoutedEventArgs e)
        {
            if (generatedImages == null)
            {
                return;
            }

            OpenFolderDialog saveDialog = new OpenFolderDialog();
            saveDialog.Title = "Save";

            if (saveDialog.ShowDialog() == true)
            {
                int count = 0;

                for (int x = 0; x < generatedImages.GetLength(0); x++)
                {
                    for (int y = generatedImages.GetLength(1) - 1; y >= 0; y--)
                    {
                        string path = saveDialog.FolderName + $"/{fileName.Text}{count}.png";
                        ImageUtils.SaveBitmapImage(path, (BitmapImage)generatedImages[x, y]);

                        count++;
                    }
                }

                if (onsave_explorer_checkbox.IsChecked == true)
                {
                    Process.Start("explorer.exe", saveDialog.FolderName);
                }
            }
        }
    }
}
