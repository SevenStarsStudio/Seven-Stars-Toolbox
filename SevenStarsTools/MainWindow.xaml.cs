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

namespace SevenStarsTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Window> windows;

        public MainWindow()
        {
            InitializeComponent();
            windows = new List<Window>();
        }

        private void btnClick_DoorMaker(object sender, RoutedEventArgs e)
        {
            CreateNewWindow<DoorGenerator>();
        }

        private void btnClick_BannerGenerator(object sender, RoutedEventArgs e)
        {
            CreateNewWindow<BannerGenerator>();
        }

        private void CreateNewWindow<T>() where T : new()
        {
            Window? window = new T() as Window;
            if(window != null)
            {
                window.ResizeMode = ResizeMode.CanMinimize;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                window.Show();
                windows.Add(window);
            }
        }

        private void btnClick_CloseAll(object sender, RoutedEventArgs e)
        {
            foreach(Window window in windows)
            {
                window.Close();
            }
            windows.Clear();
        }
    }
}