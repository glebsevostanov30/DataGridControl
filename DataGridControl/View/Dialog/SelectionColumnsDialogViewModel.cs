using System.Data;
using CommunityToolkit.Mvvm.Input;
using DataGridControl.Service;

namespace DataGridControl.View.Dialog;

public partial class SelectionColumnsDialogViewModel()
{
    private readonly CommandHistory _history = CommandHistory.instance;


    [RelayCommand]
    private void SelectAll()
    {
        // var commands = new Collection<IUndoRedoCommand>();
        //
        // foreach (var item in mainModel.Columns)
        // {
        //     var command = new AddColumnCommand(mainModel, item, item.DisplayIndex);
        //     commands.Add(command);
        // }
        //
        // _history.ExecuteGroup(commands, "Выбор всех колонок");
    }
    
    [RelayCommand]
    private void UnselectAll()
    {
        // var commands = new Collection<IUndoRedoCommand>();
        //
        // foreach (var item in mainModel.Columns)
        // {
        //     var command = new DeleteColumnCommand(mainModel, item, item.DisplayIndex);
        //     commands.Add(command);
        // }
        //
        // _history.ExecuteGroup(commands, "Выбор всех колонок");
    }
    
    [RelayCommand]
    private void Choose(DataColumn dataGridColumn)
    {
        // var deleteColumnCommand = new DeleteColumnCommand(mainModel, dataGridColumn);
        // _history.Execute(deleteColumnCommand);
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