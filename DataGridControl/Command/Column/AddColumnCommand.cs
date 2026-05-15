using System.Data;
using DataGridControl.View;

namespace DataGridControl.Command.Column;

public class AddColumnCommand(
    VeiwModel vm, 
    // DataTable vm,
    DataColumn column)
    : IUndoRedoCommand
{
    public string Description => $"Добавление колонки в позицию {column}";

    public void Undo()
    {
        // vm.Columns.Remove(column);
        // vm.IsUndoRedoInProgress = true;
        // vm.DeleteColumn(column.Id);
        // dataGridColumn.Visibility = Visibility.Hidden;
        // vm.IsUndoRedoInProgress = false;
    }

    public void Redo()
    {
        var asdf = vm.TableData.Copy();
        vm.AddColumn<string>(column.ColumnName, asdf);
        // vm.Columns.Add(column);
        // vm.IsUndoRedoInProgress = true;
        // vm.AddColumn(column.Id, column.Header);
        // dataGridColumn.Visibility = Visibility.Visible;
        // vm.IsUndoRedoInProgress = false;
    }
}