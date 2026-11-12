using L046_Labb3_Code_along.ViewModels;
using System.Windows;

namespace L046_Labb3_Code_along.Dialogs
{
    public partial class PackOptionsDialog : Window
    {
        public PackOptionsDialog()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PackOptionsDialogViewModel vm &&
                !vm.Validate(out var error))
            {
                MessageBox.Show(this, error, "Pack Options",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}