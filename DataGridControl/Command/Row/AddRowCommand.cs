using System.Data;
using DataGridControl.View;

namespace DataGridControl.Command.Row;

public class AddRowCommand(
    ViewModel vm )
    : IUndoRedoCommand
{
    private DataRowView? _row;
    public string Description => $"Добавление строки в позицию {_row}";

    public void Undo()
    {
        vm.RemoveRow(_row);
        // vm.DeleteRow(row.Id);
        // vm.IsUndoRedoInProgress = true;
        // vm.RemoveRow(_row);
        // vm.IsUndoRedoInProgress = false;
    }

    public void Redo()
    {
        vm.AddRow(null);
        // vm.IsUndoRedoInProgress = true;
        // vm.AddRow(row.Id, row.Values);
        // vm.IsUndoRedoInProgress = false;
    }
}