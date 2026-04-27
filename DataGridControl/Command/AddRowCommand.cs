using DataGridControl.Model;
using DataGridControl.View;

namespace DataGridControl.Command
{
    public class AddRowCommand(SpreadsheetViewModel vm, RowData newRow, int index) : IUndoRedoCommand
    {
        public string Description => $"Добавление строки в позицию {index}";

        public void Undo()
        {
            vm.IsUndoRedoInProgress = true;
            vm.Rows.RemoveAt(index);
            vm.IsUndoRedoInProgress = false;
        }

        public void Redo()
        {
            vm.IsUndoRedoInProgress = true;
            vm.Rows.Insert(index, newRow);
            vm.IsUndoRedoInProgress = false;
        }
    }
}