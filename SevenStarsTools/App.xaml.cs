using System.Windows;
using System.Windows.Media.Imaging;

namespace SevenStarsTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }

        public static void CopyImageToClipboard(BitmapImage image)
        {
            // Non transparancy
            Clipboard.SetImage(image);
        }
    }

}
