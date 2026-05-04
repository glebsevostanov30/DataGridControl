using System.Data;
using System.Windows;
using System.Windows.Media;

namespace DataGridControl.Model.Dialog;

public class SheetItem
{
    public required DataTable Table { get; init; }
    public required string DisplayText { get; init; }
    public required FontWeight FontWeight { get; init; }
    public required SolidColorBrush TextColor { get; init; }
}