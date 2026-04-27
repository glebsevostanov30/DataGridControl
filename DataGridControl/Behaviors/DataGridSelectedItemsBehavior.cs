using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors;

public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register(nameof(SelectedItems), typeof(IList),
            typeof(DataGridSelectedItemsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemsChanged));

    public IList SelectedItems
    {
        get => (IList)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += OnDataGridSelectionChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.SelectionChanged -= OnDataGridSelectionChanged;
    }

    private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
    {
        if (d is not DataGridSelectedItemsBehavior { AssociatedObject: not null } behavior) return;
        
        if (args.OldValue is INotifyCollectionChanged oldCollection)
            oldCollection.CollectionChanged -= behavior.OnSourceCollectionChanged;
        
        if (args.NewValue is INotifyCollectionChanged newCollection)
            newCollection.CollectionChanged += behavior.OnSourceCollectionChanged;
    }

    private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItems = SelectedItems;

        foreach (var item in e.RemovedItems) selectedItems.Remove(item);
        foreach (var item in e.AddedItems) selectedItems.Add(item);
    }

    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var dataGrid = AssociatedObject;
        if (dataGrid == null) return;

        if (e.OldItems != null)
            foreach (var item in e.OldItems)
                dataGrid.SelectedItems.Remove(item);
        if (e.NewItems == null) return;
        {
            foreach (var item in e.NewItems)
                dataGrid.SelectedItems.Add(item);
        }
    }
}