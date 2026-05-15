using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataGridControl.Model.Test;
using DataGridControl.View;

namespace DataGridControl;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()     
    {
        InitializeComponent();
        var spreadsheetViewModel = new SpreadsheetViewModel();
        DataContext = spreadsheetViewModel;
        MainDataGrid.BindToDataGrid(spreadsheetViewModel.model);
        spreadsheetViewModel.model.Columns.CollectionChanged += MainDataGrid.BindToDataGrid;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {

    }
}