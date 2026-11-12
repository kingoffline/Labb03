using System.Windows;
using System.Windows.Input;
using L046_Labb3_Code_along.Dialogs;
using L046_Labb3_Code_along.ViewModels;

namespace L046_Labb3_Code_along
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        private bool _isFullscreen;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel = new MainWindowViewModel();
            _viewModel.CreateNewPackRequested += OnCreateNewPackRequested;
            _viewModel.DeletePackRequested += OnDeletePackRequested;
            _viewModel.ImportQuestionsRequested += OnImportQuestionsRequested;
            _viewModel.PackOptionsRequested += OnPackOptionsRequested;
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

            if (Keyboard.Modifiers == ModifierKeys.Control && key == Key.N)
            {
                _viewModel.NewPackCommand.Execute(null);
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && key == Key.O)
            {
                _viewModel.PackOptionsCommand.Execute(null);
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && key == Key.I)
            {
                _viewModel.ImportQuestionsCommand.Execute(null);
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && key == Key.P)
            {
                ShowPlayerView();
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && key == Key.E)
            {
                ShowConfigurationView();
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.None && key == Key.Insert)
            {
                if (Keyboard.FocusedElement is not System.Windows.Controls.Primitives.TextBoxBase)
                {
                    _viewModel.AddQuestionCommand.Execute(null);
                    e.Handled = true;
                }
            }
            else if (Keyboard.Modifiers == ModifierKeys.None && key == Key.Delete)
            {
                if (Keyboard.FocusedElement is not System.Windows.Controls.Primitives.TextBoxBase)
                {
                    _viewModel.RemoveQuestionCommand.Execute(null);
                    e.Handled = true;
                }
            }
            else if (Keyboard.Modifiers == ModifierKeys.Alt && key == Key.Enter)
            {
                ToggleFullscreen();
                e.Handled = true;
            }
        }

        private void OnCreateNewPackRequested(object? sender, EventArgs e)
        {
            var dialogViewModel = new CreateNewPackDialogViewModel();

            var dialog = new CreateNewPackDialog
            {
                DataContext = dialogViewModel,
                Owner = this
            };

            if (dialog.ShowDialog() == true)
                _viewModel.AddPack(dialogViewModel.CreatePack());
        }

        private void OnDeletePackRequested(object? sender, EventArgs e)
        {
            if (_viewModel.ActivePack is null)
                return;

            var result = MessageBox.Show(
                this,
                $"Delete '{_viewModel.ActivePack.Name}' and all its questions?",
                "Delete Question Pack",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
                _viewModel.DeleteActivePack();
        }

        // Ofärdigt. bara Demo!
        private void OnImportQuestionsRequested(object? sender, EventArgs e)
        {
            MessageBox.Show(
                this,
                "Det är inte byggt än, plz ha tållamod :)",
                "Import Questions",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void OnPackOptionsRequested(object? sender, EventArgs e)
        {
            if (_viewModel.ActivePack is null)
                return;

            var dialogViewModel = new PackOptionsDialogViewModel(_viewModel.ActivePack);

            var dialog = new PackOptionsDialog
            {
                DataContext = dialogViewModel,
                Owner = this
            };

            if (dialog.ShowDialog() == true)
                dialogViewModel.ApplyTo(_viewModel.ActivePack);
        }
    }
}