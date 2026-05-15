// using System.Data;
// using DataGridControl.Model;
//
// namespace DataGridControl.Command.Column;
//
// public class DeleteColumnCommand(
//     SpreadsheetModel vm,
//     DataColumn dataColumn)
//     : IUndoRedoCommand
// {
//     public string Description => $"Удаление колонки колонки в позицию {dataColumn.Ordinal}";
//
//     public void Undo()
//     {
//         // dataColumn.Visibility(Visibility.Hidden);
//         // vm.IsUndoRedoInProgress = true;
//         // vm.AddColumn(dataColumn);
//         // vm.IsUndoRedoInProgress = false;
//     }
//
//     public void Redo()
//     {
//         // vm.IsUndoRedoInProgress = true;
//         // vm.RemoveColumn(dataColumn);
//         // vm.IsUndoRedoInProgress = false;
//     }
// }