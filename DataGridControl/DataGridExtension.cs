using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataGridControl.Model.Test;

namespace DataGridControl;

public static class DataGridExtension
{
    public static void BindToDataGrid(this DataGrid grid, object? sender, NotifyCollectionChangedEventArgs e)
    {
        // // grid.Columns.Clear();
        // if (e.NewItems == null) return;
        // foreach (ColumnDescriptor col in e.NewItems)
        // {
        //     var textCol = new DataGridTextColumn
        //     {
        //         Header = col.Header,
        //         Binding = new Binding($"[{col.Header}].Value")
        //         {
        //             Mode = BindingMode.TwoWay,
        //             UpdateSourceTrigger = UpdateSourceTrigger.LostFocus // Стабильнее для редактирования
        //         },
        //         SortMemberPath = $"[{col.Header}].Value"
        //     };
        //
        //     // Привязка видимости к дескриптору
        //     var binding = new Binding($"[{col.Header}].IsVisible")
        //     {
        //         Source = col,
        //         Converter = new BooleanToVisibilityConverter(),
        //     };
        //
        //     BindingOperations.SetBinding(textCol, DataGridColumn.VisibilityProperty, binding);
        //
        //
        //     grid.Columns.Add(textCol);
        // }
    }

    public static void BindToDataGrid(this DataGrid grid, DynamicTable vm)
    {
        // grid.Columns.Clear();
        // for (var i = 0; i < vm.Columns.Count; i++)
        // {
        //     var col = vm.Columns[i];
        //     var dataCol = new DataGridTextColumn
        //     {
        //         Header = col.Header,
        //         Binding = new Binding($"Values[{i}]")
        //         {
        //             Mode = BindingMode.TwoWay,
        //             UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
        //         },
        //         Visibility = col.IsVisible ? Visibility.Visible : Visibility.Collapsed
        //     };
        //
        //     // Привязка видимости к дескриптору
        //     var binding = new Binding($"Columns[{i}].IsVisible")
        //     {
        //         Source = vm,
        //         Converter = new BooleanToVisibilityConverter(),
        //     };
        //     BindingOperations.SetBinding(dataCol, DataGridColumn.VisibilityProperty, binding);
        //
        //     grid.Columns.Add(dataCol);
        // }
    }
}