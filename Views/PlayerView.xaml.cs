using System.Windows;
using System.Windows.Controls;

namespace L046_Labb3_Code_along.Views
{
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
                mainWindow.ShowPlayerView();
        }
    }
}