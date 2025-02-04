using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace SevenStarsToolbox
{
    public static class ImageUtils
    {
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
        public static PixelColor[,] GetPixels(BitmapSource source)
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


        public static Dictionary<PixelColor, List<Vector2>> GenerateColorGrid(Dictionary<PixelColor, List<Vector2>> dictionaryToUse, BitmapImage source)
        {
            dictionaryToUse.Clear();

            PixelColor[,] pixels = GetPixels(source);

            int width = source.PixelWidth;
            int height = source.PixelHeight;

            for (int pixelX = 0; pixelX < width; pixelX++)
            {
                for (int pixelY = 0; pixelY < height; pixelY++)
                {
                    PixelColor pixelColor = pixels[pixelX, pixelY];
                    if (pixelColor.Alpha == 0)
                        continue;

                    if (!dictionaryToUse.ContainsKey(pixelColor))
                    {
                        dictionaryToUse.Add(pixelColor, new List<Vector2> {
                            new Vector2(pixelX, pixelY)
                        });
                    }
                    else
                    {
                        dictionaryToUse[pixelColor].Add(new Vector2(pixelX, pixelY));
                    }
                }
            }
            return dictionaryToUse;
        }

        internal static BitmapImage Convert(BitmapImage bitmapImage, Dictionary<PixelColor, List<Vector2>> sourceGrids, Dictionary<PixelColor, List<Vector2>> templateGrids)
        {
            int width = bitmapImage.PixelWidth;
            int height = bitmapImage.PixelHeight;

            Bitmap myBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            myBitmap.SetResolution(width, height);

            PixelColor[,] pixels = GetPixels(bitmapImage);

            foreach(PixelColor color in sourceGrids.Keys)
            {
                List<Vector2> sourcePositions = sourceGrids[color];
                List<Vector2> templatePositions = templateGrids[color];

                for (int i = 0; i < sourcePositions.Count; i++)
                {
                    Vector2 sourcePosition = sourcePositions[i];
                    PixelColor pixelColor = pixels[(int)sourcePosition.X, (int)sourcePosition.Y];

                    Vector2 templatePosition = templatePositions[i];


                    myBitmap.SetPixel((int)templatePosition.X, (int)templatePosition.Y, Color.FromArgb(pixelColor.Alpha, pixelColor.Red, pixelColor.Green, pixelColor.Blue));
                }
            }

            return GetBitmapSource(myBitmap);
        }

        

        public static BitmapImage GetBitmapSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static void SaveBitmapImage(string path, BitmapImage bitmap)
        {

            using (FileStream stream = File.Create(path))
            {
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Clear();
                pngEncoder.Frames.Add(BitmapFrame.Create(bitmap));
                pngEncoder.Save(stream);
            }
        }
    }
}
