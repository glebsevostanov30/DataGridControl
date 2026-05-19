using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors;

public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty SelectedRowsProperty =
        DependencyProperty.Register(
            nameof(SelectedRows),
            typeof(IList<DataRowView>),
            typeof(DataGridSelectedItemsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public IList<DataRowView> SelectedRows
    {
        get => (IList<DataRowView>)GetValue(SelectedRowsProperty);
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
        SelectedRows = AssociatedObject.SelectedItems.Cast<DataRowView>().ToList();
    }
}