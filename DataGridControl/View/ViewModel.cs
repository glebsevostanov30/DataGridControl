using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace DataGridControl.View;

public class ViewModel: INotifyPropertyChanged
{
    public DataTable TableData { get; set; } = new();
    public ObservableCollection<DataRowView> SelectedRows { get; set; } = new();

    public ViewModel()
    {
        
        AddColumn<int>("ID");
        AddColumn<string>("Username");
        AddColumn<string>("Mail");
        
        for (int i = 0; i < 5; i++)
        {
            AddColumn<string>(i.ToString());
        }
        
        
        for (int i = 0; i < 10; i++)
        {
            AddRow(i, "Me", "me@mail.com");    
        }
        
        TableData =  TableData.Copy();
        TableData.AcceptChanges();
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
        if(row== null) return;
        row.Delete();
        OnPropertyChanged(nameof(TableData));
    }
    
    public void RemoveColumn(DataColumn? dataColumn)
    {
        if (dataColumn == null) return;
        TableData.Columns.Remove(dataColumn);
        OnPropertyChanged(nameof(TableData));
    }

    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}