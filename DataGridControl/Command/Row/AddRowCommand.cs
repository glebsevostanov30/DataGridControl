using System.Data;
using DataGridControl.View;

namespace DataGridControl.Command.Row;

public class AddRowCommand(
    VeiwModel vm, 
    // RowRecord row
    DataRow row
    )
    : IUndoRedoCommand
{
    public string Description => $"Добавление строки в позицию {row}";

    public void Undo()
    {
        // vm.Rows.Remove(row);
        // vm.DeleteRow(row.Id);
        // vm.IsUndoRedoInProgress = true;
        // vm.RemoveRow(_row);
        // vm.IsUndoRedoInProgress = false;
    }

    public void Redo()
    {
        vm.AddRow(vm.TableData, null);
        // vm.IsUndoRedoInProgress = true;
        // vm.AddRow(row.Id, row.Values);
        // vm.IsUndoRedoInProgress = false;
    }
}