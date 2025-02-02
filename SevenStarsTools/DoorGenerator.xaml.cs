﻿using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Image = System.Windows.Controls.Image;
using Color = System.Drawing.Color;
using System.Drawing;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace SevenStarsTools
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
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
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

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
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

            PixelColor[,] pixels = GetPixels((BitmapSource)sourceRawImage.Source);


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
                            PixelColor pixelColor = pixels[pixelX + (WIDTH * x), pixelY + (HEIGHT * y)];
                            Color color = Color.FromArgb(pixelColor.Alpha, pixelColor.Red, pixelColor.Green, pixelColor.Blue);
                            myBitmap.SetPixel(pixelX, pixelY, color);
                        }
                    }
                    generatedImages[x, y] = GetBitmapSource(myBitmap);
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
            margin.Top = croppedImage.Width * y;
            margin.Left = croppedImage.Height * x;
            croppedImage.Margin = margin;

            generatedImageGrid.Children.Add(croppedImage);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
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
                        string path = saveDialog.FolderName + $"/{fileName.Text}{count}";
                        saveImage(x, y, path);
                        count++;
                    }
                }

                if (onsave_explorer_checkbox.IsChecked == true)
                {
                    Process.Start("explorer.exe", saveDialog.FolderName);
                }
            }
        }

        private void saveImage(int x, int y, string path)
        {
            BitmapSource bitmap = generatedImages[x, y];


            using (FileStream stream = File.Create(path + ".png"))
            {
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Clear();
                pngEncoder.Frames.Add(BitmapFrame.Create(bitmap));
                pngEncoder.Save(stream);
            }

        }
        [StructLayout(LayoutKind.Explicit)]
        public struct PixelColor
        {
            // 32 bit BGRA 
            [FieldOffset(0)] public UInt32 ColorBGRA;
            // 8 bit components
            [FieldOffset(0)] public byte Blue;
            [FieldOffset(1)] public byte Green;
            [FieldOffset(2)] public byte Red;
            [FieldOffset(3)] public byte Alpha;
        }
        public PixelColor[,] GetPixels(BitmapSource source)
        {
            if (source.Format != PixelFormats.Bgra32)
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

            int width = source.PixelWidth;
            int height = source.PixelHeight;


            PixelColor[,] result = new PixelColor[width, height];

            var unsortedByteResults = new byte[height * width * 4];

            source.CopyPixels(unsortedByteResults, width * 4, 0);
            int count = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result[x, y] = new PixelColor
                    {
                        Blue = unsortedByteResults[count + 0],
                        Green = unsortedByteResults[count + 1],
                        Red = unsortedByteResults[count + 2],
                        Alpha = unsortedByteResults[count + 3]
                    };
                    count += 4;
                }
            }

            return result;
        }

        public BitmapSource GetBitmapSource(Bitmap bitmap)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap
            (
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            return bitmapSource;
        }
    }
}
