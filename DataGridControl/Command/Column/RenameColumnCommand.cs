using System.Data;
using DataGridControl.View;

namespace DataGridControl.Command.Column;

public class RenameColumnCommand(
    ViewModel vm, 
    // DataTable vm,
    string oldName,
    string newName)
    : IUndoRedoCommand
{
    public string Description => $"Добавление колонки в позицию {newName}";

    public void Undo()
    {
        vm.RenameColumn(newName, oldName);
        // vm.Columns.Remove(column);
        // vm.IsUndoRedoInProgress = true;
        // vm.DeleteColumn(column.Id);
        // dataGridColumn.Visibility = Visibility.Hidden;
        // vm.IsUndoRedoInProgress = false;
    }

    public void Redo()
    {
        vm.RenameColumn(oldName, newName);
        // vm.Columns.Add(column);
        // vm.IsUndoRedoInProgress = true;
        // vm.AddColumn(column.Id, column.Header);
        // dataGridColumn.Visibility = Visibility.Visible;
        // vm.IsUndoRedoInProgress = false;
    }
}