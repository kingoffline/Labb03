using L046_Labb3_Code_along.ViewModels;
using System.Windows;

namespace L046_Labb3_Code_along.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewPackDialog.xaml
    /// </summary>
    public partial class CreateNewPackDialog : Window
    {
        public CreateNewPackDialog()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateNewPackDialogViewModel vm &&
                !vm.Validate(out var error))
            {
                MessageBox.Show(this, error, "New Question Pack",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}
