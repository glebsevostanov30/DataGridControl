using System.Collections.ObjectModel;

namespace DataGridControl.Model;

public class SpreadsheetModel
{
    public ObservableCollection<RowData> SelectedRows { get; set; } = [];
    
    public bool IsUndoRedoInProgress
    {
        get;
        set
        {
            field = value;
            // OnPropertyChanged();
            // CommandManager.InvalidateRequerySuggested();
        }
    }
    
    public ObservableCollection<RowData> Rows { get; } =
    [
        new() { Col1 = "A1", Col2 = "B1", Col3 = "C1" },
        new() { Col1 = "A2", Col2 = "B2", Col3 = "C2" },
        new() { Col1 = "A3", Col2 = "B3", Col3 = "C3" }
    ];
    
    // public ObservableCollection<ColumnData> Columns { get; }

    // Columns = new ObservableCollection<ColumnData>();
    // // Инициализация демо-данными
    // Columns.Add(new ColumnData { Header = "Col1", PropertyName = "Col1" });
    // Columns.Add(new ColumnData { Header = "Col2", PropertyName = "Col2" });
    // Columns.Add(new ColumnData { Header = "Col3", PropertyName = "Col3" });
}