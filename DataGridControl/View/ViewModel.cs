using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DataGridControl.View;

public class ViewModel: INotifyPropertyChanged
{
    public DataTable TableData
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = new();

    public IList<DataRowView> SelectedRows { get; set; } = new List<DataRowView>();
    public IList<DataGridCellInfo> SelectedCells { get; set; } = new List<DataGridCellInfo>();

    public ViewModel()
    {
        
        AddColumn<int>("ID");
        AddColumn<string>("Username");
        AddColumn<string>("Mail");
        
        for (int i = 0; i < 50; i++)
        {
            AddColumn<string>(i.ToString());
        }
        
        
        for (int i = 0; i < 1_000; i++)
        {
            AddRow(i, "Me", "me@mail.com");    
        }
        
        TableData.PrimaryKey = new[] { TableData.Columns["Id"] };
        
        TableData =  TableData.Copy();
        TableData.AcceptChanges();
    }

    public void RenameColumn(string oldName, string newName)
    {
        var dataTableCopy = TableData.Copy();
        dataTableCopy.Columns[oldName]?.ColumnName = newName;
        TableData = dataTableCopy;
        OnPropertyChanged(nameof(TableData));
    }
    

    public void AddColumn<TData>(string columnName, int columnIndex = -1)
    {
        var dataTableCopy = TableData.Copy();
        var newColumn = new DataColumn(columnName, typeof(TData));
        
        dataTableCopy.Columns.Add(newColumn);
        if (columnIndex > -1)
        {
            newColumn.SetOrdinal(columnIndex);
        }
        
        var newColumnIndex = dataTableCopy.Columns.IndexOf(newColumn);
        
        foreach (DataRow row in dataTableCopy.Rows)
        {
            row[newColumnIndex] = default(TData);
        }

        TableData = dataTableCopy;
        OnPropertyChanged(nameof(TableData));
    }

    public void ReturnRow(DataRowView? row)
    {
        if (row == null) return;
        row.Row.RejectChanges();
        OnPropertyChanged(nameof(TableData));
    }

    public void AddRow(params object?[]? columnValues)
    {
        var rowModelWithCurrentColumns = TableData.NewRow();
        TableData.Rows.Add(rowModelWithCurrentColumns);

        if (columnValues == null) return;
        
        for (var columnIndex = 0; columnIndex < TableData.Columns.Count; columnIndex++)
        {
            if(columnValues.Length - 1 < columnIndex) break;
            if(columnValues[columnIndex] == null) continue;
            
            rowModelWithCurrentColumns[columnIndex] = columnValues[columnIndex];
        }
        
        OnPropertyChanged(nameof(TableData));
    }
    
    public void RemoveRow(DataRowView? row)
    {
        row?.Delete();
    }
    
    public void RemoveAllRow(DataRowView? row)
    {
    }
    
    public void CopyUnselectedRows(IList<DataRowView> selectedRows)
    {
        var selectedSet = new HashSet<DataRow>(selectedRows.Select(r => r.Row));

        var result = TableData.Clone();

        foreach (DataRow row in TableData.Rows)
        {
            if (!selectedSet.Contains(row))
                result.ImportRow(row);
        }

        TableData = result;
        OnPropertyChanged(nameof(TableData));
    }
    
    public void ReturnDataTable(DataTable dataTable)
    {
        TableData = dataTable;
        OnPropertyChanged(nameof(TableData));
    }
    
    public void RemoveColumn(DataGridColumn? dataColumn)
    {
        if (dataColumn == null) return;
        var dataTableCopy = TableData.Copy();
        dataTableCopy.Columns.Remove(dataColumn.Header.ToString() ?? string.Empty);
        TableData = dataTableCopy;
        OnPropertyChanged(nameof(TableData));
    }

    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}