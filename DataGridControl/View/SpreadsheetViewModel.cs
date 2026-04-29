using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DataGridControl.Command;
using DataGridControl.Command.Row;
using DataGridControl.Model;

namespace DataGridControl.View;

public partial class SpreadsheetViewModel : INotifyPropertyChanged
{
    public ObservableCollection<RowData> Rows { get; }
    private readonly Stack<IList<IUndoRedoCommand>> _undoStack = new();
    private readonly Stack<IList<IUndoRedoCommand>> _redoStack = new();
    

    public bool IsUndoRedoInProgress
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public ObservableCollection<RowData> SelectedRows
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    } = [];
    
    public ObservableCollection<DataGridColumn> SelectedColumns
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = [];

    public SpreadsheetViewModel()
    {
        Rows = [];
        AddNewRowInternal("A1", "B1", "C1");
        AddNewRowInternal("A2", "B2", "C2");
        AddNewRowInternal("A3", "B3", "C3");
    }

    private void AddNewRowInternal(string c1, string c2, string c3)
    {
        var newRow = new RowData { Col1 = c1, Col2 = c2, Col3 = c3 };
        Rows.Add(newRow);
    }


    [RelayCommand]
    private void AddRow()
    {
        var newRow = new RowData { Col1 = "", Col2 = "", Col3 = "" };
        var command = new AddRowCommand(this, newRow, Rows.Count);
        ExecuteCommand(new List<IUndoRedoCommand> { command });
        // Применяем сразу Redo, чтобы добавить строку
        command.Redo();
    }

    [RelayCommand]
    private void DeleteRows()
    {
        var itemsToDelete = SelectedRows.ToList();
        if (itemsToDelete.Count == 0) return;
        var commands = new List<IUndoRedoCommand>();
        foreach (var item in itemsToDelete)
        {
            var index = Rows.IndexOf(item);
            var command = new DeleteRowCommand(this, item, index);
            commands.Add(command);
            command.Redo();
        }

        ExecuteCommand(commands);

        SelectedRows.Clear();
    }

    private void ExecuteCommand(IList<IUndoRedoCommand> command)
    {
        _undoStack.Push(command);
        _redoStack.Clear();
        CommandManager.InvalidateRequerySuggested();
    }

    [RelayCommand]
    private void Undo()
    {
        if (_undoStack.Count == 0) return;
        var commands = _undoStack.Pop();
        foreach (var command in commands.Reverse())
        {
            command.Undo();
        }

        _redoStack.Push(commands);
        CommandManager.InvalidateRequerySuggested();
    }

    [RelayCommand]
    private void Redo()
    {
        if (_redoStack.Count == 0) return;
        var commands = _redoStack.Pop();

        foreach (var command in commands)
        {
            command.Redo();
        }

        _undoStack.Push(commands);
        CommandManager.InvalidateRequerySuggested();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}