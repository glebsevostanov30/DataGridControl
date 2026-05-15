using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace DataGridControl.View;

public class VeiwModel: INotifyPropertyChanged
{
    public DataTable TableData { get; set; }

    public VeiwModel()
    {
        var changedTable = new DataTable();
        
        AddColumn<int>("ID", changedTable);
        AddColumn<string>("Username", changedTable);
        AddColumn<string>("Mail", changedTable);
        
        for (int i = 0; i < 100; i++)
        {
            AddColumn<string>(i.ToString(), changedTable);
        }

        
        for (int i = 0; i < 10_000; i++)
        {
            AddRow(changedTable, i, "Me", "me@mail.com");    
        }
        TableData =  changedTable;
    }

    // Appends a new column. 
    // Use 'columnIndex' parameter to assign an other column index than the last
    public void AddColumn<TData>(string columnName, DataTable targetDataTable, int columnIndex = -1)
    {
        var newColumn = new DataColumn(columnName, typeof(TData));

        targetDataTable.Columns.Add(newColumn);
        if (columnIndex > -1)
        {
            newColumn.SetOrdinal(columnIndex);
        }

        var newColumnIndex = targetDataTable.Columns.IndexOf(newColumn);

        // Initialize existing rows with default value for the new column
        foreach (DataRow row in targetDataTable.Rows)
        {
            row[newColumnIndex] = default(TData);
        }
        
        OnPropertyChanged(nameof(TableData));
    }

    public void AddRow(DataTable targetDataTable, params object[]? columnValues)
    {
        var rowModelWithCurrentColumns = targetDataTable.NewRow();
        targetDataTable.Rows.Add(rowModelWithCurrentColumns);

        if (columnValues == null) return;
        if (columnValues.Length != targetDataTable.Columns.Count) return;
        
        for (var columnIndex = 0; columnIndex < targetDataTable.Columns.Count; columnIndex++)
        {
            rowModelWithCurrentColumns[columnIndex] = columnValues[columnIndex];
        }
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