using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors;

public class DataGridCellsBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty CellProperty =
        DependencyProperty.RegisterAttached(
            nameof(Cells),
            typeof(IList<DataGridCellInfo>),
            typeof(DataGridCellsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public IList<DataGridCellInfo> Cells
    {
        get => (IList<DataGridCellInfo>)GetValue(CellProperty);
        set => SetValue(CellProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectedCellsChanged += OnSourceCollectionChanged;
    }
    
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.SelectedCellsChanged -= OnSourceCollectionChanged;
    }

    private void OnSourceCollectionChanged(object sender, SelectedCellsChangedEventArgs e)
    {
        Cells = AssociatedObject.SelectedCells;
        // foreach (var item in e.RemovedCells) Cells?.Remove(item);
        // foreach (var item in e.AddedCells) Cells?.Add(item);
    }
}