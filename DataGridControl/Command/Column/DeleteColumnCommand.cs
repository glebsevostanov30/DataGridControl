using System.Data;
using System.Windows.Controls;
using DataGridControl.Model;
using DataGridControl.View;

namespace DataGridControl.Command.Column;

public class DeleteColumnCommand(
    ViewModel vm,
    DataGridColumn dataColumn
    )
    : IUndoRedoCommand
{
    public string Description => $"Удаление колонки колонки в позицию {dataColumn.Header}";
    private DataTable? _dataTable;

    public void Undo()
    {
        if(_dataTable == null) return;
        vm.TableData = _dataTable;
        // dataColumn.Visibility(Visibility.Hidden);
        // vm.IsUndoRedoInProgress = true;
        // vm.AddColumn(dataColumn);
        // vm.IsUndoRedoInProgress = false;
    }

    public void Redo()
    {
        _dataTable = vm.TableData.Copy();
        vm.RemoveColumn(dataColumn);
        // vm.IsUndoRedoInProgress = true;
        // vm.RemoveColumn(dataColumn);
        // vm.IsUndoRedoInProgress = false;
    }
}