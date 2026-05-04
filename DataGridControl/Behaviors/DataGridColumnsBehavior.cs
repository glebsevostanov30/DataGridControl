using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors;

public class DataGridColumnsBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.RegisterAttached(
            nameof(Columns),
            typeof(ObservableCollection<DataGridColumn>),
            typeof(DataGridColumnsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnBindableColumnsChanged));

    public ObservableCollection<DataGridColumn> Columns
    {
        get => (ObservableCollection<DataGridColumn>)GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Columns.CollectionChanged += OnSourceCollectionChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Columns.CollectionChanged -= OnSourceCollectionChanged;
    }

    private static void OnBindableColumnsChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
        if (source is not DataGridColumnsBehavior { AssociatedObject: not null } behavior) return;
        
        if (e.OldValue is ObservableCollection<DataGridColumn> oldCollection)
            oldCollection.CollectionChanged -= behavior.OnSourceCollectionChanged;

        if (e.NewValue is ObservableCollection<DataGridColumn> newCollection)
        {
            newCollection.CollectionChanged += behavior.OnSourceCollectionChanged;
        }
    }
    
    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs ne)
    {
        switch (ne.Action)
        {
            case NotifyCollectionChangedAction.Add when ne.NewItems is not null:
                foreach (DataGridColumn column in ne.NewItems)
                    Columns.Insert(ne.NewStartingIndex, column);
                break;
            case NotifyCollectionChangedAction.Remove when ne.OldItems is not null:
                foreach (DataGridColumn column in ne.OldItems)
                    Columns.Remove(column);
                break;
            case NotifyCollectionChangedAction.Move:
                Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);
                break;
            case NotifyCollectionChangedAction.Replace when ne.NewItems is not null:
                Columns[ne.NewStartingIndex] = ne.NewItems[0] as DataGridColumn ?? new DataGridTextColumn();
                break;
            case NotifyCollectionChangedAction.Reset:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}