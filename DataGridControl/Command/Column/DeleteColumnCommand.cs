using System.Windows;
using System.Windows.Controls;
using DataGridControl.Model;

namespace DataGridControl.Command.Column;

public class DeleteColumnCommand(SpreadsheetModel vm,
    DataGridColumn dataGridColumn,
    int index)
    : IUndoRedoCommand
{
    public string Description => $"Удаление колонки колонки в позицию {index}";
    
    public void Undo()
    {
        vm.IsUndoRedoInProgress = true;
        dataGridColumn.Visibility = Visibility.Visible;
        vm.IsUndoRedoInProgress = false;
    }
    public void Redo()
    {
        vm.IsUndoRedoInProgress = true;
        dataGridColumn.Visibility = Visibility.Hidden;
        vm.IsUndoRedoInProgress = false;
    }
}