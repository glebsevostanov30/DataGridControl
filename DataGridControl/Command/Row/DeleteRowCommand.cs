using System.Data;
using DataGridControl.Model;
using DataGridControl.View;

namespace DataGridControl.Command.Row;

public class DeleteRowCommand(
    ViewModel vm,
    DataRowView? row
    )
    : IUndoRedoCommand
{
    public string Description => $"Удаление строки в позиции {row}";

    public void Undo()
    {
        // vm.IsUndoRedoInProgress = true;
        vm.ReturnRow(row);
        // vm.IsUndoRedoInProgress = false;
    }

    public void Redo()
    {
        // vm.IsUndoRedoInProgress = true;
        vm.RemoveRow(row);
        // vm.IsUndoRedoInProgress = false;
    }
}