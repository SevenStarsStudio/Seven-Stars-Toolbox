using System.Windows;

namespace SevenStarsToolbox
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
            version.Text = App.VERSION;
        }

        private void OpenBannerGenerator(object sender, RoutedEventArgs e)
        {
            CreateNewWindow<BannerGenerator>();
        }
        private void OpenDoorGenerator(object sender, RoutedEventArgs e)
        {
            CreateNewWindow<DoorGenerator>();
        }
        private void OpenBlockSetGenerator(object sender, RoutedEventArgs e)
        {
            CreateNewWindow<BlockSetGenerator>();
        }


        private void ReportIssue(object sender, RoutedEventArgs e)
        {
            string url = "https://github.com/SevenStarsStudio/Seven-Stars-Tools/issues/new?template=Blank+issue";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
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