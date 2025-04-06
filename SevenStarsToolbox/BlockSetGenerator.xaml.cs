using SevenStarsToolbox.Data;
using System.Windows;
using System.Windows.Controls;

namespace SevenStarsToolbox
{
    /// <summary>
    /// Interaction logic for BlockSetGenerator.xaml
    /// </summary>
    public partial class BlockSetGenerator : Window
    {

        Dictionary<BlockPresets, List<CheckBox>> checkboxesByPresets = new Dictionary<BlockPresets, List<CheckBox>>();
        BlockPresets currentPreset = BlockPresets.CUSTOM;

        public BlockSetGenerator()
        {
            InitializeComponent();
            version.Text = App.VERSION;

        }

        private void ApplyPreset(BlockPresets newPreset)
        {
            this.currentPreset = newPreset;


        }

        private void PresetSelectionUpdate(object sender, SelectionChangedEventArgs e)
        {
            if(MyListView != null)
                MyListView.Items.Add(new CheckBox()
                {
                    Name = "test",
                    Content = $"{fileName.Text} thing"
                });
        }

        private void btnClick_selectAll(object sender, RoutedEventArgs e)
        {

        }

        private void btnClick_deselectAll(object sender, RoutedEventArgs e)
        {

        }

        protected void UpdateName(object sender, EventArgs e)
        {

        }

        private void btnClick_save(object sender, RoutedEventArgs e)
        {

        }
    }
}
