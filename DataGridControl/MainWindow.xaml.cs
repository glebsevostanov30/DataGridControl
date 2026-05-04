using System.Windows;

namespace DataGridControl;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var asdf = MainDataGrid.SelectedItems;
        var asdf1 = MainDataGrid.CurrentColumn;
        Console.WriteLine("asdf");
    }
}