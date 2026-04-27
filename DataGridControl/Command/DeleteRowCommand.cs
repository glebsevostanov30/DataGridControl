using DataGridControl.Command;
using DataGridControl.Model;
using DataGridControl.View;

namespace DataGridControl.Command
{
    public class DeleteRowCommand(SpreadsheetViewModel vm,
        RowData deletedRow,
        int index)
        : IUndoRedoCommand
    {
        private readonly RowData _deletedRow = deletedRow.Clone();

        public string Description => $"Удаление строки в позиции {index}";

        public void Undo()
        {
            vm.IsUndoRedoInProgress = true;
            vm.Rows.Insert(index, _deletedRow.Clone());
            vm.IsUndoRedoInProgress = false;
        }

        public void Redo()
        {
            vm.IsUndoRedoInProgress = true;
            vm.Rows.RemoveAt(index);
            vm.IsUndoRedoInProgress = false;
        }
    }
}