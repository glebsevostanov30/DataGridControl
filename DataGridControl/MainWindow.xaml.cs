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
    public MainWindow()
    {
        InitializeComponent();
        var spreadsheetViewModel = new SpreadsheetViewModel();
        DataContext = spreadsheetViewModel;

    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {

    }

    private void RenameHeaderButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var header = FindAncestor<DataGridColumnHeader>(button);
        var column = header?.Column;

        if (column == null) return;

        // 📝 Пример ввода нового имени (в реальном проекте используйте кастомное Dialog-окно)
        string newName = Microsoft.VisualBasic.Interaction.InputBox("Введите новое название:", "Переименование", column.Header.ToString());
    
        if (!string.IsNullOrWhiteSpace(newName))
        {
            column.Header = newName;
        }
    }

// 🔍 Вспомогательный метод для обхода визуального дерева
    private static T FindAncestor<T>(DependencyObject child) where T : DependencyObject
    {
        DependencyObject parent = VisualTreeHelper.GetParent(child);
        while (parent != null)
        {
            if (parent is T ancestor) return ancestor;
            parent = VisualTreeHelper.GetParent(parent);
        }
        return null;
    }
}