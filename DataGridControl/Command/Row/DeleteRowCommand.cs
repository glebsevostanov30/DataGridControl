using System.Data;
using DataGridControl.View;

namespace DataGridControl.Command.Row;

public class DeleteRowCommand(
    ViewModel vm,
    // DataRowView? row
    IList<DataRowView> selectedRows
    )
    : IUndoRedoCommand
{
    public string Description => $"Удаление строки в позиции ";
    private DataTable _dataTable;

    public void Undo()
    {
        vm.ReturnDataTable(_dataTable);
        // vm.IsUndoRedoInProgress = true;
        // vm.ReturnRow(row);
        // vm.IsUndoRedoInProgress = false;
    }

    public void Redo()
    {
        // vm.IsUndoRedoInProgress = true;
        // vm.RemoveRow(row);
        _dataTable = vm.TableData.Copy();
        vm.CopyUnselectedRows(selectedRows);
        // vm.IsUndoRedoInProgress = false;
    }
}