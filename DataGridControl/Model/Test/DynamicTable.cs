using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DataGridControl.Model.Test;

public class DynamicTable
{
    public readonly ObservableCollection<ColumnDescriptor> Columns = new();
    public readonly ObservableCollection<RowRecord> Rows = [];
    
    private readonly Dictionary<string, int> _rowIdToIndex = new();
    private readonly Dictionary<string, int> _colIdToIndex = new();

    public int ColumnCount => Columns.Count;
    public int RowCount => Rows.Count;

    // Быстрый доступ по индексам (для DataGrid биндинга)
    public object this[int row, int col] => Rows[row].Values[col];

    // Доступ по ID
    public object GetCell(string rowId, string colId) =>
        _rowIdToIndex.TryGetValue(rowId, out var r) && _colIdToIndex.TryGetValue(colId, out var c)
            ? Rows[r].Values[c] : null;

    public void SetCell(string rowId, string colId, object value)
    {
        if (_rowIdToIndex.TryGetValue(rowId, out var r) && _colIdToIndex.TryGetValue(colId, out var c))
            Rows[r].Values[c] = value;
    }

    public void AddColumn(string id, string header)
    {
        _colIdToIndex[id] = Columns.Count;
        Columns.Add(new ColumnDescriptor { Id = id, Header = header });
    
        int newCount = Columns.Count;
        foreach (var row in Rows)
        {
            var temp = row.Values;          // 1. Читаем свойство в локальную переменную
            Array.Resize(ref temp, newCount); // 2. Передаём её по ссылке
            row.Values = temp;              // 3. Записываем результат обратно
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Columns)));
    }

    public void AddRow(string id, object[]? initialValues)
    {
        _rowIdToIndex[id] = Rows.Count;
        Rows.Add(new RowRecord { Id = id, Values = initialValues ?? new object[ColumnCount] });
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rows)));
    }

    public void SetRowVisibility(string rowId, bool visible)
    {
        if (_rowIdToIndex.TryGetValue(rowId, out var idx))
            Rows[idx].IsVisible = visible;
    }

    public void SetColumnVisibility(string colId, bool visible)
    {
        if (_colIdToIndex.TryGetValue(colId, out var idx))
        {
            Columns[idx].IsVisible = visible;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Columns)));
        }
    }

    public void DeleteRow(string rowId)
    {
        if (!_rowIdToIndex.Remove(rowId, out var idx)) return;
        Rows.RemoveAt(idx);
        _rowIdToIndex.Clear();
        for (int i = 0; i < Rows.Count; i++) _rowIdToIndex[Rows[i].Id] = i;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rows)));
    }

    public void DeleteColumn(string colId)
    {
        if (!_colIdToIndex.Remove(colId, out var idx)) return;
    
        Columns.RemoveAt(idx);
        _colIdToIndex.Clear();
        for (var i = 0; i < Columns.Count; i++) _colIdToIndex[Columns[i].Id] = i;

        var newCount = Columns.Count;
        foreach (var row in Rows)
        {
            // Оптимизация: сразу создаём массив нужного размера, 
            // избегая двойной аллокации (Array.Copy + Array.Resize)
            var newArray = new object[newCount];
        
            if (idx > 0) 
                Array.Copy(row.Values, 0, newArray, 0, idx); // Данные до удаляемой колонки
            
            int remaining = row.Values.Length - idx - 1;
            if (remaining > 0) 
                Array.Copy(row.Values, idx + 1, newArray, idx, remaining); // Данные после
            
            row.Values = newArray;
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Columns)));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}