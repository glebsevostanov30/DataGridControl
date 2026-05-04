using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using DataGridControl.Command;
using DataGridControl.Command.Column;
using DataGridControl.Model;
using DataGridControl.Service;

namespace DataGridControl.View.Dialog;

public partial class SelectionColumnsDialogViewModel(SpreadsheetModel mainModel)
{
    private readonly CommandHistory _history = CommandHistory.instance;

    public SpreadsheetModel mainModel { get; } = mainModel;

    [RelayCommand]
    private void SelectAll()
    {
        var commands = new Collection<IUndoRedoCommand>();
        
        foreach (var item in mainModel.Columns)
        {
            var command = new AddColumnCommand(mainModel, item, item.DisplayIndex);
            commands.Add(command);
        }
        
        _history.ExecuteGroup(commands, "Выбор всех колонок");
    }
    
    [RelayCommand]
    private void UnselectAll()
    {
        var commands = new Collection<IUndoRedoCommand>();
        
        foreach (var item in mainModel.Columns)
        {
            var command = new DeleteColumnCommand(mainModel, item, item.DisplayIndex);
            commands.Add(command);
        }
        
        _history.ExecuteGroup(commands, "Выбор всех колонок");
    }
    
    [RelayCommand]
    private void Choose(DataGridColumn dataGridColumn)
    {
        if (dataGridColumn.Visibility == Visibility.Visible)
        {
            var addCommand = new AddColumnCommand(mainModel, dataGridColumn, dataGridColumn.DisplayIndex);
            _history.Execute(addCommand);
        }
        
        var deleteColumnCommand = new DeleteColumnCommand(mainModel, dataGridColumn, dataGridColumn.DisplayIndex);
        _history.Execute(deleteColumnCommand);
    }
    
    [RelayCommand]
    private void Undo()
    {
        _history.Undo();
    }

    [RelayCommand]
    private void Redo()
    {
        _history.Redo();
    }
}