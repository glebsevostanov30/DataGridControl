// using DataGridControl.Model;
//
// namespace DataGridControl.Command.Row;
//
// public class DeleteRowCommand(
//     SpreadsheetModel vm,
//     RowRecord rowRecord)
//     : IUndoRedoCommand
// {
//     public string Description => $"Удаление строки в позиции {rowRecord.Id}";
//
//     public void Undo()
//     {
//         vm.IsUndoRedoInProgress = true;
//         vm.AddRow(rowRecord.Id, rowRecord.Values);
//         vm.IsUndoRedoInProgress = false;
//     }
//
//     public void Redo()
//     {
//         vm.IsUndoRedoInProgress = true;
//         vm.DeleteRow(rowRecord.Id);
//         vm.IsUndoRedoInProgress = false;
//     }
// }