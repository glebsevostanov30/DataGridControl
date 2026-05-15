// using System.Collections.ObjectModel;
// using System.Windows;
// using System.Windows.Controls;
// using DataGridControl.Model;
// using Microsoft.Xaml.Behaviors;
//
// namespace DataGridControl.Behaviors;
//
// public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
// {
//     public static readonly DependencyProperty SelectedRowsProperty =
//         DependencyProperty.Register(
//             nameof(SelectedRows),
//             typeof(ObservableCollection<RowRecord>),
//             typeof(DataGridSelectedItemsBehavior),
//             new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
//
//     public ObservableCollection<RowRecord> SelectedRows
//     {
//         get => (ObservableCollection<RowRecord>)GetValue(SelectedRowsProperty);
//         set => SetValue(SelectedRowsProperty, value);
//     }
//
//     protected override void OnAttached()
//     {
//         base.OnAttached();
//         AssociatedObject.SelectionChanged += OnDataGridSelectionChanged;
//     }
//
//     protected override void OnDetaching()
//     {
//         base.OnDetaching();
//         AssociatedObject.SelectionChanged -= OnDataGridSelectionChanged;
//     }
//
//     private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
//     {
//         foreach (RowRecord item in e.RemovedItems) SelectedRows?.Remove(item);
//         foreach (RowRecord item in e.AddedItems) SelectedRows?.Add(item);
//     }
//     
// }