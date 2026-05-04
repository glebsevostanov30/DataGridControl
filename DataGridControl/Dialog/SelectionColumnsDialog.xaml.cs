using System.ComponentModel;
using System.Windows;
using DataGridControl.Model;
using DataGridControl.View.Dialog;

namespace DataGridControl.Dialog
{
    public partial class SelectionColumnsDialog : Window
    {
        public SelectionColumnsDialog(SpreadsheetModel mainModel)
        {
            InitializeComponent();
            DataContext = new SelectionColumnsDialogViewModel(mainModel);
        }

        private void ButtonBaseOk_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ButtonBaseClose_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void SelectionColumnsDialog_OnClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void SelectionColumnsDialog_OnClosed(object? sender, EventArgs e)
        {
            Hide();
        }
    }
}