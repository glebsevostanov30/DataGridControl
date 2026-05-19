using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using DataGridControl.View;

namespace DataGridControl;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    SpreadsheetViewModel spreadsheetViewModel = new SpreadsheetViewModel();
    public MainWindow()
    {
        InitializeComponent();
        spreadsheetViewModel = new SpreadsheetViewModel();
        DataContext = spreadsheetViewModel;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("");
    }
}