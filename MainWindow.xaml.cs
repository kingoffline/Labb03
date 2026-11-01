using L046_Labb3_Code_along.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace L046_Labb3_Code_along
{
    public partial class MainWindow : Window
    {
        private bool _isFullscreen;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        public void ShowPlayerView()
        {
            ConfigurationViewControl.Visibility = Visibility.Collapsed;
            PlayerViewControl.Visibility = Visibility.Visible;
        }

        public void ShowConfigurationView()
        {
            PlayerViewControl.Visibility = Visibility.Collapsed;
            ConfigurationViewControl.Visibility = Visibility.Visible;
        }

        public void ToggleFullscreen()
        {
            if (_isFullscreen)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }

            _isFullscreen = !_isFullscreen;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key == Key.System ? e.SystemKey : e.Key;

            if (Keyboard.Modifiers == ModifierKeys.Control && key == Key.P)
            {
                ShowPlayerView();
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && key == Key.E)
            {
                ShowConfigurationView();
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Alt && key == Key.Enter)
            {
                ToggleFullscreen();
                e.Handled = true;
            }
        }
    }
}
