using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using DataGridControl.Model;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors;

public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty SelectedRowsProperty =
        DependencyProperty.Register(
            nameof(SelectedRows),
            typeof(ObservableCollection<DataRowView>),
            typeof(DataGridSelectedItemsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public ObservableCollection<DataRowView> SelectedRows
    {
        get => (ObservableCollection<DataRowView>)GetValue(SelectedRowsProperty);
        set => SetValue(SelectedRowsProperty, value);
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

    private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (DataRowView item in e.RemovedItems) SelectedRows?.Remove(item);
        foreach (DataRowView item in e.AddedItems) SelectedRows?.Add(item);
    }
    
}