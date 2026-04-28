using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors;

public class DataGridColumnsBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty BindableColumnsProperty =
        DependencyProperty.RegisterAttached("BindableColumns",
            typeof(ObservableCollection<DataGridColumn>),
            typeof(DataGridColumnsBehavior),
            new UIPropertyMetadata(null, OnBindableColumnsChanged));

    public static void SetBindableColumns(DependencyObject obj, ObservableCollection<DataGridColumn> value) =>
        obj.SetValue(BindableColumnsProperty, value);

    public static ObservableCollection<DataGridColumn> GetBindableColumns(DependencyObject obj) =>
        (ObservableCollection<DataGridColumn>)obj.GetValue(BindableColumnsProperty);
    

    private static void OnBindableColumnsChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
        if (source is not DataGrid dataGrid) return;
        if (e.OldValue is ObservableCollection<DataGridColumn> oldCollection)
            oldCollection.CollectionChanged -= OnCollectionChanged;

        if (e.NewValue is ObservableCollection<DataGridColumn> newCollection)
        {
            dataGrid.Columns.Clear();
            foreach (var column in newCollection)
                dataGrid.Columns.Add(column);

            newCollection.CollectionChanged += OnCollectionChanged;
            dataGrid.Columns.CollectionChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs ne)
        {
            // Здесь мы ловим изменения в коллекции (добавление, удаление, перемещение)
            // и вносим их в UI
            switch (ne.Action)
            {
                case NotifyCollectionChangedAction.Add when ne.NewItems is not null:
                    foreach (DataGridColumn column in ne.NewItems)
                        dataGrid.Columns.Insert(ne.NewStartingIndex, column);
                    break;
                case NotifyCollectionChangedAction.Remove when ne.OldItems is not null:
                    foreach (DataGridColumn column in ne.OldItems)
                        dataGrid.Columns.Remove(column);
                    break;
                case NotifyCollectionChangedAction.Move:
                    dataGrid.Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace when ne.NewItems is not null:
                    dataGrid.Columns[ne.NewStartingIndex] = ne.NewItems[0] as DataGridColumn;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}