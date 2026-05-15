// using System.Collections.Specialized;
// using System.ComponentModel;
// using System.Windows;
// using System.Windows.Controls;
// using System.Windows.Data;
// using DataGridControl.Model;
// using Microsoft.Xaml.Behaviors;
//
// namespace DataGridControl.Behaviors;
//
// public class DataGridColumnsBehavior : Behavior<DataGrid>
// {
//     public static readonly DependencyProperty ColumnsSourceProperty =
//         DependencyProperty.Register(
//             nameof(ColumnsSource),
//             typeof(IList<ColumnDescriptor>),
//             typeof(DataGridColumnsBehavior),
//             new FrameworkPropertyMetadata(
//                 null,
//                 FrameworkPropertyMetadataOptions.None, OnColumnsSourcePropertyChanged));
//     
//
//     public IList<ColumnDescriptor> ColumnsSource
//     {
//         get => (IList<ColumnDescriptor>)GetValue(ColumnsSourceProperty);
//         set => SetValue(ColumnsSourceProperty, value);
//     }
//     
//     private static void OnColumnsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//     {
//         if (d is DataGridColumnsBehavior behavior)
//         {
//             behavior.InitializeColumns(e.NewValue as IList<ColumnDescriptor>);
//         }
//     }
//
//     private void InitializeColumns(IList<ColumnDescriptor>? newColumns)
//     {
//         var grid = AssociatedObject;
//         if (grid == null) return;
//         if (newColumns == null) return;
//
//
//         // 2. Генерация колонок
//         for (var i = 0; i < newColumns.Count; i++)
//         {
//             var colDesc = newColumns[i];
//             var dataCol = new DataGridTextColumn
//             {
//                 Header = colDesc.Header,
//                 Binding = new Binding($"Values[{i}]") { Mode = BindingMode.TwoWay },
//             };
//
//             // 3. Настройка видимости через BindingOperations
//             var converter = new BooleanToVisibilityConverter();
//             var visBinding = new Binding($"Columns[{i}].IsVisible")
//             {
//                 Source = grid.DataContext,
//                 Converter = converter
//             };
//
//             // ✅ Ключевая строка: используем статический класс BindingOperations
//             BindingOperations.SetBinding(dataCol, DataGridColumn.VisibilityProperty, visBinding);
//
//             grid.Columns.Add(dataCol);
//
//             // 4. Подписка на изменения дескриптора
//             if (colDesc is INotifyPropertyChanged npc)
//                 npc.PropertyChanged += Column_PropertyChanged;
//         }
//
//         // 5. Подписка на изменения коллекции
//         if (newColumns is INotifyCollectionChanged newColl)
//             newColl.CollectionChanged += Grid_ColumnsChanged;
//     }
//
//     // ✅ 4. Обработчик изменения свойств отдельной колонки (например, IsVisible)
//     private void Column_PropertyChanged(object? sender, PropertyChangedEventArgs e)
//     {
//         if (e.PropertyName == nameof(ColumnDescriptor.IsVisible) &&
//             sender is ColumnDescriptor cd &&
//             AssociatedObject != null)
//         {
//             // Находим индекс дескриптора в исходной коллекции
//             var index = ColumnsSource?.IndexOf(cd) ?? -1;
//             if (index >= 0 && index < AssociatedObject.Columns.Count)
//             {
//                 // Обновляем видимость соответствующей колонки в DataGrid
//                 var targetCol = AssociatedObject.Columns[index];
//                 targetCol.Visibility = cd.IsVisible ? Visibility.Visible : Visibility.Collapsed;
//             }
//         }
//     }
//
//     private void Grid_ColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e)
//     {
//         var asdf = e.NewItems;
//         var asdf2 = asdf[0] as ColumnDescriptor;
//         var asdf1 = e.NewItems as IList<ColumnDescriptor>;
//         InitializeColumns(asdf1);
//     }
//
//     protected override void OnAttached()
//     {
//         base.OnAttached();
//         InitializeColumns(ColumnsSource);
//     }
//
//     protected override void OnDetaching()
//     {
//         base.OnDetaching();
//         if (ColumnsSource is INotifyCollectionChanged coll)
//             coll.CollectionChanged -= Grid_ColumnsChanged;
//         foreach (var col in ColumnsSource)
//             if (col is INotifyPropertyChanged npc)
//                 npc.PropertyChanged -= Column_PropertyChanged;
//     }
// }