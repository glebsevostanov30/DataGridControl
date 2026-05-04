using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DataGridControl.Model;

public class SpreadsheetModel
{
    public ObservableCollection<RowData> SelectedRows { get; set; } = [];
    public ObservableCollection<DataGridColumn> Columns { get; set; } = [];
    
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
   
}